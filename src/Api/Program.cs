using HydroDataLab.Api;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var etlBaseUrl = builder.Configuration["Etl:BaseUrl"] ?? builder.Configuration["Python:BaseUrl"];
if (!Uri.TryCreate(etlBaseUrl, UriKind.Absolute, out var etlEndpoint)
    || (etlEndpoint.Scheme != Uri.UriSchemeHttp && etlEndpoint.Scheme != Uri.UriSchemeHttps))
    throw new InvalidOperationException("Configurer Etl__BaseUrl avec l'adresse HTTP du service ETL. Aspire la fournit automatiquement; Python__BaseUrl reste accepté.");
var dataDirectory = Path.GetFullPath(builder.Configuration["Hydro:DataDir"] ?? ".runtime/api");
Directory.CreateDirectory(dataDirectory);
builder.Services.AddSingleton(new RunStore(Path.Combine(dataDirectory, "hydrolab.sqlite")));
builder.Services.AddSingleton<PipelineCoordinator>();
builder.Services.AddHttpClient("etl", client =>
{
    client.BaseAddress = new Uri(etlEndpoint.AbsoluteUri.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromMinutes(3);
});
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();
builder.Services.AddQuartz(q =>
{
    q.UsePersistentStore(store =>
    {
        store.UseSqlite($"Data Source={Path.Combine(dataDirectory, "quartz.sqlite")}");
        store.ProvisionSchema();
        store.ConfigureStore(options => options.StoreJobDataAsStrings = true);
    });
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddSource(PipelineJob.ActivitySourceName))
    .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation());
if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
{
    builder.Services.AddOpenTelemetry().WithTracing(t => t.AddOtlpExporter()).WithMetrics(m => m.AddOtlpExporter());
    builder.Logging.AddOpenTelemetry(logging => { logging.IncludeFormattedMessage = true; logging.IncludeScopes = true; logging.AddOtlpExporter(); });
}
var app = builder.Build();
app.UseExceptionHandler();
var store = app.Services.GetRequiredService<RunStore>();
store.Initialize();
store.InterruptInFlight();
app.MapHealthChecks("/health");
app.MapGet("/api/runs", (RunStore db) => db.List());
app.MapGet("/api/runs/{id}", (string id, RunStore db) => db.Get(id) is { } run ? Results.Ok(run) : Results.NotFound());
app.MapGet("/api/runs/{id}/details", (string id, RunStore db) => db.Details(id) is { } details ? Results.Ok(details) : Results.NotFound());
app.MapPost("/api/runs", async (RunRequest request, PipelineCoordinator coordinator) =>
{
    if (!request.IsValid) return Results.BadRequest(new { message = "Source ou scénario non reconnu." });
    var id = await coordinator.QueueAsync(request);
    return id is null ? Results.Conflict(new { message = "Une exécution est déjà en cours." }) : Results.Accepted($"/api/runs/{id}", new { id });
});
app.MapPost("/api/runs/{id}/retry", async (string id, RunStore db, PipelineCoordinator coordinator) =>
{
    var previous = db.Get(id);
    if (previous is null) return Results.NotFound();
    if (previous.Status is not ("failed" or "interrupted")) return Results.BadRequest(new { message = "Seules les exécutions en échec ou interrompues sont relançables." });
    var next = await coordinator.QueueAsync(new(previous.Source, false, previous.QualityScenario), previous.Id);
    return next is null ? Results.Conflict(new { message = "Une exécution est déjà en cours." }) : Results.Accepted($"/api/runs/{next}", new { id = next });
});
app.MapGet("/api/report", (RunStore db) => db.LatestReport() is { } report ? Results.Content(report, "application/json") : Results.NotFound(new { message = "Lancez un premier traitement pour afficher les observations." }));
app.MapGet("/api/schedule", (RunStore db) => db.ReadSchedule());
app.MapPut("/api/schedule", async (ScheduleSettings settings, PipelineCoordinator coordinator) =>
{
    if (settings.IntervalMinutes is < 1 or > 60) return Results.BadRequest(new { message = "L'intervalle doit être compris entre 1 et 60 minutes." });
    await coordinator.SetScheduleAsync(settings);
    return Results.Ok(settings);
});
app.MapGet("/api/artifacts/{runId}/{file}", async (string runId, string file, RunStore db, IHttpClientFactory clients, CancellationToken cancellationToken) =>
{
    if (!Guid.TryParseExact(runId, "D", out _) || !new[] { "observations.csv", "observations.parquet", "report.json", "hourly.csv" }.Contains(file)) return Results.BadRequest();
    if (db.Get(runId)?.Status != "succeeded") return Results.NotFound();
    using var response = await clients.CreateClient("etl").GetAsync($"artifacts/{runId}/{file}", cancellationToken);
    if (!response.IsSuccessStatusCode) return Results.NotFound(new { message = "Export indisponible dans ce répertoire d'exécution." });
    var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
    var contentType = file.EndsWith(".csv") ? "text/csv; charset=utf-8" : file.EndsWith(".json") ? "application/json" : "application/vnd.apache.parquet";
    return Results.File(bytes, contentType, file);
});
await app.Services.GetRequiredService<PipelineCoordinator>().RestoreScheduleAsync();
app.Run();
public partial class Program;
