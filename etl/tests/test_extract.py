import io
import json
import shutil
import subprocess
import sys
from datetime import datetime, timezone
from urllib.error import HTTPError, URLError
from urllib.parse import parse_qs, urlparse

import pytest
from hydrodatalab.extract import _request_json, extract, fixture_dir


def test_snapshot_hash_tampering_rejected(tmp_path):
    shutil.copytree(fixture_dir(), tmp_path / "fixture")
    (tmp_path / "fixture" / "demand.json").write_text("{}", encoding="utf-8")
    with pytest.raises(ValueError, match="intégrité"):
        extract(directory=tmp_path / "fixture")


def test_live_uses_previous_local_day_and_pagination():
    calls = []
    def fetch(url):
        calls.append(url)
        query = parse_qs(urlparse(url).query)
        assert "2026-11-01T00:00:00-04:00" in query["where"][0]
        assert "2026-11-02T00:00:00-05:00" in query["where"][0]
        offset = int(query["offset"][0])
        count = 125 if "demande-electricite" in url else 75
        return {"total_count": count, "results": [{"index": i} for i in range(offset, min(offset + 100, count))]}
    bundle = extract("live", now=datetime(2026, 11, 2, 15, tzinfo=timezone.utc), fetch=fetch)
    assert len(calls) == 3
    assert len(bundle.demand) == 125 and len(bundle.hydrometeo) == 75
    assert bundle.period["start"] == "2026-11-01"
    assert all(item["license"] == "CC BY-NC 4.0" for item in bundle.provenance)


def test_explicit_live_day_is_used_without_silent_replacement():
    calls = []
    def fetch(url):
        calls.append(url)
        where = parse_qs(urlparse(url).query)["where"][0]
        assert "2026-03-08T00:00:00-05:00" in where
        assert "2026-03-09T00:00:00-04:00" in where
        return {"results": [{"example": 1}], "total_count": 1}
    bundle = extract("live", day="2026-03-08", now=datetime(2026, 10, 18, 20, tzinfo=timezone.utc), fetch=fetch)
    assert bundle.period == {"start": "2026-03-08", "end": "2026-03-09", "timezone": "America/Toronto"}
    assert len(calls) == 2


@pytest.mark.parametrize("day", ["20261017", "2026-02-30", "tomorrow", "2026-10-18", "2026-10-19"])
def test_bad_or_unfinished_day_is_rejected(day):
    with pytest.raises(ValueError):
        extract("live", day=day, now=datetime(2026, 10, 18, 20, tzinfo=timezone.utc), fetch=lambda _: pytest.fail("aucun réseau"))


def test_fixture_accepts_its_day_and_rejects_another_completed_day():
    now = datetime(2026, 10, 18, 20, tzinfo=timezone.utc)
    assert len(extract(day="2026-10-17", now=now).demand) == 96
    with pytest.raises(ValueError, match="uniquement le 2026-10-17"):
        extract(day="2026-10-16", now=now)


def test_live_empty_or_incomplete_pages_fail_without_fixture_fallback():
    now = datetime(2026, 10, 18, 20, tzinfo=timezone.utc)
    with pytest.raises(ValueError, match="Aucune donnée"):
        extract("live", now=now, fetch=lambda url: {"results": [], "total_count": 0})
    with pytest.raises(ValueError, match="incomplète"):
        extract("live", now=now, fetch=lambda url: {"results": [], "total_count": 5})


def test_network_transient_error_retries_and_permanent_error_does_not():
    attempts, waits = [], []
    def transient(request, timeout):
        attempts.append(request.full_url)
        assert timeout == 20
        if len(attempts) < 3:
            raise URLError("offline")
        return io.BytesIO(b'{"results": []}')
    assert _request_json("https://example.test", opener=transient, sleeper=waits.append) == {"results": []}
    assert waits == [0.5, 1.0]
    def permanent(request, timeout):
        raise HTTPError(request.full_url, 404, "missing", {}, None)
    with pytest.raises(RuntimeError, match="HTTP 404"):
        _request_json("https://example.test", opener=permanent, sleeper=lambda _: pytest.fail("must not retry"))
    def offline(request, timeout):
        raise URLError("offline")
    with pytest.raises(RuntimeError, match="mode fixture"):
        _request_json("https://example.test", opener=offline, sleeper=lambda _: None)


def test_standalone_extractor_imports_no_transform_modules(tmp_path):
    code = "import sys; from hydrodatalab.extract import main; main(['--source','fixture','--date','2026-10-17','--output',sys.argv[1]]); assert not any(x in sys.modules for x in ('pandas','hydrodatalab.pipeline','hydrodatalab.normalize','hydrodatalab.coverage'))"
    subprocess.run([sys.executable, "-c", code, str(tmp_path)], check=True, capture_output=True)
    assert len(extract(directory=tmp_path).demand) == 96
