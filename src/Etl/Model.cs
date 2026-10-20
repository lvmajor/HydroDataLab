using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace HydroDataLab.Etl;

public static class Json
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    public static JsonNode Node<T>(T value) => JsonSerializer.SerializeToNode(value, Options)!;
    public static string Text(JsonNode? node) => node is JsonValue value && value.TryGetValue<string>(out var text) ? text : "";
}

public sealed record Period(string Start, string End, string Timezone = "America/Toronto")
{
    public (DateTimeOffset Start, DateTimeOffset End) Bounds()
    {
        var zone = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
        DateTimeOffset Midnight(string day)
        {
            var date = Time.ParseDay(day)!.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);
            return new DateTimeOffset(date, zone.GetUtcOffset(date)).ToUniversalTime();
        }
        var start = Midnight(Start);
        var end = Midnight(End);
        if (end <= start) throw new ArgumentException("La fin de période doit être postérieure au début");
        return (start, end);
    }
}

public static partial class Time
{
    public static string Utc(DateTimeOffset value) => value.UtcDateTime.ToString(
        value.Ticks % TimeSpan.TicksPerSecond == 0 ? "yyyy-MM-dd'T'HH:mm:ss'Z'" : "yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'", CultureInfo.InvariantCulture);
    public static string Now() => DateTimeOffset.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
    public static string Local(DateTimeOffset value, string zone) => TimeZoneInfo.ConvertTime(value,
        TimeZoneInfo.FindSystemTimeZoneById(zone)).ToString("yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);
    public static DateOnly? ParseDay(string? value)
    {
        if (value is null) return null;
        if (!DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            throw new ArgumentException("La date doit être une date civile valide au format YYYY-MM-DD");
        return date;
    }
    [GeneratedRegex(@"[Tt ].*(?:[Zz]|[+-]\d{2}:?\d{2})$")]
    private static partial Regex ExplicitOffset();
    public static bool TryInstant(string text, out DateTimeOffset instant)
    {
        instant = default;
        if (!ExplicitOffset().IsMatch(text) || !DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed)) return false;
        // Python datetime has microsecond precision.
        instant = new DateTimeOffset(parsed.UtcTicks - parsed.UtcTicks % 10, TimeSpan.Zero);
        return true;
    }
}

public sealed record RawBundle(List<JsonObject> Demand, List<JsonObject> Hydrometeo, Period Period, JsonArray Provenance);
public sealed record Observation(string SeriesId, DateTimeOffset Timestamp, double? Value, string Unit, string Station);
public sealed record SeriesDefinition(string Id, string Label, string Station, string Unit, string Kind, int IntervalMinutes);
public sealed record Normalized(List<Observation> Rows, int InvalidCount, int DuplicateCount, List<string> Warnings);
public sealed record Coverage(int Observed, int Expected, double Percent);
public sealed record Hour(string StartUtc, string LocalHour, int Observed, int Expected, double? MeanDemandMW);
public sealed record Indicators(double? PeakHourlyMW, string? PeakHourUtc, double? EstimatedEnergyMWh,
    double? WaterChangeCm, string? WaterStartUtc, string? WaterEndUtc);
public sealed record Analysis(List<Hour> Hourly, int CompleteHours, int FinishedHours, Indicators Indicators);
public sealed record RunOptions(string Source = "fixture", string? RunId = null, bool SimulateFailure = false,
    string QualityScenario = "complete", string? Day = null, DateTimeOffset? Now = null);

public sealed record EtlPaths(string Fixtures, string Output)
{
    public static EtlPaths FromConfiguration(IConfiguration configuration)
    {
        // Locate the repository for direct CLI use; containers provide explicit paths.
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (root.Parent is not null && !Directory.Exists(Path.Combine(root.FullName, "data", "fixtures"))) root = root.Parent;
        return new(configuration["HYDRO_FIXTURE_DIR"] ?? Path.Combine(root.FullName, "data", "fixtures"),
            configuration["HYDRO_OUTPUT_DIR"] ?? Path.Combine(root.FullName, "outputs", "dotnet"));
    }
}
