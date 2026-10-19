"""Le même pipeline est appelé par CLI, HTTP et Docker."""
from __future__ import annotations

import copy
import errno
import json
import os
import shutil
import tempfile
from datetime import datetime
from pathlib import Path
from uuid import UUID, uuid4

import pandas as pd

from . import coverage
from .analysis import analyze
from .diagnostics import Execution
from .extract import extract, utc_now, validate_day
from .normalize import SERIES, normalize

ARTIFACTS = {"observations.csv", "observations.parquet", "report.json", "hourly.csv"}


def validate_run_id(value: str) -> str:
    return str(UUID(value))


def output_root() -> Path:
    return Path(os.environ.get("HYDRO_OUTPUT_DIR", Path(__file__).resolve().parents[2] / "outputs" / "python"))


def artifact_path(run_id: str, name: str, root: Path | None = None) -> Path:
    if name not in ARTIFACTS:
        raise ValueError("Fichier non autorisé")
    return (root or output_root()) / validate_run_id(run_id) / name


def run_pipeline(source="fixture", *, output: Path | None = None, run_id: str | None = None,
                 simulate_failure=False, quality_scenario="complete", day: str | None = None, now: datetime | None = None,
                 extractor=extract) -> dict:
    if source not in ("fixture", "live") or quality_scenario not in ("complete", "missing"):
        raise ValueError("Source ou scénario inconnu")
    run_id = validate_run_id(run_id) if run_id else str(uuid4())
    selected_day = validate_day(day)
    root = Path(output) if output is not None else output_root()
    destination = root / run_id
    existing = destination / "report.json"
    if existing.exists():
        report = json.loads(existing.read_text(encoding="utf-8"))
        if (report["source"] != source or report["qualityScenario"] != quality_scenario
                or (selected_day is not None and report["period"]["start"] != str(selected_day))):
            raise ValueError("Ce runId existe avec des paramètres différents")
        return report
    execution = Execution()
    with execution.step("extract", "Extraire les sources"):
        if simulate_failure:
            raise RuntimeError("Échec simulé du pipeline pour la démonstration. Relancez avec simulateFailure=false.")
        bundle = extractor(source, day=day, now=now)
        bundle = copy.deepcopy(bundle)
    with execution.step("normalize", "Valider et normaliser"):
        frame, stats, warnings = normalize(bundle, now=now)
        scenario_warnings = []
        if quality_scenario == "missing":
            valid_water = frame.index[(frame["series_id"] == "water_level") & frame["value"].notna()]
            if valid_water.empty:
                raise ValueError("Aucune observation valide de niveau d'eau à retirer pour ce scénario")
            frame = frame.drop(index=valid_water[0]).reset_index(drop=True)
            scenario_warnings.append("Scénario pédagogique : une observation de niveau d'eau a été retirée uniquement de la copie en mémoire.")
    with execution.step("analyze", "Calculer la synthèse et les indicateurs"):
        analysis = analyze(frame, bundle.period, now=now)
        series = []
        for series_id, metadata in SERIES.items():
            subset = frame.loc[frame["series_id"] == series_id]
            valid = subset.loc[subset["value"].notna()]
            if valid.empty:
                raise ValueError(f"Aucune mesure valide pour la série {series_id}")
            points = [{"timestamp": row.timestamp_utc.isoformat().replace("+00:00", "Z"),
                       "value": None if pd.isna(row.value) else float(row.value)} for row in subset.itertuples()]
            series.append({"id": series_id, **metadata, "points": points,
                           "minimum": float(valid["value"].min()), "maximum": float(valid["value"].max()),
                           "latest": float(valid.iloc[-1]["value"]),
                           "coverage": coverage.calculate_coverage(valid["timestamp_utc"], bundle.period["start"],
                               bundle.period["end"], metadata["intervalMinutes"], bundle.period["timezone"])})
    report = {"schemaVersion": 1, "runId": run_id, "source": source, "generatedAtUtc": utc_now(),
              "period": bundle.period, "qualityScenario": quality_scenario,
              "summary": {"observationCount": int(frame["value"].notna().sum()), "seriesCount": len(series), **stats},
              "analysis": analysis,
              "etl": {"rawCount": len(bundle.demand) + len(bundle.hydrometeo),
                      "normalizedCount": len(frame), "validCount": int(frame.value.notna().sum()),
                      "missingCount": int(frame.value.isna().sum()),
                      "scenarioRemovedCount": 1 if quality_scenario == "missing" else 0,
                      "rawDemandSample": [{"date": r.get("date"), "valeurs_demandetotal": r.get("valeurs_demandetotal")} for r in bundle.demand[:4]],
                      "normalizedDemandSample": [{"timestamp_utc": r.timestamp_utc.isoformat().replace("+00:00", "Z"),
                                                  "value": None if pd.isna(r.value) else float(r.value), "unit": r.unit}
                                                 for r in frame.loc[frame.series_id == "demand"].head(4).itertuples()]},
              "series": series, "provenance": bundle.provenance, "warnings": scenario_warnings + warnings,
              "artifacts": {"csv": "observations.csv", "parquet": "observations.parquet", "report": "report.json", "hourly": "hourly.csv"}}
    root.mkdir(parents=True, exist_ok=True)
    staging = Path(tempfile.mkdtemp(prefix=f".{run_id}-", dir=root))
    try:
        with execution.step("export", "Écrire les exports CSV et Parquet"):
            csv_frame = frame.copy()
            csv_frame["timestamp_utc"] = csv_frame["timestamp_utc"].dt.strftime("%Y-%m-%dT%H:%M:%SZ")
            csv_frame.to_csv(staging / "observations.csv", index=False, encoding="utf-8", lineterminator="\n")
            frame.to_parquet(staging / "observations.parquet", index=False, engine="pyarrow")
            pd.DataFrame(analysis["hourly"], columns=["startUtc", "localHour", "observed", "expected", "meanDemandMW"]).to_csv(staging / "hourly.csv", index=False, encoding="utf-8", lineterminator="\n")
        report["execution"] = execution.as_dict()
        (staging / "report.json").write_text(json.dumps(report, ensure_ascii=False, indent=2, allow_nan=False), encoding="utf-8")
        try:
            staging.rename(destination)
        except OSError as error:
            if error.errno not in (errno.EEXIST, errno.ENOTEMPTY):
                raise
            if existing.exists():
                other = json.loads(existing.read_text(encoding="utf-8"))
                if (other["source"] != source or other["qualityScenario"] != quality_scenario
                        or (selected_day is not None and other["period"]["start"] != str(selected_day))):
                    raise ValueError("Ce runId existe avec des paramètres différents")
                return other
            raise
    finally:
        if staging.exists():
            if staging.resolve().parent != root.resolve() or not staging.name.startswith(f".{run_id}-"):
                raise RuntimeError("Chemin temporaire inattendu : nettoyage refusé")
            shutil.rmtree(staging)
    return report
