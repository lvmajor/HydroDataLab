using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HydroDataLab.Etl;

public interface ISourceReader
{
    Task<RawBundle> ExtractAsync(string source, string? day, DateTimeOffset now, CancellationToken cancellationToken);
}

public sealed class SourceReader(HttpClient client, EtlPaths paths) : ISourceReader
{
    private const string BaseUrl = "https://donnees.hydroquebec.com/api/explore/v2.1/catalog/datasets";
    public Func<TimeSpan, CancellationToken, Task> Delay { get; init; } = Task.Delay;

    public async Task<RawBundle> ExtractAsync(string source, string? day, DateTimeOffset now, CancellationToken cancellationToken = default)
    {
        var selectedDay = Time.ParseDay(day);
        var localDay = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(now, "America/Toronto").DateTime);
        if (selectedDay >= localDay) throw new ArgumentException("La date doit désigner une journée terminée dans America/Toronto");
        if (source == "fixture")
        {
            var manifest = JsonNode.Parse(await File.ReadAllTextAsync(Path.Combine(paths.Fixtures, "manifest.json"), cancellationToken))!;
            var period = manifest["period"]!.Deserialize<Period>(Json.Options)!;
            if (selectedDay.HasValue && day != period.Start)
                throw new ArgumentException($"Le snapshot couvre uniquement le {period.Start}; la date demandée est refusée.");
            var provenance = manifest["sources"]!.AsArray();
            List<List<JsonObject>> payloads = [];
            foreach (var name in new[] { "demand.json", "hydrometeo.json" })
            {
                var raw = await File.ReadAllBytesAsync(Path.Combine(paths.Fixtures, name), cancellationToken);
                var metadata = provenance.Single(p => Json.Text(p?["file"]) == name)!;
                if (Convert.ToHexStringLower(SHA256.HashData(raw)) != Json.Text(metadata["sha256"]))
                    throw new ArgumentException($"L'intégrité du snapshot {name} n'est plus valide.");
                // JSON files exported by PowerShell may have a UTF-8 BOM.
                var offset = raw.AsSpan().StartsWith(new byte[] { 0xef, 0xbb, 0xbf }) ? 3 : 0;
                payloads.Add(Rows(JsonNode.Parse(raw.AsSpan(offset))!));
            }
            return new(payloads[0], payloads[1], period, (JsonArray)provenance.DeepClone());
        }
        if (source != "live") throw new ArgumentException("source doit être fixture ou live");
        var startDay = selectedDay ?? localDay.AddDays(-1);
        var livePeriod = new Period(startDay.ToString("yyyy-MM-dd"), startDay.AddDays(1).ToString("yyyy-MM-dd"));
        var (start, end) = livePeriod.Bounds();
        var where = $"date >= \"{Time.Local(start, livePeriod.Timezone)}\" AND date < \"{Time.Local(end, livePeriod.Timezone)}\"";
        var queries = new[] {
            ("demande-electricite-quebec", where),
            ("donnees-hydrometeorologiques", where + " AND ((identifiant=\"1-15412\" AND composition_depil_type_point_donnee=\"Niveau\") OR (identifiant=\"1-10622\" AND composition_depil_type_point_donnee IN (\"Température Minimum\",\"Température Maximum\")))")
        };
        List<List<JsonObject>> allRows = [];
        JsonArray sources = [];
        foreach (var (dataset, filter) in queries)
        {
            List<JsonObject> rows = [];
            string? firstUrl = null;
            var offset = 0;
            while (true)
            {
                var url = $"{BaseUrl}/{dataset}/records?where={Uri.EscapeDataString(filter).Replace("%20", "+")}&order_by=date&limit=100&offset={offset}";
                firstUrl ??= url;
                var payload = await RequestAsync(url, cancellationToken);
                var page = Rows(payload);
                rows.AddRange(page);
                var total = payload["total_count"]?.GetValue<int>() ?? rows.Count;
                if (page.Count == 0 && rows.Count < total)
                    throw new ArgumentException("Pagination Hydro-Québec incomplète : page vide avant la fin annoncée.");
                if (rows.Count >= total) break;
                offset += page.Count;
                if (offset > 1000) throw new ArgumentException("Réponse trop volumineuse pour cette journée de démonstration.");
            }
            if (rows.Count == 0) throw new ArgumentException($"Aucune donnée pour {dataset} le {livePeriod.Start}. Aucun remplacement silencieux.");
            allRows.Add(rows);
            sources.Add(Json.Node(new { label = "Hydro-Québec", url = firstUrl, license = "CC BY-NC 4.0", retrievedAtUtc = Time.Now(), recordCount = rows.Count }));
        }
        return new(allRows[0], allRows[1], livePeriod, sources);
    }

    private static List<JsonObject> Rows(JsonNode payload)
    {
        if (payload["results"] is not JsonArray array || array.Any(row => row is not JsonObject))
            throw new ArgumentException("Réponse Hydro-Québec invalide : results doit être une liste d'objets.");
        return array.Select(row => (JsonObject)row!.DeepClone()).ToList();
    }

    private async Task<JsonNode> RequestAsync(string url, CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 3; attempt++)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.ParseAdd("application/json");
                request.Headers.UserAgent.ParseAdd("HydroDataLab-Training/1.0");
                using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeout.CancelAfter(TimeSpan.FromSeconds(20));
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
                if (response.IsSuccessStatusCode)
                    return JsonNode.Parse(await response.Content.ReadAsStringAsync(timeout.Token)) ?? throw new ArgumentException("Réponse JSON vide");
                if (attempt == 2 || (int)response.StatusCode is not (429 or 500 or 502 or 503 or 504))
                    throw new InvalidOperationException($"Source Hydro-Québec indisponible (HTTP {(int)response.StatusCode}).");
            }
            catch (Exception error) when (error is HttpRequestException or IOException || error is OperationCanceledException && !cancellationToken.IsCancellationRequested)
            {
                if (attempt == 2) throw new InvalidOperationException("Source Hydro-Québec inaccessible. Vérifier le réseau ou sélectionner le mode fixture.", error);
            }
            await Delay(TimeSpan.FromSeconds(0.5 * Math.Pow(2, attempt)), cancellationToken);
        }
        throw new InvalidOperationException("Nombre maximal de tentatives dépassé");
    }

    public static async Task WriteSnapshotAsync(RawBundle bundle, string output, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(output);
        JsonArray sources = [];
        var index = 0;
        foreach (var (name, rows) in new[] { ("demand.json", bundle.Demand), ("hydrometeo.json", bundle.Hydrometeo) })
        {
            var raw = JsonSerializer.SerializeToUtf8Bytes(new { total_count = rows.Count, results = rows }, Json.Options);
            await File.WriteAllBytesAsync(Path.Combine(output, name), raw, cancellationToken);
            var source = (JsonObject)bundle.Provenance[index++]!.DeepClone();
            source["file"] = name;
            source["recordCount"] = rows.Count;
            source["sha256"] = Convert.ToHexStringLower(SHA256.HashData(raw));
            sources.Add(source);
        }
        var manifest = new JsonObject { ["schemaVersion"] = 1, ["period"] = Json.Node(bundle.Period), ["sources"] = sources };
        await File.WriteAllTextAsync(Path.Combine(output, "manifest.json"), manifest.ToJsonString(Json.Options), cancellationToken);
    }
}
