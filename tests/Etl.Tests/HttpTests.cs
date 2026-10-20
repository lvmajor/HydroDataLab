using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using HydroDataLab.Etl;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HydroDataLab.Etl.Tests;

public sealed class HttpTests : IDisposable
{
    private readonly string output = Path.Combine(Path.GetTempPath(), "hydro-http-tests-" + Guid.NewGuid().ToString("N"));

    private Microsoft.AspNetCore.Builder.WebApplication Create(ISourceReader? source = null) => EtlProgram.Build([], builder =>
    {
        builder.WebHost.UseTestServer();
        builder.Logging.ClearProviders();
        builder.Services.AddSingleton(new EtlPaths(PipelineTests.Fixtures, output));
        if (source is not null) builder.Services.AddSingleton(source);
    });

    [Fact]
    public async Task HttpContractSupportsFailuresReplayAndFourArtifacts()
    {
        await using var app = Create();
        await app.StartAsync();
        using var client = app.GetTestClient();
        Assert.Equal("ok", (await client.GetFromJsonAsync<JsonObject>("/health"))!["status"]!.GetValue<string>());
        var id = Guid.NewGuid().ToString();
        using var failure = await client.PostAsJsonAsync("/run", new { runId = id, simulateFailure = true });
        Assert.Equal(HttpStatusCode.ServiceUnavailable, failure.StatusCode);
        var error = (await failure.Content.ReadFromJsonAsync<JsonObject>())!["detail"]!;
        Assert.Contains("Échec simulé", Json.Text(error["message"]));
        Assert.Equal("failed", Json.Text(error["execution"]!["steps"]![0]!["status"]));
        using var response = await client.PostAsJsonAsync("/run", new { runId = id });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var report = await response.Content.ReadFromJsonAsync<JsonObject>();
        Assert.Equal(168, report!["summary"]!["observationCount"]!.GetValue<int>());
        foreach (var name in Pipeline.ArtifactNames)
        {
            using var artifact = await client.GetAsync($"/artifacts/{id}/{name}");
            Assert.Equal(HttpStatusCode.OK, artifact.StatusCode);
            Assert.NotNull(artifact.Content.Headers.ContentDisposition);
            Assert.True((await artifact.Content.ReadAsByteArrayAsync()).Length > 100);
        }
        using var replay = await client.PostAsJsonAsync("/run", new { runId = id, simulateFailure = true });
        Assert.True(JsonNode.DeepEquals(report, await replay.Content.ReadFromJsonAsync<JsonObject>()));
        using var conflict = await client.PostAsJsonAsync("/run", new { runId = id, qualityScenario = "missing" });
        Assert.Equal(HttpStatusCode.UnprocessableEntity, conflict.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.GetAsync($"/artifacts/{id}/secret.txt")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/artifacts/{Guid.NewGuid()}/report.json")).StatusCode);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await client.GetAsync("/artifacts/invalid/report.json")).StatusCode);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"runId\":\"../outside\"}")]
    [InlineData("{\"runId\":\"0a617413-4c19-4e24-9250-c875426e3d13\",\"source\":null}")]
    [InlineData("{\"runId\":\"0a617413-4c19-4e24-9250-c875426e3d13\",\"source\":\"unknown\"}")]
    [InlineData("{\"runId\":\"0a617413-4c19-4e24-9250-c875426e3d13\",\"simulateFailure\":\"false\"}")]
    [InlineData("[]")]
    [InlineData("invalid json")]
    public async Task InvalidRequestsAreRejectedWithoutCreatingExports(string json)
    {
        await using var app = Create();
        await app.StartAsync();
        using var client = app.GetTestClient();
        using var response = await client.PostAsync("/run", new StringContent(json, Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        Assert.NotNull((await response.Content.ReadFromJsonAsync<JsonObject>())!["detail"]!["message"]);
        Assert.False(Directory.Exists(output));
    }

    [Fact]
    public async Task BusyRunIsRejectedWhileHealthRemainsResponsiveAndGateReleases()
    {
        var bundle = await new SourceReader(new HttpClient(), new(PipelineTests.Fixtures, output)).ExtractAsync("fixture", null, PipelineTests.Now);
        var source = new BlockingSource(bundle);
        await using var app = Create(source);
        await app.StartAsync();
        using var client = app.GetTestClient();
        var first = client.PostAsJsonAsync("/run", new { runId = Guid.NewGuid() });
        try
        {
            await source.Entered.Task.WaitAsync(TimeSpan.FromSeconds(5));
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/health")).StatusCode);
            Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync("/run", new { runId = Guid.NewGuid() })).StatusCode);
        }
        finally { source.Release.TrySetResult(); }
        Assert.Equal(HttpStatusCode.OK, (await first).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.PostAsJsonAsync("/run", new { runId = Guid.NewGuid() })).StatusCode);
    }

    private sealed class BlockingSource(RawBundle bundle) : ISourceReader
    {
        public TaskCompletionSource Entered { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource Release { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public async Task<RawBundle> ExtractAsync(string source, string? day, DateTimeOffset now, CancellationToken cancellationToken)
        {
            Entered.TrySetResult();
            await Release.Task.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
            return bundle;
        }
    }
    public void Dispose()
    {
        if (Directory.Exists(output) && Path.GetFileName(output).StartsWith("hydro-http-tests-", StringComparison.Ordinal)) Directory.Delete(output, true);
    }
}
