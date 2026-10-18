using HydroDataLab.Api;
using Microsoft.Data.Sqlite;

namespace HydroDataLab.Api.Tests;

public sealed class RunStoreTests : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "hydrolab-tests-" + Guid.NewGuid());
    private RunStore Create()
    {
        var store = new RunStore(Path.Combine(directory, "test.sqlite"));
        store.Initialize();
        return store;
    }
    [Fact]
    public void ConcurrentLaunchesReserveOneSlotOnly()
    {
        var store = Create();
        var results = new System.Collections.Concurrent.ConcurrentBag<string?>();
        Parallel.For(0, 8, _ => results.Add(store.TryCreate(new RunRequest())));
        Assert.Single(results, id => id is not null);
        Assert.Single(store.List());
    }
    [Fact]
    public void RestartPreservesHistoryAndMarksOnlyActiveRunsInterrupted()
    {
        var store = Create();
        var succeeded = store.TryCreate(new RunRequest())!;
        Assert.True(store.TryStart(succeeded));
        store.Complete(succeeded, "{\"observationCount\":168}", null);
        var interrupted = store.TryCreate(new RunRequest())!;
        Assert.True(store.TryStart(interrupted));
        var restarted = Create();
        restarted.InterruptInFlight();
        Assert.Equal("succeeded", restarted.Get(succeeded)!.Status);
        Assert.Equal("interrupted", restarted.Get(interrupted)!.Status);
        Assert.False(restarted.TryStart(interrupted));
        Assert.NotNull(restarted.LatestReport());
        Assert.NotNull(restarted.TryCreate(new RunRequest()));
    }
    [Fact]
    public void FailedRunKeepsLastValidReport()
    {
        var store = Create();
        var first = store.TryCreate(new RunRequest())!;
        store.Complete(first, "{\"source\":\"fixture\"}", null);
        var second = store.TryCreate(new RunRequest("live"))!;
        store.Complete(second, null, "Réseau indisponible");
        Assert.Equal("{\"source\":\"fixture\"}", store.LatestReport());
        Assert.Equal("failed", store.Get(second)!.Status);
    }
    [Fact]
    public void SchedulePersistsAndStartsDisabled()
    {
        var store = Create();
        Assert.False(store.ReadSchedule().Enabled);
        store.WriteSchedule(new(true, 7));
        Assert.Equal(new(true, 7), Create().ReadSchedule());
    }
    [Theory]
    [InlineData("fixture", "complete", true)]
    [InlineData("live", "missing", true)]
    [InlineData("https://evil.invalid", "complete", false)]
    [InlineData("fixture", "unknown", false)]
    public void RunRequestValidatesSourcesAndScenarios(string source, string scenario, bool expected)
        => Assert.Equal(expected, new RunRequest(source, false, scenario).IsValid);
    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        if (Directory.Exists(directory)) Directory.Delete(directory, recursive: true);
    }
}
