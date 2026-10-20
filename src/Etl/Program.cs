using System.Text.Json;
using System.Text.Json.Nodes;
using HydroDataLab.Etl;

public partial class EtlProgram
{
    public static async Task<int> Main(string[] args)
    {
        if (args.FirstOrDefault() is "run" or "extract" or "--help" or "-h") return await CommandLine.RunAsync(args);
        await Build(args).RunAsync();
        return 0;
    }

    public static WebApplication Build(string[] args, Action<WebApplicationBuilder>? configure = null)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton(services => EtlPaths.FromConfiguration(services.GetRequiredService<IConfiguration>()));
        builder.Services.AddHttpClient<ISourceReader, SourceReader>();
        builder.Services.AddTransient<Pipeline>();
        builder.Services.AddSingleton<RunGate>();
        configure?.Invoke(builder);
        var app = builder.Build();
        app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
        app.MapPost("/run", async (HttpRequest request, Pipeline pipeline, RunGate gate, ILogger<EtlProgram> logger, CancellationToken cancellationToken) =>
        {
            RunOptions options;
            try
            {
                var body = await JsonNode.ParseAsync(request.Body, cancellationToken: cancellationToken) as JsonObject
                    ?? throw new ArgumentException("Un objet JSON est requis");
                var id = Json.Text(body["runId"]);
                if (!Guid.TryParse(id, out var runId)) throw new ArgumentException("runId doit être un UUID");
                var source = body.ContainsKey("source") ? Json.Text(body["source"]) : "fixture";
                var quality = body.ContainsKey("qualityScenario") ? Json.Text(body["qualityScenario"]) : "complete";
                if (source is not ("fixture" or "live") || quality is not ("complete" or "missing")) throw new ArgumentException("Source ou scénario inconnu");
                var failure = false;
                if (body.ContainsKey("simulateFailure") && (body["simulateFailure"] is not JsonValue value || !value.TryGetValue(out failure)))
                    throw new ArgumentException("simulateFailure doit être un booléen");
                options = new(source, runId.ToString("D"), failure, quality);
            }
            catch (Exception error) when (error is JsonException or ArgumentException or InvalidOperationException)
            { return Error(error, 422); }
            if (!await gate.Semaphore.WaitAsync(0, cancellationToken))
                return Results.Json(new { detail = "Un pipeline ETL est déjà en cours. Réessayez après sa fin." }, statusCode: 409);
            using var scope = logger.BeginScope(new Dictionary<string, object?> { ["RunId"] = options.RunId });
            try
            {
                logger.LogInformation("Démarrage du pipeline .NET {RunId}", options.RunId);
                var report = await pipeline.RunAsync(options, cancellationToken);
                logger.LogInformation("Pipeline terminé {RunId}", options.RunId);
                return Results.Json(report, Json.Options);
            }
            catch (Exception error) when (error is ArgumentException or FormatException or JsonException)
            { logger.LogWarning(error, "Données refusées {RunId}", options.RunId); return Error(error, 422); }
            catch (Exception error) when (error is InvalidOperationException or IOException or UnauthorizedAccessException or HttpRequestException)
            { logger.LogError(error, "Pipeline en échec {RunId}", options.RunId); return Error(error, 503); }
            finally { gate.Semaphore.Release(); }
        });
        app.MapGet("/artifacts/{runId}/{filename}", (string runId, string filename, Pipeline pipeline) =>
        {
            if (!Guid.TryParse(runId, out _)) return Error(new ArgumentException("runId doit être un UUID"), 422);
            if (!Pipeline.ArtifactNames.Contains(filename)) return Error(new ArgumentException("Fichier non autorisé"), 400);
            var path = pipeline.ArtifactPath(runId, filename);
            if (!File.Exists(path)) return Results.Json(new { detail = "Export introuvable" }, statusCode: 404);
            var contentType = filename.EndsWith(".csv", StringComparison.Ordinal) ? "text/csv; charset=utf-8" : filename.EndsWith(".json", StringComparison.Ordinal) ? "application/json" : "application/vnd.apache.parquet";
            return Results.File(path, contentType, filename);
        });
        return app;
    }

    private static IResult Error(Exception error, int status) => Results.Json(new
        { detail = new { message = error.Message, execution = error.Data["execution"] as JsonObject } }, Json.Options, statusCode: status);
}

public sealed class RunGate : IDisposable
{
    public SemaphoreSlim Semaphore { get; } = new(1, 1);
    public void Dispose() => Semaphore.Dispose();
}
