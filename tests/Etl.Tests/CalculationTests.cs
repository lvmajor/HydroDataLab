using System.Text.Json.Nodes;
using HydroDataLab.Etl;

namespace HydroDataLab.Etl.Tests;

public sealed class CalculationTests
{
    [Theory]
    [InlineData("2026-03-08", "2026-03-09", 23)]
    [InlineData("2026-10-17", "2026-10-18", 24)]
    [InlineData("2026-11-01", "2026-11-02", 25)]
    public void CoverageAndEnergyFollowActualCivilDay(string startDay, string endDay, int hours)
    {
        var period = new Period(startDay, endDay);
        var (start, end) = period.Bounds();
        var rows = Enumerable.Range(0, hours * 4).Select(i => new Observation("demand", start.AddMinutes(15 * i), 100, "MW", "Québec")).ToList();
        var coverage = Calculations.Coverage(rows.Select(r => r.Timestamp), period, 15);
        Assert.Equal(new Coverage(hours * 4, hours * 4, 100), coverage);
        var analysis = Calculations.Analyze(rows, period, end);
        Assert.Equal(hours, analysis.CompleteHours);
        Assert.Equal(hours, analysis.Hourly.Select(h => h.LocalHour).Distinct().Count());
        Assert.Equal(hours * 100, analysis.Indicators.EstimatedEnergyMWh);
        Assert.Equal(Time.Utc(start), analysis.Indicators.PeakHourUtc);
        if (hours == 25) Assert.Equal(2, analysis.Hourly.Count(h => h.LocalHour.Contains("T01:00:00")));
        rows.RemoveAt(0);
        analysis = Calculations.Analyze(rows, period, end);
        Assert.Null(analysis.Hourly[0].MeanDemandMW);
        Assert.Null(analysis.Indicators.EstimatedEnergyMWh);
    }

    [Fact]
    public void CoverageDeduplicatesAndExcludesOffGridAndExclusiveEnd()
    {
        var period = new Period("2026-11-01", "2026-11-02");
        var (start, end) = period.Bounds();
        DateTimeOffset[] timestamps = [start, start, start.AddHours(1), start.AddHours(2), start.AddMinutes(1), start.AddHours(-1), end];
        Assert.Equal(new Coverage(3, 25, 12), Calculations.Coverage(timestamps, period, 60));
    }

    [Theory]
    [InlineData(0)] [InlineData(-1)] [InlineData(7)]
    public void InvalidIntervalsFail(int interval) => Assert.Throws<ArgumentException>(() =>
        Calculations.Coverage([], new("2026-10-17", "2026-10-18"), interval));

    [Fact]
    public void InvalidPeriodFails() => Assert.Throws<ArgumentException>(() => new Period("2026-10-18", "2026-10-17").Bounds());

    [Fact]
    public void NormalizationPreservesZeroNullAndLastDuplicateAndCountsRejectedValues()
    {
        var period = new Period("2026-10-17", "2026-10-18");
        var (start, _) = period.Bounds();
        var demand = new List<JsonObject>();
        JsonNode?[] values = [0, null, "", "bad", true, "NaN", "Infinity", "1.25"];
        for (var i = 0; i < values.Length; i++) demand.Add(new() { ["date"] = Time.Utc(start.AddMinutes(15 * i)), ["valeurs_demandetotal"] = values[i] });
        demand.Add(new() { ["date"] = Time.Utc(start), ["valeurs_demandetotal"] = 42 });
        demand.Add(new() { ["date"] = "2026-10-17T04:00:00", ["valeurs_demandetotal"] = 1 });
        demand.Add(new() { ["date"] = "not-a-date", ["valeurs_demandetotal"] = 1 });
        demand.Add(new() { ["date"] = Time.Utc(start.AddHours(-1)), ["valeurs_demandetotal"] = 1 });
        demand.Add(new() { ["date"] = Time.Utc(start.AddHours(24)), ["valeurs_demandetotal"] = 1 });
        demand.Add(new() { ["date"] = Time.Utc(start.AddHours(12)), ["valeurs_demandetotal"] = 1 });
        var hydro = new List<JsonObject> { new() { ["identifiant"] = "1-15412", ["composition_depil_type_point_donnee"] = "Niveau", ["date"] = Time.Utc(start), ["valeur"] = 1, ["composition_depil_nom_unite_mesure"] = "cm" } };
        var normalized = Calculations.Normalize(new(demand, hydro, period, []), start.AddHours(6));
        Assert.Equal(10, normalized.InvalidCount);
        Assert.Equal(1, normalized.DuplicateCount);
        Assert.Equal(8, normalized.Rows.Count);
        Assert.Equal(42, normalized.Rows[0].Value);
        Assert.Equal(1.25, normalized.Rows[^1].Value);
        Assert.Equal(6, normalized.Rows.Count(r => r.Value is null));
        Assert.Equal(3, normalized.Warnings.Count);
        Assert.StartsWith("2 valeur(s)", normalized.Warnings[1]);
        Assert.Equal(0, Calculations.Normalize(new([new() { ["date"] = Time.Utc(start), ["valeurs_demandetotal"] = 0 }], [], period, []), start).Rows[0].Value);
    }

    [Fact]
    public void HourlyAnalysisUsesLastDuplicateAndOnlyFinishedHours()
    {
        var period = new Period("2026-10-17", "2026-10-18");
        var (start, _) = period.Bounds();
        var rows = Enumerable.Range(0, 4).Select(i => new Observation("demand", start.AddMinutes(i * 15), 0, "MW", "Québec")).ToList();
        rows.Add(new("demand", start, 100, "MW", "Québec"));
        rows.Add(new("demand", start.AddMinutes(1), 900, "MW", "Québec"));
        rows.Add(new("water_level", start, 10, "m", "Gouin"));
        rows.Add(new("water_level", start.AddHours(1), 10.03, "m", "Gouin"));
        rows.Add(new("water_level", start.AddHours(2), 999, "m", "Gouin"));
        Assert.Empty(Calculations.Analyze(rows, period, start.AddMinutes(59)).Hourly);
        var result = Calculations.Analyze(rows, period, start.AddHours(1));
        Assert.Equal(25, Assert.Single(result.Hourly).MeanDemandMW);
        Assert.Null(result.Indicators.EstimatedEnergyMWh);
        Assert.Equal(3, result.Indicators.WaterChangeCm);
        Assert.Equal(Time.Utc(start.AddHours(1)), result.Indicators.WaterEndUtc);
    }
}
