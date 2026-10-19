"""Calculs BI purs : aucune dépendance au réseau, à HTTP ou à Quartz."""
from datetime import datetime, timezone

import pandas as pd


def instant(value):
    return value.isoformat().replace("+00:00", "Z")


def analyze(frame, period, *, now=None):
    """Moyennes uniquement sur quatre mesures valides; aucune interpolation."""
    start = pd.Timestamp(period["start"], tz=period["timezone"]).tz_convert("UTC")
    end = pd.Timestamp(period["end"], tz=period["timezone"]).tz_convert("UTC")
    current = pd.Timestamp(now or datetime.now(timezone.utc)).tz_convert("UTC")
    # UTC évite de confondre les deux occurrences de 01 h au retour à l'heure normale.
    hours = pd.date_range(start, end, freq="h", inclusive="left")
    hours = hours[hours + pd.Timedelta(hours=1) <= current]
    demand = frame.loc[frame.series_id == "demand"].drop_duplicates("timestamp_utc", keep="last")
    demand = demand.set_index("timestamp_utc")["value"]
    hourly = []
    for hour in hours:
        values = demand.reindex(pd.date_range(hour, periods=4, freq="15min"))
        count = int(values.notna().sum())
        hourly.append({"startUtc": instant(hour), "localHour": hour.tz_convert(period["timezone"]).isoformat(),
                       "observed": count, "expected": 4,
                       "meanDemandMW": float(values.mean()) if count == 4 else None})
    complete = [h for h in hourly if h["meanDemandMW"] is not None]
    peak = max(complete, key=lambda h: h["meanDemandMW"], default=None)
    full_period = end <= current and len(complete) == len(hours) and len(hours) > 0
    water = frame.loc[(frame.series_id == "water_level") & frame.value.notna()
                      & (frame.timestamp_utc >= start) & (frame.timestamp_utc < end)
                      & (frame.timestamp_utc <= current)].sort_values("timestamp_utc")
    water = water.drop_duplicates("timestamp_utc", keep="last")
    first, last = (water.iloc[0], water.iloc[-1]) if len(water) >= 2 else (None, None)
    return {"hourly": hourly, "completeHours": len(complete), "finishedHours": len(hours),
            "indicators": {
                "peakHourlyMW": peak["meanDemandMW"] if peak else None,
                "peakHourUtc": peak["startUtc"] if peak else None,
                "estimatedEnergyMWh": sum(h["meanDemandMW"] for h in complete) if full_period else None,
                "waterChangeCm": round(float(last.value - first.value) * 100, 6) if first is not None else None,
                "waterStartUtc": instant(first.timestamp_utc) if first is not None else None,
                "waterEndUtc": instant(last.timestamp_utc) if last is not None else None}}
