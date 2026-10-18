"""Extraction autonome : python -m hydrodatalab.extract --source fixture --output DIR."""
from __future__ import annotations

import argparse
import hashlib
import json
import os
import re
import time
from dataclasses import dataclass
from datetime import date, datetime, timedelta, timezone
from pathlib import Path
from urllib.error import HTTPError, URLError
from urllib.parse import urlencode
from urllib.request import Request, urlopen
from zoneinfo import ZoneInfo

BASE_URL = "https://donnees.hydroquebec.com/api/explore/v2.1/catalog/datasets"
TIMEZONE = "America/Toronto"
LICENSE = "CC BY-NC 4.0"


def utc_now() -> str:
    return datetime.now(timezone.utc).isoformat(timespec="seconds").replace("+00:00", "Z")


def fixture_dir() -> Path:
    return Path(os.environ.get("HYDRO_FIXTURE_DIR", Path(__file__).resolve().parents[2] / "data" / "fixtures"))


def validate_day(day: str | None) -> date | None:
    if day is None:
        return None
    try:
        if not re.fullmatch(r"\d{4}-\d{2}-\d{2}", day):
            raise ValueError
        return date.fromisoformat(day)
    except (TypeError, ValueError) as error:
        raise ValueError("La date doit être une date civile valide au format YYYY-MM-DD") from error


@dataclass
class RawBundle:
    demand: list[dict]
    hydrometeo: list[dict]
    period: dict
    provenance: list[dict]


def _request_json(url: str, *, opener=urlopen, sleeper=time.sleep) -> dict:
    """Trois tentatives au maximum; aucun secours fixture silencieux."""
    request = Request(url, headers={"User-Agent": "HydroDataLab-Training/1.0", "Accept": "application/json"})
    for attempt in range(3):
        try:
            with opener(request, timeout=20) as response:
                return json.load(response)
        except HTTPError as error:
            if error.code not in (429, 500, 502, 503, 504) or attempt == 2:
                raise RuntimeError(f"Source Hydro-Québec indisponible (HTTP {error.code}).") from error
        except (URLError, TimeoutError, OSError) as error:
            if attempt == 2:
                raise RuntimeError("Source Hydro-Québec inaccessible. Vérifier le réseau ou sélectionner le mode fixture.") from error
        sleeper(0.5 * (2 ** attempt))
    raise AssertionError("unreachable")


def extract(source: str = "fixture", *, directory: Path | None = None,
            day: str | None = None, now: datetime | None = None, fetch=_request_json) -> RawBundle:
    now = now or datetime.now(timezone.utc)
    if now.tzinfo is None:
        raise ValueError("now doit contenir un fuseau horaire")
    local_day = now.astimezone(ZoneInfo(TIMEZONE)).date()
    selected_day = validate_day(day)
    if selected_day is not None and selected_day >= local_day:
        raise ValueError("La date doit désigner une journée terminée dans America/Toronto")
    if source == "fixture":
        directory = directory or fixture_dir()
        manifest = json.loads((directory / "manifest.json").read_text(encoding="utf-8-sig"))
        if selected_day is not None and str(selected_day) != manifest["period"]["start"]:
            raise ValueError(f"Le snapshot couvre uniquement le {manifest['period']['start']}; la date demandée est refusée.")
        payloads = {}
        for name in ("demand.json", "hydrometeo.json"):
            raw = (directory / name).read_bytes()
            metadata = next(item for item in manifest["sources"] if item["file"] == name)
            if hashlib.sha256(raw).hexdigest() != metadata["sha256"]:
                raise ValueError(f"L'intégrité du snapshot {name} n'est plus valide.")
            payloads[name] = json.loads(raw)["results"]
        return RawBundle(payloads["demand.json"], payloads["hydrometeo.json"],
                         manifest["period"], manifest["sources"])
    if source != "live":
        raise ValueError("source doit être fixture ou live")
    start_day = selected_day or local_day - timedelta(days=1)
    end_day = start_day + timedelta(days=1)
    start = datetime.combine(start_day, datetime.min.time(), ZoneInfo(TIMEZONE))
    end = datetime.combine(end_day, datetime.min.time(), ZoneInfo(TIMEZONE))
    where = f'date >= "{start.isoformat()}" AND date < "{end.isoformat()}"'
    queries = [
        ("demande-electricite-quebec", where),
        ("donnees-hydrometeorologiques", where + ' AND ((identifiant="1-15412" AND composition_depil_type_point_donnee="Niveau") OR (identifiant="1-10622" AND composition_depil_type_point_donnee IN ("Température Minimum","Température Maximum")))'),
    ]
    all_rows, provenance = [], []
    for dataset, filter_ in queries:
        rows, offset = [], 0
        first_url = ""
        while True:
            url = f"{BASE_URL}/{dataset}/records?" + urlencode({"where": filter_, "order_by": "date", "limit": 100, "offset": offset})
            first_url = first_url or url
            payload = fetch(url)
            page = payload.get("results")
            if not isinstance(page, list):
                raise ValueError("Réponse Hydro-Québec invalide : results doit être une liste.")
            rows.extend(page)
            if not page and len(rows) < payload.get("total_count", len(rows)):
                raise ValueError("Pagination Hydro-Québec incomplète : page vide avant la fin annoncée.")
            if len(rows) >= payload.get("total_count", len(rows)):
                break
            offset += len(page)
            if offset > 1000:
                raise ValueError("Réponse trop volumineuse pour cette journée de démonstration.")
        if not rows:
            raise ValueError(f"Aucune donnée pour {dataset} le {start_day}. Aucun remplacement silencieux.")
        all_rows.append(rows)
        provenance.append({"label": "Hydro-Québec", "url": first_url, "license": LICENSE,
                           "retrievedAtUtc": utc_now(), "recordCount": len(rows)})
    return RawBundle(*all_rows, {"start": str(start_day), "end": str(end_day), "timezone": TIMEZONE}, provenance)


def main(argv=None) -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--source", choices=("fixture", "live"), default="fixture")
    parser.add_argument("--date", help="Journée civile terminée à Toronto, YYYY-MM-DD")
    parser.add_argument("--output", type=Path, required=True)
    args = parser.parse_args(argv)
    try:
        bundle = extract(args.source, day=args.date)
    except (ValueError, RuntimeError, OSError) as error:
        parser.exit(1, f"Échec : {error}\n")
    args.output.mkdir(parents=True, exist_ok=True)
    sources = []
    for name, rows, provenance in zip(("demand.json", "hydrometeo.json"),
                                     (bundle.demand, bundle.hydrometeo), bundle.provenance):
        path = args.output / name
        path.write_text(json.dumps({"total_count": len(rows), "results": rows}, ensure_ascii=False, indent=2), encoding="utf-8")
        sources.append({**provenance, "file": name, "recordCount": len(rows),
                        "sha256": hashlib.sha256(path.read_bytes()).hexdigest()})
    (args.output / "manifest.json").write_text(json.dumps({"schemaVersion": 1, "period": bundle.period,
        "sources": sources}, ensure_ascii=False, indent=2), encoding="utf-8")
    print(json.dumps({"output": str(args.output), "records": len(bundle.demand) + len(bundle.hydrometeo)}, ensure_ascii=False))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
