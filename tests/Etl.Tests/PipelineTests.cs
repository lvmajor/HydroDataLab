using System.Text.Json.Nodes;
using Parquet;
using HydroDataLab.Etl;

namespace HydroDataLab.Etl.Tests;

public sealed class PipelineTests : IDisposable
{
    private readonly string output = Path.Combine(Path.GetTempPath(), "hydro-etl-tests-" + Guid.NewGuid().ToString("N"));
    internal static readonly DateTimeOffset Now = DateTimeOffset.Parse("2026-11-05T12:00:00Z");
    internal static string Fixtures => Path.Combine(AppContext.BaseDirectory, "fixtures");
    private Pipeline Create() => new(new SourceReader(new HttpClient(), new(Fixtures, output)), new(Fixtures, output));

    [Theory]
    [InlineData("complete", 168, 100)]
    [InlineData("missing", 167, 95.83)]
    public async Task FixturePublishesExpectedResults(string scenario, int count, double coverage)
    {
        var report = await Create().RunAsync(new(QualityScenario: scenario, Now: Now));
        Assert.Equal(count, report["summary"]!["observationCount"]!.GetValue<int>());
        Assert.Equal(coverage, report["series"]!.AsArray().Single(s => Json.Text(s!["id"]) == "water_level")!["coverage"]!["percent"]!.GetValue<double>());
        Assert.Equal(489442, report["analysis"]!["indicators"]!["estimatedEnergyMWh"]!.GetValue<double>());
        Assert.Equal(22009.75, report["analysis"]!["indicators"]!["peakHourlyMW"]!.GetValue<double>());
        Assert.Equal(24, report["analysis"]!["completeHours"]!.GetValue<int>());
        Assert.All(report["execution"]!["steps"]!.AsArray(), s => Assert.Equal("succeeded", Json.Text(s!["status"])));
        var directory = Path.Combine(output, Json.Text(report["runId"]));
        Assert.Equal(4, Directory.GetFiles(directory).Length);
        Assert.True(JsonNode.DeepEquals(report, JsonNode.Parse(await File.ReadAllTextAsync(Path.Combine(directory, "report.json")))));
        Assert.DoesNotContain(Directory.GetDirectories(output), p => Path.GetFileName(p).StartsWith('.'));
        await using var parquet = await ParquetReader.CreateAsync(Path.Combine(directory, "observations.parquet"));
        Assert.Equal(new[] { "series_id", "timestamp_utc", "value", "unit", "station" }, parquet.Schema.GetDataFields().Select(f => f.Name));
        using var group = parquet.OpenRowGroupReader(0);
        Assert.Equal(count, group.RowCount);
        var values = new double?[count];
        await group.ReadAsync<double>(parquet.Schema.GetDataFields()[2], values.AsMemory());
        var expectedValues = report["series"]!.AsArray().OrderBy(s => Json.Text(s!["id"]), StringComparer.Ordinal)
            .SelectMany(s => s!["points"]!.AsArray()).Select(p => p!["value"]?.GetValue<double>());
        Assert.Equal(expectedValues, values);
    }

    [Fact]
    public async Task ReplayDoesNotReextractOrRewriteAndRejectsConflictingParameters()
    {
        var pipeline = Create();
        var id = Guid.NewGuid().ToString();
        var options = new RunOptions(RunId: id, Now: Now);
        var original = await pipeline.RunAsync(options);
        var file = pipeline.ArtifactPath(id, "report.json");
        var modified = File.GetLastWriteTimeUtc(file);
        var replay = await new Pipeline(new BrokenSource(), new(Fixtures, output)).RunAsync(options with { SimulateFailure = true });
        Assert.True(JsonNode.DeepEquals(original, replay));
        Assert.Equal(modified, File.GetLastWriteTimeUtc(file));
        await Assert.ThrowsAsync<ArgumentException>(() => pipeline.RunAsync(options with { QualityScenario = "missing" }));
        await Assert.ThrowsAsync<ArgumentException>(() => pipeline.RunAsync(options with { Source = "live" }));
        await Assert.ThrowsAsync<ArgumentException>(() => pipeline.RunAsync(options with { Day = "2026-10-16" }));
    }

    [Fact]
    public async Task FailureKeepsDiagnosticsAndPublishesNothing()
    {
        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => Create().RunAsync(new(SimulateFailure: true, Now: Now)));
        var steps = ((JsonObject)error.Data["execution"]!)["steps"]!.AsArray();
        Assert.Equal("failed", Json.Text(Assert.Single(steps)!["status"]));
        Assert.False(Directory.Exists(output));
    }

    [Fact]
    public async Task ConcurrentProcessesPublishOneImmutableReport()
    {
        var source = new SourceReader(new HttpClient(), new(Fixtures, output));
        var bundle = await source.ExtractAsync("fixture", null, Now);
        var rendezvous = new BarrierSource(bundle);
        var id = Guid.NewGuid().ToString();
        var pipeline = new Pipeline(rendezvous, new(Fixtures, output));
        var reports = await Task.WhenAll(pipeline.RunAsync(new(RunId: id, Now: Now)), pipeline.RunAsync(new(RunId: id, Now: Now)));
        Assert.True(JsonNode.DeepEquals(reports[0], reports[1]));
        Assert.Single(Directory.GetDirectories(output));
    }

    [Fact]
    public async Task MissingScenarioRemovesValidWaterAndParquetRetainsNull()
    {
        var bundle = await new SourceReader(new HttpClient(), new(Fixtures, output)).ExtractAsync("fixture", null, Now);
        var first = bundle.Hydrometeo.First(r => Json.Text(r["identifiant"]) == "1-15412");
        first["valeur"] = null;
        var original = Json.Node(bundle).ToJsonString();
        var pipeline = new Pipeline(new FixedSource(bundle), new(Fixtures, output));
        var report = await pipeline.RunAsync(new(QualityScenario: "missing", Now: Now));
        Assert.Equal(original, Json.Node(bundle).ToJsonString());
        Assert.Equal(166, report["summary"]!["observationCount"]!.GetValue<int>());
        Assert.Equal(1, report["etl"]!["missingCount"]!.GetValue<int>());
        await using var parquet = await ParquetReader.CreateAsync(pipeline.ArtifactPath(Json.Text(report["runId"]), "observations.parquet"));
        using var group = parquet.OpenRowGroupReader(0);
        var values = new double?[group.RowCount];
        await group.ReadAsync<double>(parquet.Schema.GetDataFields()[2], values.AsMemory());
        Assert.Single(values, v => v is null);
    }

    private sealed class BrokenSource : ISourceReader
    {
        public Task<RawBundle> ExtractAsync(string source, string? day, DateTimeOffset now, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Extraction inattendue lors d'une relance");
    }
    internal sealed class FixedSource(RawBundle bundle) : ISourceReader
    {
        public Task<RawBundle> ExtractAsync(string source, string? day, DateTimeOffset now, CancellationToken cancellationToken) => Task.FromResult(bundle);
    }
    private sealed class BarrierSource(RawBundle bundle) : ISourceReader
    {
        private readonly TaskCompletionSource ready = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int calls;
        public async Task<RawBundle> ExtractAsync(string source, string? day, DateTimeOffset now, CancellationToken cancellationToken)
        {
            if (Interlocked.Increment(ref calls) == 2) ready.SetResult();
            await ready.Task.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
            return bundle;
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(output) && Path.GetFileName(output).StartsWith("hydro-etl-tests-", StringComparison.Ordinal)) Directory.Delete(output, true);
    }
}
