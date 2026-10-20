"""Development check only: compare the Python and .NET engines on identical snapshots.

Run after building src/Etl: uv run --project etl python scripts/verify-etl-parity.py
The .NET-only preparation and test scripts do not require this check or Python.
"""
from __future__ import annotations

import argparse
import copy
import hashlib
import json
import os
from pathlib import Path
import subprocess
import sys
import tempfile
from datetime import datetime, timedelta, timezone
from uuid import uuid4
from zoneinfo import ZoneInfo

import pandas as pd
import pyarrow as pa
import pyarrow.parquet as pq

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / "etl"))
from hydrodatalab.extract import extract  # noqa: E402
from hydrodatalab.pipeline import run_pipeline  # noqa: E402


def snapshot(folder: Path, *, day: str | None = None, anomalies: bool = False):
    manifest = json.loads((ROOT / "data/fixtures/manifest.json").read_text(encoding="utf-8-sig"))
    demand = json.loads((ROOT / "data/fixtures/demand.json").read_text(encoding="utf-8"))["results"]
    hydro = json.loads((ROOT / "data/fixtures/hydrometeo.json").read_text(encoding="utf-8"))["results"]
    if day:
        zone = ZoneInfo("America/Toronto")
        local_start = datetime.fromisoformat(day).replace(tzinfo=zone)
        local_end = local_start + timedelta(days=1)
        start, end = local_start.astimezone(timezone.utc), local_end.astimezone(timezone.utc)
        hours = int((end - start).total_seconds() / 3600)
        demand = [{"date": (start + timedelta(minutes=15 * i)).isoformat(), "valeurs_demandetotal": 100.0} for i in range(hours * 4)]
        hydro = []
        for identifier, kind, unit, value in [("1-15412", "Niveau", "m", 10), ("1-10622", "Température Minimum", "°C", -2), ("1-10622", "Température Maximum", "°C", 3)]:
            hydro.extend({"date": (start + timedelta(hours=i)).isoformat(), "identifiant": identifier,
                "composition_depil_type_point_donnee": kind, "composition_depil_nom_unite_mesure": unit,
                "valeur": value + i / 100} for i in range(hours))
        manifest["period"] = {"start": day, "end": local_end.date().isoformat(), "timezone": "America/Toronto"}
    if anomalies:
        for index, value in enumerate([0, None, "", True, "NaN", "invalid", "2.5"]):
            demand[index]["valeurs_demandetotal"] = value
        demand.append({**demand[7], "valeurs_demandetotal": 25000})
        demand.extend([{**demand[0], "date": "2026-10-17T00:00:00"}, {**demand[0], "date": "2099-01-01T00:00:00Z"}])
        first_water = next(row for row in hydro if row["identifiant"] == "1-15412")
        first_water["valeur"] = None
        hydro.append({**first_water, "composition_depil_nom_unite_mesure": "cm"})
    folder.mkdir()
    for name, rows in [("demand.json", demand), ("hydrometeo.json", hydro)]:
        raw = json.dumps({"total_count": len(rows), "results": rows}, ensure_ascii=False).encode("utf-8")
        (folder / name).write_bytes(raw)
        source = next(item for item in manifest["sources"] if item["file"] == name)
        source.update(sha256=hashlib.sha256(raw).hexdigest(), recordCount=len(rows))
    (folder / "manifest.json").write_text(json.dumps(manifest, ensure_ascii=False), encoding="utf-8")


def stable(report):
    value = copy.deepcopy(report)
    value.pop("generatedAtUtc")
    for step in value["execution"]["steps"]:
        for key in ("startedAtUtc", "completedAtUtc", "durationMs"):
            step.pop(key)
    return value


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--dotnet-dll", type=Path, default=ROOT / "src/Etl/bin/Debug/net10.0/HydroDataLab.Etl.dll")
    args = parser.parse_args()
    if not args.dotnet_dll.is_file():
        parser.error("Compiler src/Etl avant la comparaison.")
    results = []
    with tempfile.TemporaryDirectory(prefix="hydro-parity-") as directory:
        temporary = Path(directory)
        for name, day, anomalies in [("fixture", None, False), ("anomalies", None, True), ("spring-23h", "2026-03-08", False), ("fall-25h", "2025-11-02", False)]:
            fixture = temporary / name
            snapshot(fixture, day=day, anomalies=anomalies)
            for quality in ("complete", "missing"):
                run_id = str(uuid4())
                python_root, dotnet_root = temporary / "python", temporary / "dotnet"
                report = run_pipeline(output=python_root, run_id=run_id, quality_scenario=quality,
                    extractor=lambda source, **kwargs: extract(source, directory=fixture, **kwargs))
                subprocess.run(["dotnet", str(args.dotnet_dll), "run", "--source", "fixture", "--run-id", run_id,
                    "--quality-scenario", quality, "--output", str(dotnet_root)], check=True, capture_output=True, text=True,
                    env={**os.environ, "HYDRO_FIXTURE_DIR": str(fixture)})
                dotnet = json.loads((dotnet_root / run_id / "report.json").read_text(encoding="utf-8"))
                assert stable(report) == stable(dotnet), f"Rapports différents : {name}/{quality}"
                for artifact in ("observations.csv", "hourly.csv"):
                    assert (python_root / run_id / artifact).read_bytes() == (dotnet_root / run_id / artifact).read_bytes(), f"CSV différent : {name}/{quality}/{artifact}"
                python_table = pq.read_table(python_root / run_id / "observations.parquet")
                dotnet_table = pq.read_table(dotnet_root / run_id / "observations.parquet")
                # pandas adds Arrow large_string and pandas-specific metadata. Both
                # files must retain the contract's nullable logical types and values.
                canonical = pa.schema([("series_id", pa.string()), ("timestamp_utc", pa.timestamp("us", tz="UTC")),
                    ("value", pa.float64()), ("unit", pa.string()), ("station", pa.string())])
                for table in (python_table, dotnet_table):
                    assert table.column_names == canonical.names
                    assert table.schema.field("timestamp_utc").type == canonical.field("timestamp_utc").type
                    assert table.schema.field("value").type == pa.float64()
                    assert all(field.nullable for field in table.schema)
                    assert all(pa.types.is_string(table.schema.field(name).type) or pa.types.is_large_string(table.schema.field(name).type)
                        for name in ("series_id", "unit", "station"))
                pd.testing.assert_frame_equal(python_table.cast(canonical).to_pandas(), dotnet_table.cast(canonical).to_pandas(), check_exact=True)
                results.append({"case": name, "scenario": quality, "observations": report["summary"]["observationCount"], "hours": report["analysis"]["finishedHours"], "report": "equal", "csv": "byte-identical", "parquet": "same logical schema and values"})
    print(json.dumps({"cases": results}, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
