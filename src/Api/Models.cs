namespace HydroDataLab.Api;

public sealed record RunRequest(string Source = "fixture", bool SimulateFailure = false, string QualityScenario = "complete")
{
    public bool IsValid => Source is "fixture" or "live" && QualityScenario is "complete" or "missing";
}
public sealed record RunRecord(string Id, string Source, string Status, string Stage, string CreatedAtUtc, string? StartedAtUtc, string? CompletedAtUtc, string? Error, string QualityScenario);
public sealed record ScheduleSettings(bool Enabled = false, int IntervalMinutes = 5);

public sealed record RunDetails(RunRecord Run, string? RetryOf, bool? SimulateFailure, System.Text.Json.JsonElement? Report, System.Text.Json.JsonElement? Execution);
