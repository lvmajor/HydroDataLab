"""Exécuter explicitement depuis la racine; hors collecte normale de etl/tests."""
from datetime import datetime, timezone

from hydrodatalab.coverage import calculate_coverage
from hydrodatalab.pipeline import run_pipeline


def test_analyst_sees_missing_water_observation(tmp_path):
    report = run_pipeline(output=tmp_path, quality_scenario="missing", now=datetime(2026, 10, 18, 20, tzinfo=timezone.utc))
    water = next(series for series in report["series"] if series["id"] == "water_level")
    assert water["coverage"] == {"observed": 23, "expected": 24, "percent": 95.83}


def test_expected_slots_follow_daylight_saving_time():
    assert calculate_coverage([], "2026-03-08", "2026-03-09", 15) == {"observed": 0, "expected": 92, "percent": 0}
    repeated_hour = ["2026-11-01T01:00:00-04:00", "2026-11-01T01:00:00-05:00"]
    assert calculate_coverage(repeated_hour, "2026-11-01", "2026-11-02", 60) == {"observed": 2, "expected": 25, "percent": 8}
