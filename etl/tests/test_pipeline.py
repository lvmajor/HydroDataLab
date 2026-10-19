import copy
import errno
import json
import subprocess
import sys
from concurrent.futures import ThreadPoolExecutor
from threading import Barrier, Event
from datetime import datetime, timezone
from pathlib import Path
from uuid import uuid4

import pandas as pd
import pytest
from fastapi.testclient import TestClient

from hydrodatalab.api import app
from hydrodatalab.extract import extract
from hydrodatalab.normalize import normalize
from hydrodatalab.pipeline import artifact_path, run_pipeline

NOW = datetime(2026, 10, 18, 20, tzinfo=timezone.utc)


def test_real_fixture_roundtrip_csv_parquet_and_report(tmp_path):
    report = run_pipeline(output=tmp_path, now=NOW)
    assert report["summary"] == {"observationCount": 168, "seriesCount": 4, "invalidCount": 0, "duplicateCount": 0}
    assert report["period"] == {"start": "2026-10-17", "end": "2026-10-18", "timezone": "America/Toronto"}
    for series in report["series"]:
        assert series["coverage"]["percent"] == 100
        assert series["points"] == sorted(series["points"], key=lambda point: point["timestamp"])
        assert all(point["timestamp"].endswith("Z") for point in series["points"])
    directory = tmp_path / report["runId"]
    assert sorted(file.name for file in directory.iterdir()) == ["hourly.csv", "observations.csv", "observations.parquet", "report.json"]
    csv = pd.read_csv(directory / "observations.csv", parse_dates=["timestamp_utc"])
    parquet = pd.read_parquet(directory / "observations.parquet")
    pd.testing.assert_frame_equal(csv, parquet, check_dtype=False)
    assert json.loads((directory / "report.json").read_text(encoding="utf-8")) == report


def test_missing_scenario_never_changes_snapshot(tmp_path):
    before = extract()
    report = run_pipeline(output=tmp_path, now=NOW, quality_scenario="missing")
    water = next(item for item in report["series"] if item["id"] == "water_level")
    assert water["coverage"] == {"observed": 23, "expected": 24, "percent": 95.83}
    assert report["summary"]["observationCount"] == 167
    assert "copie en mémoire" in report["warnings"][0]
    assert extract() == before


def test_zero_null_invalid_and_duplicate_have_distinct_meanings():
    bundle = copy.deepcopy(extract())
    bundle.demand[0]["valeurs_demandetotal"] = 0
    bundle.demand[1]["valeurs_demandetotal"] = None
    bundle.demand[2]["valeurs_demandetotal"] = "not-a-number"
    duplicate = copy.deepcopy(bundle.demand[3])
    duplicate["valeurs_demandetotal"] = 123
    bundle.demand.append(duplicate)
    frame, stats, warnings = normalize(bundle, now=NOW)
    demand = frame.loc[frame.series_id == "demand"].reset_index(drop=True)
    assert demand.iloc[0].value == 0
    assert pd.isna(demand.iloc[1].value) and pd.isna(demand.iloc[2].value)
    assert demand.iloc[3].value == 123
    assert stats == {"invalidCount": 1, "duplicateCount": 1}
    assert len(warnings) == 3


def test_dates_require_offset_future_and_wrong_unit_excluded():
    bundle = copy.deepcopy(extract())
    bundle.demand[0]["date"] = "2026-10-17T00:00:00"
    bundle.demand[1]["date"] = "2026-10-19T00:00:00-04:00"
    bundle.hydrometeo[0]["composition_depil_nom_unite_mesure"] = "MW"
    frame, stats, _ = normalize(bundle, now=NOW)
    assert stats["invalidCount"] == 3
    assert len(frame) == 165
    future_frame, future_stats, _ = normalize(extract(), now=datetime(2026, 10, 17, 5, tzinfo=timezone.utc))
    assert future_stats["invalidCount"] > 0
    assert future_frame.timestamp_utc.max() <= pd.Timestamp("2026-10-17T05:00:00Z")


def test_atomic_failure_leaves_no_published_or_staged_run(tmp_path, monkeypatch):
    def fail(*args, **kwargs):
        raise OSError("disque simulé indisponible")
    monkeypatch.setattr(pd.DataFrame, "to_parquet", fail)
    with pytest.raises(OSError, match="disque"):
        run_pipeline(output=tmp_path, now=NOW)
    assert list(tmp_path.iterdir()) == []


def test_idempotence_conflict_and_filename_validation(tmp_path):
    run_id = str(uuid4())
    first = run_pipeline(output=tmp_path, run_id=run_id, now=NOW)
    assert run_pipeline(output=tmp_path, run_id=run_id, now=NOW) == first
    with pytest.raises(ValueError, match="paramètres"):
        run_pipeline(output=tmp_path, run_id=run_id, quality_scenario="missing", now=NOW)
    with pytest.raises(ValueError):
        artifact_path("../../outside", "report.json", tmp_path)
    with pytest.raises(ValueError):
        artifact_path(run_id, "../../secret", tmp_path)


def test_replay_preserves_artifact_bytes_and_rejects_source_change_before_extract(tmp_path):
    run_id = str(uuid4())
    first = run_pipeline(output=tmp_path, run_id=run_id, now=NOW)
    directory = tmp_path / run_id
    before = {path.name: (path.read_bytes(), path.stat().st_mtime_ns) for path in directory.iterdir()}
    def forbidden_extract(*args, **kwargs):
        pytest.fail("Un résultat déjà publié ne doit pas être extrait à nouveau")
    assert run_pipeline(output=tmp_path, run_id=run_id, extractor=forbidden_extract) == first
    with pytest.raises(ValueError, match="paramètres"):
        run_pipeline("live", output=tmp_path, run_id=run_id, extractor=forbidden_extract)
    with pytest.raises(ValueError, match="paramètres"):
        run_pipeline(output=tmp_path, run_id=run_id, quality_scenario="missing", extractor=forbidden_extract)
    with pytest.raises(ValueError, match="paramètres"):
        run_pipeline(output=tmp_path, run_id=run_id, day="2026-10-16", extractor=forbidden_extract)
    assert run_pipeline(output=tmp_path, run_id=run_id, day="2026-10-17", extractor=forbidden_extract) == first
    assert {path.name: (path.read_bytes(), path.stat().st_mtime_ns) for path in directory.iterdir()} == before


def test_concurrent_same_uuid_is_idempotent_on_windows_and_linux(tmp_path, monkeypatch):
    run_id, ready = str(uuid4()), Barrier(2)
    rename = Path.rename
    def posix_rename(path, destination):
        try:
            return rename(path, destination)
        except FileExistsError as error:
            # Linux renvoie ENOTEMPTY pour un répertoire cible déjà publié.
            raise OSError(errno.ENOTEMPTY, "Directory not empty") from error
    monkeypatch.setattr(Path, "rename", posix_rename)
    def synchronized_extract(*args, **kwargs):
        bundle = extract(*args, **kwargs)
        ready.wait(timeout=5)
        return bundle
    with ThreadPoolExecutor(max_workers=2) as executor:
        pending = [executor.submit(run_pipeline, output=tmp_path, run_id=run_id, now=NOW, extractor=synchronized_extract) for _ in range(2)]
        reports = [job.result(timeout=10) for job in pending]
    assert reports[0] == reports[1]
    assert [path.name for path in tmp_path.iterdir()] == [run_id]


def test_missing_scenario_removes_a_valid_value_even_if_first_raw_value_is_null(tmp_path):
    bundle = copy.deepcopy(extract())
    first_water = next(row for row in bundle.hydrometeo if row["identifiant"] == "1-15412")
    first_water["valeur"] = None
    def sample(*args, **kwargs):
        return bundle
    report = run_pipeline(output=tmp_path, quality_scenario="missing", now=NOW, extractor=sample)
    water = next(series for series in report["series"] if series["id"] == "water_level")
    assert water["coverage"] == {"observed": 22, "expected": 24, "percent": 91.67}
    assert any(point["value"] is None for point in water["points"])
    assert len(bundle.hydrometeo) == 72


def test_http_cli_share_exact_same_observations(tmp_path, monkeypatch):
    http_root, cli_root = tmp_path / "http", tmp_path / "cli"
    monkeypatch.setenv("HYDRO_OUTPUT_DIR", str(http_root))
    client = TestClient(app)
    assert client.get("/health").json() == {"status": "ok"}
    run_id = str(uuid4())
    response = client.post("/run", json={"runId": run_id, "source": "fixture"})
    assert response.status_code == 200
    command = [sys.executable, "-m", "hydrodatalab", "run", "--source", "fixture", "--date", "2026-10-17", "--output", str(cli_root), "--run-id", run_id]
    result = subprocess.run(command, check=True, capture_output=True, text=True, encoding="utf-8")
    assert json.loads(result.stdout)["summary"] == response.json()["summary"]
    assert (http_root / run_id / "observations.csv").read_bytes() == (cli_root / run_id / "observations.csv").read_bytes()
    pd.testing.assert_frame_equal(pd.read_parquet(http_root / run_id / "observations.parquet"), pd.read_parquet(cli_root / run_id / "observations.parquet"))
    assert client.get(f"/artifacts/{run_id}/observations.csv").status_code == 200
    assert client.get(f"/artifacts/{run_id}/secret.txt").status_code == 400
    assert client.get(f"/artifacts/{uuid4()}/report.json").status_code == 404
    assert client.post("/run", json={"runId": str(uuid4()), "simulateFailure": True}).status_code == 503
    assert client.post("/run", json={"runId": "../bad"}).status_code == 422


def test_busy_run_rejected_and_health_responsive_during_synchronous_job(monkeypatch):
    import hydrodatalab.api as api
    started, release = Event(), Event()
    calls = []
    def waiting_pipeline(*args, **kwargs):
        calls.append(kwargs["run_id"])
        started.set()
        assert release.wait(timeout=5)
        return {"summary": {"observationCount": 0}}
    monkeypatch.setattr(api, "run_pipeline", waiting_pipeline)
    with TestClient(app) as client, ThreadPoolExecutor(max_workers=1) as executor:
        pending = executor.submit(client.post, "/run", json={"runId": str(uuid4())})
        try:
            assert started.wait(timeout=3)
            assert client.get("/health").json() == {"status": "ok"}
            busy = client.post("/run", json={"runId": str(uuid4())})
            assert busy.status_code == 409
            assert "déjà en cours" in busy.json()["detail"]
            assert len(calls) == 1
        finally:
            release.set()
        assert pending.result(timeout=3).status_code == 200
        assert client.post("/run", json={"runId": str(uuid4())}).status_code == 200
        assert len(calls) == 2


def test_run_lock_released_after_pipeline_failure(monkeypatch):
    import hydrodatalab.api as api
    calls = []
    def fail_then_succeed(*args, **kwargs):
        calls.append(kwargs["run_id"])
        if len(calls) == 1:
            raise RuntimeError("panne simulée")
        return {"summary": {"observationCount": 0}}
    monkeypatch.setattr(api, "run_pipeline", fail_then_succeed)
    with TestClient(app) as client:
        assert client.post("/run", json={"runId": str(uuid4())}).status_code == 503
        assert client.post("/run", json={"runId": str(uuid4())}).status_code == 200
