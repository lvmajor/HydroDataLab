"""Validation, types explicites et dédoublonnage des mesures HQ."""
from datetime import datetime, timezone
import math

import pandas as pd

from .extract import RawBundle

SERIES = {
    "demand": {"label": "Demande électrique", "station": "Québec (hors réseaux autonomes)", "unit": "MW", "kind": "demand", "intervalMinutes": 15},
    "water_level": {"label": "Niveau d’eau", "station": "Réservoir Gouin Nord", "unit": "m", "kind": "water_level", "intervalMinutes": 60},
    "temperature_min": {"label": "Température minimale horaire", "station": "Gouin Bois (CM2T)", "unit": "°C", "kind": "temperature_min", "intervalMinutes": 60},
    "temperature_max": {"label": "Température maximale horaire", "station": "Gouin Bois (CM2T)", "unit": "°C", "kind": "temperature_max", "intervalMinutes": 60},
}
HYDRO_SERIES = {
    ("1-15412", "Niveau"): "water_level",
    ("1-10622", "Température Minimum"): "temperature_min",
    ("1-10622", "Température Maximum"): "temperature_max",
}


def normalize(bundle: RawBundle, *, now: datetime | None = None):
    now = now or datetime.now(timezone.utc)
    start = pd.Timestamp(bundle.period["start"], tz=bundle.period["timezone"]).tz_convert("UTC")
    end = pd.Timestamp(bundle.period["end"], tz=bundle.period["timezone"]).tz_convert("UTC")
    records, invalid, missing = [], 0, 0
    candidates = [("demand", item, item.get("valeurs_demandetotal"), "MW") for item in bundle.demand]
    for item in bundle.hydrometeo:
        key = HYDRO_SERIES.get((item.get("identifiant"), item.get("composition_depil_type_point_donnee")))
        if key:
            candidates.append((key, item, item.get("valeur"), item.get("composition_depil_nom_unite_mesure")))
    for series_id, item, raw_value, unit in candidates:
        try:
            instant = datetime.fromisoformat(str(item.get("date")).replace("Z", "+00:00"))
            if instant.tzinfo is None:
                raise ValueError("date sans fuseau")
            timestamp = pd.Timestamp(instant).tz_convert("UTC")
            if not start <= timestamp < end or instant > now or unit != SERIES[series_id]["unit"]:
                raise ValueError("hors période, dans le futur ou unité invalide")
        except (ValueError, TypeError):
            invalid += 1
            continue
        if raw_value is None or raw_value == "":
            value = None
            missing += 1
        else:
            try:
                if isinstance(raw_value, bool):
                    raise ValueError("booléen au lieu d'une mesure")
                value = float(raw_value)
                if not math.isfinite(value):
                    raise ValueError("non fini")
            except (ValueError, TypeError):
                invalid += 1
                value = None
        records.append({"series_id": series_id, "timestamp_utc": timestamp,
                        "value": value, "unit": unit, "station": SERIES[series_id]["station"]})
    frame = pd.DataFrame(records, columns=["series_id", "timestamp_utc", "value", "unit", "station"])
    frame["timestamp_utc"] = pd.to_datetime(frame["timestamp_utc"], utc=True)
    frame["value"] = pd.array(frame["value"], dtype="Float64")
    duplicate_count = int(frame.duplicated(["series_id", "timestamp_utc"], keep="last").sum())
    frame = frame.drop_duplicates(["series_id", "timestamp_utc"], keep="last").sort_values(["series_id", "timestamp_utc"]).reset_index(drop=True)
    warnings = []
    if invalid:
        warnings.append(f"{invalid} observation(s) invalide(s), future(s) ou hors période : exclue(s) ou valeur laissée absente.")
    if missing:
        warnings.append(f"{missing} valeur(s) absente(s) : aucun remplacement par zéro.")
    if duplicate_count:
        warnings.append(f"{duplicate_count} doublon(s) supprimé(s) ; la dernière occurrence de chaque série/instant est conservée.")
    return frame, {"invalidCount": invalid, "duplicateCount": duplicate_count}, warnings
