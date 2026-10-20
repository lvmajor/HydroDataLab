using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using HydroDataLab.Etl;

namespace HydroDataLab.Etl.Tests;

public sealed class SourceTests : IDisposable
{
    private readonly string temporary = Path.Combine(Path.GetTempPath(), "hydro-source-tests-" + Guid.NewGuid().ToString("N"));
    private static HttpResponseMessage Response(string json, HttpStatusCode status = HttpStatusCode.OK) => new(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    [Fact]
    public async Task LiveExtractionPaginatesWithCivilTimeAndKeepsProvenance()
    {
        List<string> urls = [];
        using var client = new HttpClient(new Handler(request =>
        {
            urls.Add(WebUtility.UrlDecode(request.RequestUri!.ToString()));
            var count = urls.Count == 1 ? 100 : urls.Count == 2 ? 25 : 75;
            return Response(Json.Node(new { total_count = urls.Count < 3 ? 125 : 75, results = Enumerable.Range(0, count).Select(i => new { date = "2026-03-08T00:00:00-05:00", value = i }) }).ToJsonString());
        }));
        var bundle = await new SourceReader(client, new("unused", temporary)).ExtractAsync("live", "2026-03-08", PipelineTests.Now);
        Assert.Equal(125, bundle.Demand.Count);
        Assert.Equal(75, bundle.Hydrometeo.Count);
        Assert.Equal(3, urls.Count);
        Assert.Contains("offset=100", urls[1]);
        Assert.Contains("2026-03-08T00:00:00-05:00", urls[0]);
        Assert.Contains("2026-03-09T00:00:00-04:00", urls[0]);
        Assert.Contains("Température Minimum", urls[2]);
        Assert.Equal(2, bundle.Provenance.Count);
    }

    [Theory]
    [InlineData(404, 1)] [InlineData(429, 3)] [InlineData(503, 3)]
    public async Task HttpRetriesOnlyTransientErrors(int status, int expectedAttempts)
    {
        var attempts = 0;
        List<double> delays = [];
        using var client = new HttpClient(new Handler(_ => { attempts++; return Response("{}", (HttpStatusCode)status); }));
        var source = new SourceReader(client, new("unused", temporary)) { Delay = (delay, _) => { delays.Add(delay.TotalSeconds); return Task.CompletedTask; } };
        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => source.ExtractAsync("live", null, PipelineTests.Now));
        Assert.Contains($"HTTP {status}", error.Message);
        Assert.Equal(expectedAttempts, attempts);
        Assert.Equal(expectedAttempts == 1 ? [] : new[] { 0.5, 1.0 }, delays);
    }

    [Fact]
    public async Task NetworkErrorsDoNotFallBackToFixture()
    {
        var attempts = 0;
        using var client = new HttpClient(new Handler(_ => { attempts++; throw new HttpRequestException("UnknownIssuer"); }));
        var source = new SourceReader(client, new(PipelineTests.Fixtures, temporary)) { Delay = (_, _) => Task.CompletedTask };
        await Assert.ThrowsAsync<InvalidOperationException>(() => source.ExtractAsync("live", null, PipelineTests.Now));
        Assert.Equal(3, attempts);
    }

    [Theory]
    [InlineData("{\"total_count\":1,\"results\":[]}")]
    [InlineData("{\"total_count\":0,\"results\":[]}")]
    [InlineData("{\"results\":{}}")]
    public async Task EmptyOrIncompleteLiveDataFails(string json)
    {
        using var client = new HttpClient(new Handler(_ => Response(json)));
        await Assert.ThrowsAsync<ArgumentException>(() => new SourceReader(client, new("unused", temporary)).ExtractAsync("live", null, PipelineTests.Now));
    }

    [Theory]
    [InlineData("2026-02-30")]
    [InlineData("2026-2-03")]
    [InlineData("2026-11-05")]
    [InlineData("2027-01-01")]
    public async Task InvalidOrUnfinishedDatesFailBeforeNetwork(string day)
    {
        using var client = new HttpClient(new Handler(_ => throw new Xunit.Sdk.XunitException("Network should not be called")));
        await Assert.ThrowsAsync<ArgumentException>(() => new SourceReader(client, new("unused", temporary)).ExtractAsync("live", day, PipelineTests.Now));
    }

    [Fact]
    public async Task SnapshotRoundTripAndTamperDetection()
    {
        using var client = new HttpClient();
        var bundle = await new SourceReader(client, new(PipelineTests.Fixtures, temporary)).ExtractAsync("fixture", "2026-10-17", PipelineTests.Now);
        await SourceReader.WriteSnapshotAsync(bundle, temporary);
        var reader = new SourceReader(client, new(temporary, "unused"));
        var restored = await reader.ExtractAsync("fixture", null, PipelineTests.Now);
        Assert.Equal(96, restored.Demand.Count);
        Assert.Equal(72, restored.Hydrometeo.Count);
        await Assert.ThrowsAsync<ArgumentException>(() => reader.ExtractAsync("fixture", "2026-10-16", PipelineTests.Now));
        await File.AppendAllTextAsync(Path.Combine(temporary, "demand.json"), " ");
        var error = await Assert.ThrowsAsync<ArgumentException>(() => reader.ExtractAsync("fixture", null, PipelineTests.Now));
        Assert.Contains("intégrité", error.Message);
    }

    private sealed class Handler(Func<HttpRequestMessage, HttpResponseMessage> response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(response(request));
    }
    public void Dispose()
    {
        if (Directory.Exists(temporary) && Path.GetFileName(temporary).StartsWith("hydro-source-tests-", StringComparison.Ordinal)) Directory.Delete(temporary, true);
    }
}
