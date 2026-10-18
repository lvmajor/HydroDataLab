import pytest
from hydrodatalab.coverage import calculate_coverage


@pytest.mark.parametrize("start,end,hours", [("2026-03-08", "2026-03-09", 23), ("2026-11-01", "2026-11-02", 25), ("2026-10-17", "2026-10-18", 24)])
def test_expected_slots_follow_toronto_civil_days(start, end, hours):
    assert calculate_coverage([], start, end, 60) == {"observed": 0, "expected": hours, "percent": 0}
    assert calculate_coverage([], start, end, 15)["expected"] == hours * 4


def test_repeated_autumn_hour_is_two_instants_and_duplicates_are_one():
    timestamps = ["2026-11-01T01:00:00-04:00", "2026-11-01T01:00:00-05:00", "2026-11-01T06:00:00Z"]
    assert calculate_coverage(timestamps, "2026-11-01", "2026-11-02", 60) == {"observed": 2, "expected": 25, "percent": 8}


def test_outside_offgrid_and_end_exclusive_points_do_not_inflate_coverage():
    timestamps = ["2026-10-17T03:00:00Z", "2026-10-17T04:00:00Z", "2026-10-17T04:01:00Z", "2026-10-18T04:00:00Z"]
    assert calculate_coverage(timestamps, "2026-10-17", "2026-10-18", 60)["observed"] == 1


@pytest.mark.parametrize("interval", [0, -1, 7])
def test_bad_interval_rejected(interval):
    with pytest.raises(ValueError):
        calculate_coverage([], "2026-10-17", "2026-10-18", interval)


def test_naive_timestamp_and_inverted_period_rejected():
    with pytest.raises(ValueError):
        calculate_coverage(["2026-10-17T04:00:00"], "2026-10-17", "2026-10-18", 60)
    with pytest.raises(ValueError):
        calculate_coverage([], "2026-10-18", "2026-10-17", 60)
