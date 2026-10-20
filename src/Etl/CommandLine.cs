using System.Text.Json;
using System.Text.Json.Nodes;

namespace HydroDataLab.Etl;

public static class CommandLine
{
    public static async Task<int> RunAsync(string[] args)
    {
        if (args.Any(a => a is "--help" or "-h"))
        {
            Console.WriteLine("HydroDataLab ETL .NET\nSans sous-commande : serveur HTTP ASP.NET Core.\nrun|extract --source fixture|live --date YYYY-MM-DD --output DIR\nrun : --run-id UUID --quality-scenario complete|missing --simulate-failure\nextract : --output obligatoire. Configuration : HYDRO_FIXTURE_DIR, HYDRO_OUTPUT_DIR.");
            return 0;
        }
        try
        {
            Dictionary<string, string> options = [];
            for (var i = 1; i < args.Length; i++)
            {
                var option = args[i];
                if (option == "--simulate-failure" && args[0] == "run") { options[option] = "true"; continue; }
                if (option is not ("--source" or "--date" or "--output" or "--run-id" or "--quality-scenario")
                    || args[0] == "extract" && option is "--run-id" or "--quality-scenario"
                    || i + 1 >= args.Length || args[i + 1].StartsWith("--", StringComparison.Ordinal))
                    throw new ArgumentException($"Option inconnue ou valeur absente : {option}");
                options[option] = args[++i];
            }
            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            var paths = EtlPaths.FromConfiguration(config);
            if (options.TryGetValue("--output", out var output)) paths = paths with { Output = Path.GetFullPath(output) };
            using var client = new HttpClient();
            var source = new SourceReader(client, paths);
            var sourceName = options.GetValueOrDefault("--source", "fixture");
            if (args[0] == "extract")
            {
                if (output is null) throw new ArgumentException("extract requiert --output DIR");
                var bundle = await source.ExtractAsync(sourceName, options.GetValueOrDefault("--date"), DateTimeOffset.UtcNow);
                await SourceReader.WriteSnapshotAsync(bundle, output);
                Console.WriteLine(Json.Node(new { output, records = bundle.Demand.Count + bundle.Hydrometeo.Count }).ToJsonString(Json.Options));
            }
            else
            {
                var report = await new Pipeline(source, paths).RunAsync(new(sourceName, options.GetValueOrDefault("--run-id"),
                    options.ContainsKey("--simulate-failure"), options.GetValueOrDefault("--quality-scenario", "complete"), options.GetValueOrDefault("--date")));
                Console.WriteLine(new JsonObject
                {
                    ["runId"] = report["runId"]!.DeepClone(), ["summary"] = report["summary"]!.DeepClone(),
                    ["warnings"] = report["warnings"]!.DeepClone(), ["indicators"] = report["analysis"]!["indicators"]!.DeepClone()
                }.ToJsonString(Json.Options));
            }
            return 0;
        }
        catch (Exception error) when (error is ArgumentException or FormatException or JsonException or InvalidOperationException or IOException or UnauthorizedAccessException or HttpRequestException)
        {
            Console.Error.WriteLine($"Échec : {error.Message}");
            return 1;
        }
    }
}
