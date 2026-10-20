using System.Diagnostics;
using System.Text.Json.Nodes;

namespace HydroDataLab.Etl;

public sealed class Pipeline(ISourceReader sourceReader, EtlPaths paths)
{
    public static readonly IReadOnlySet<string> ArtifactNames = new HashSet<string>(StringComparer.Ordinal)
        { "observations.csv", "observations.parquet", "report.json", "hourly.csv" };

    public string ArtifactPath(string runId, string name)
    {
        if (!ArtifactNames.Contains(name)) throw new ArgumentException("Fichier non autorisé");
        return Path.Combine(Path.GetFullPath(paths.Output), Guid.Parse(runId).ToString("D"), name);
    }

    public async Task<JsonObject> RunAsync(RunOptions options, CancellationToken cancellationToken = default)
    {
        if (options.Source is not ("fixture" or "live") || options.QualityScenario is not ("complete" or "missing"))
            throw new ArgumentException("Source ou scénario inconnu");
        var runId = options.RunId is null ? Guid.NewGuid().ToString("D") : Guid.Parse(options.RunId).ToString("D");
        Time.ParseDay(options.Day);
        var root = Path.GetFullPath(paths.Output);
        var destination = Path.Combine(root, runId);
        var existing = Path.Combine(destination, "report.json");
        async Task<JsonObject> ReplayAsync()
        {
            var report = JsonNode.Parse(await File.ReadAllTextAsync(existing, cancellationToken))!.AsObject();
            if (Json.Text(report["source"]) != options.Source || Json.Text(report["qualityScenario"]) != options.QualityScenario
                || options.Day is not null && Json.Text(report["period"]?["start"]) != options.Day)
                throw new ArgumentException("Ce runId existe avec des paramètres différents");
            return report;
        }
        if (File.Exists(existing)) return await ReplayAsync();
        var now = options.Now ?? DateTimeOffset.UtcNow;
        var execution = new Execution();
        var bundle = await execution.StepAsync("extract", "Extraire les sources", async () =>
        {
            if (options.SimulateFailure) throw new InvalidOperationException("Échec simulé du pipeline pour la démonstration. Relancez avec simulateFailure=false.");
            return await sourceReader.ExtractAsync(options.Source, options.Day, now, cancellationToken);
        });
        var normalized = await execution.StepAsync("normalize", "Valider et normaliser", () =>
        {
            var result = Calculations.Normalize(bundle, now);
            if (options.QualityScenario == "missing")
            {
                var index = result.Rows.FindIndex(r => r.SeriesId == "water_level" && r.Value.HasValue);
                if (index < 0) throw new ArgumentException("Aucune observation valide de niveau d'eau à retirer pour ce scénario");
                result.Rows.RemoveAt(index);
                result.Warnings.Insert(0, "Scénario pédagogique : une observation de niveau d'eau a été retirée uniquement de la copie en mémoire.");
            }
            return Task.FromResult(result);
        });
        var rows = normalized.Rows;
        var (analysis, series) = await execution.StepAsync("analyze", "Calculer la synthèse et les indicateurs", () =>
        {
            var analysis = Calculations.Analyze(rows, bundle.Period, now);
            JsonArray series = [];
            foreach (var metadata in Calculations.Series)
            {
                var subset = rows.Where(r => r.SeriesId == metadata.Id).ToList();
                var valid = subset.Where(r => r.Value.HasValue).ToList();
                if (valid.Count == 0) throw new ArgumentException($"Aucune mesure valide pour la série {metadata.Id}");
                var item = Json.Node(metadata).AsObject();
                item["points"] = Json.Node(subset.Select(r => new { timestamp = Time.Utc(r.Timestamp), value = r.Value }));
                item["minimum"] = valid.Min(r => r.Value);
                item["maximum"] = valid.Max(r => r.Value);
                item["latest"] = valid[^1].Value;
                item["coverage"] = Json.Node(Calculations.Coverage(valid.Select(r => r.Timestamp), bundle.Period, metadata.IntervalMinutes));
                series.Add(item);
            }
            return Task.FromResult((analysis, series));
        });
        var report = new JsonObject
        {
            ["schemaVersion"] = 1, ["runId"] = runId, ["source"] = options.Source, ["generatedAtUtc"] = Time.Now(),
            ["period"] = Json.Node(bundle.Period), ["qualityScenario"] = options.QualityScenario,
            ["summary"] = Json.Node(new { observationCount = rows.Count(r => r.Value.HasValue), seriesCount = series.Count, normalized.InvalidCount, normalized.DuplicateCount }),
            ["analysis"] = Json.Node(analysis),
            ["etl"] = Json.Node(new
            {
                rawCount = bundle.Demand.Count + bundle.Hydrometeo.Count,
                normalizedCount = rows.Count, validCount = rows.Count(r => r.Value.HasValue), missingCount = rows.Count(r => !r.Value.HasValue),
                scenarioRemovedCount = options.QualityScenario == "missing" ? 1 : 0,
                rawDemandSample = bundle.Demand.Take(4).Select(r => new { date = r["date"], valeurs_demandetotal = r["valeurs_demandetotal"] }),
                normalizedDemandSample = rows.Where(r => r.SeriesId == "demand").Take(4).Select(r => new { timestamp_utc = Time.Utc(r.Timestamp), value = r.Value, unit = r.Unit })
            }),
            ["series"] = series, ["provenance"] = bundle.Provenance.DeepClone(), ["warnings"] = Json.Node(normalized.Warnings),
            ["artifacts"] = Json.Node(new { csv = "observations.csv", parquet = "observations.parquet", report = "report.json", hourly = "hourly.csv" })
        };
        Directory.CreateDirectory(root);
        var staging = Path.Combine(root, $".{runId}-{Guid.NewGuid():N}");
        Directory.CreateDirectory(staging);
        try
        {
            await execution.StepAsync("export", "Écrire les exports CSV et Parquet", async () =>
            {
                await Exports.WriteAsync(staging, rows, analysis, cancellationToken);
                return true;
            });
            report["execution"] = execution.Snapshot();
            await File.WriteAllTextAsync(Path.Combine(staging, "report.json"), report.ToJsonString(Json.Options), cancellationToken);
            try { Directory.Move(staging, destination); }
            catch (IOException) when (File.Exists(existing)) { return await ReplayAsync(); }
        }
        finally
        {
            // Only a freshly created sibling staging directory may be removed.
            if (Directory.Exists(staging))
            {
                if (Directory.GetParent(Path.GetFullPath(staging))?.FullName != root || !Path.GetFileName(staging).StartsWith($".{runId}-", StringComparison.Ordinal))
                    throw new InvalidOperationException("Chemin temporaire inattendu : nettoyage refusé");
                Directory.Delete(staging, recursive: true);
            }
        }
        return report;
    }
}

public sealed class Execution
{
    private readonly JsonArray steps = [];
    public JsonObject Snapshot() => new() { ["steps"] = steps.DeepClone() };
    public async Task<T> StepAsync<T>(string id, string label, Func<Task<T>> work)
    {
        var step = new JsonObject { ["id"] = id, ["label"] = label, ["startedAtUtc"] = Time.Now(), ["status"] = "running" };
        steps.Add(step);
        var timer = Stopwatch.StartNew();
        try
        {
            var result = await work();
            step["status"] = "succeeded";
            return result;
        }
        catch (Exception error)
        {
            step["status"] = "failed";
            step["error"] = error.Message;
            Complete();
            error.Data["execution"] = Snapshot();
            throw;
        }
        finally { Complete(); }
        void Complete()
        {
            step["durationMs"] = Math.Round(timer.Elapsed.TotalMilliseconds, 3);
            step["completedAtUtc"] = Time.Now();
        }
    }
}
