from datetime import datetime, timezone
import json

import pandas as pd
import pytest

from hydrodatalab.analysis import analyze
from hydrodatalab.pipeline import run_pipeline

NOW = datetime(2030, 1, 1, tzinfo=timezone.utc)


def day_frame(day, end):
    period = {"start": day, "end": end, "timezone": "America/Toronto"}
    times = pd.date_range(pd.Timestamp(day, tz=period["timezone"]).tz_convert("UTC"),
                          pd.Timestamp(end, tz=period["timezone"]).tz_convert("UTC"), freq="15min", inclusive="left")
    return pd.DataFrame({"series_id": "demand", "timestamp_utc": times, "value": 100.0}), period


@pytest.mark.parametrize("day,end,hours", [("2026-10-17", "2026-10-18", 24),
                                           ("2026-03-08", "2026-03-09", 23),
                                           ("2026-11-01", "2026-11-02", 25)])
def test_energy_and_hourly_follow_real_duration(day, end, hours):
    frame, period = day_frame(day, end)
    result = analyze(frame, period, now=NOW)
    assert result["finishedHours"] == result["completeHours"] == hours
    assert result["indicators"]["estimatedEnergyMWh"] == hours * 100
    assert len({row["localHour"] for row in result["hourly"]}) == hours
    assert result["indicators"]["peakHourlyMW"] == 100
    assert result["indicators"]["waterChangeCm"] is None


def test_zero_missing_duplicate_and_off_grid_are_not_confused():
    frame, period = day_frame("2026-10-17", "2026-10-18")
    frame.loc[0:3, "value"] = [0, 100, 200, 300]
    frame.loc[4, "value"] = None
    duplicate = frame.iloc[[3]].copy()
    off_grid = frame.iloc[[0]].copy()
    off_grid["timestamp_utc"] += pd.Timedelta(minutes=1)
    off_grid["value"] = 999999
    result = analyze(pd.concat([frame, duplicate, off_grid]), period, now=NOW)
    assert result["hourly"][0]["meanDemandMW"] == 150
    assert result["hourly"][1]["observed"] == 3
    assert result["hourly"][1]["meanDemandMW"] is None
    assert result["completeHours"] == 23
    assert result["indicators"]["estimatedEnergyMWh"] is None
    assert result["indicators"]["peakHourlyMW"] == 150


def test_no_future_hours_or_daily_estimate_before_period_end():
    frame, period = day_frame("2026-10-17", "2026-10-18")
    result = analyze(frame, period, now=datetime(2026, 10, 17, 5, 30, tzinfo=timezone.utc))
    assert result["finishedHours"] == 1
    assert result["indicators"]["estimatedEnergyMWh"] is None


def test_water_change_uses_observed_endpoints_and_centimetres():
    frame, period = day_frame("2026-10-17", "2026-10-18")
    water = frame.iloc[[0, 4, 8]].copy()
    water["series_id"] = "water_level"
    water["value"] = [404.08, None, 404.06]
    result = analyze(pd.concat([frame, water]), period, now=NOW)["indicators"]
    assert result["waterChangeCm"] == -2
    assert result["waterStartUtc"] == "2026-10-17T04:00:00Z"
    assert result["waterEndUtc"] == "2026-10-17T06:00:00Z"


def test_published_analysis_audit_and_diagnostics(tmp_path):
    report = run_pipeline(output=tmp_path)
    assert report["etl"]["rawCount"] == report["etl"]["validCount"] == 168
    assert report["analysis"]["completeHours"] == 24
    assert len(report["etl"]["rawDemandSample"]) == 4
    hourly = pd.read_csv(tmp_path / report["runId"] / "hourly.csv")
    assert len(hourly) == 24
    assert hourly.meanDemandMW.sum() == pytest.approx(report["analysis"]["indicators"]["estimatedEnergyMWh"])
    steps = report["execution"]["steps"]
    assert [s["id"] for s in steps] == ["extract", "normalize", "analyze", "export"]
    assert all(s["durationMs"] >= 0 and s["status"] == "succeeded" for s in steps)
    assert json.loads((tmp_path / report["runId"] / "report.json").read_text(encoding="utf-8")) == report


def test_failure_carries_completed_and_failed_stages(tmp_path):
    with pytest.raises(RuntimeError) as error:
        run_pipeline(output=tmp_path, simulate_failure=True)
    steps = error.value.execution["steps"]
    assert len(steps) == 1 and steps[0]["status"] == "failed"
    assert steps[0]["durationMs"] >= 0
    assert list(tmp_path.iterdir()) == []
