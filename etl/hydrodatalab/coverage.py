"""Petite fonctionnalité indépendante, remplacée par un stub au checkpoint 06."""
from datetime import datetime, time, timezone
from zoneinfo import ZoneInfo


def calculate_coverage(timestamps, start: str, end: str, interval_minutes: int,
                       timezone_name: str = "America/Toronto") -> dict:
    """Compte des créneaux distincts attendus, y compris les jours de 23/25 heures.

    timestamps contient uniquement les observations dont la valeur est valide.
    start/end sont des dates civiles, borne de fin exclusive.
    """
    if interval_minutes <= 0 or 60 % interval_minutes:
        raise ValueError("interval_minutes doit être un diviseur positif de 60")
    zone = ZoneInfo(timezone_name)
    first = datetime.combine(datetime.fromisoformat(start).date(), time(), zone).astimezone(timezone.utc)
    last = datetime.combine(datetime.fromisoformat(end).date(), time(), zone).astimezone(timezone.utc)
    if last <= first:
        raise ValueError("La fin doit suivre le début")
    step = interval_minutes * 60
    expected = int((last - first).total_seconds() / step)
    slots = set()
    for timestamp in timestamps:
        instant = datetime.fromisoformat(str(timestamp).replace("Z", "+00:00"))
        if instant.tzinfo is None:
            raise ValueError("Un horodatage d'observation doit contenir son décalage UTC")
        elapsed = (instant.astimezone(timezone.utc) - first).total_seconds()
        if 0 <= elapsed < expected * step and elapsed % step == 0:
            slots.add(int(elapsed / step))
    observed = len(slots)
    return {"observed": observed, "expected": expected, "percent": round(100 * observed / expected, 2)}
