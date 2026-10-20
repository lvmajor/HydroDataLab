using System.Globalization;
using System.Text;
using Parquet;
using Parquet.Schema;

namespace HydroDataLab.Etl;

public static class Exports
{
    private static string Csv(string value) => value.IndexOfAny([',', '"', '\r', '\n']) >= 0 ? '"' + value.Replace("\"", "\"\"") + '"' : value;
    private static string Number(double? value)
    {
        if (!value.HasValue) return "";
        var text = value.Value.ToString("R", CultureInfo.InvariantCulture);
        return text.IndexOfAny(['.', 'E', 'e']) < 0 ? text + ".0" : text;
    }
    public static async Task WriteAsync(string directory, IReadOnlyList<Observation> rows, Analysis analysis, CancellationToken cancellationToken)
    {
        var csv = new StringBuilder("series_id,timestamp_utc,value,unit,station\n");
        foreach (var row in rows)
            csv.AppendJoin(',', Csv(row.SeriesId), row.Timestamp.UtcDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture),
                Number(row.Value), Csv(row.Unit), Csv(row.Station)).Append('\n');
        await File.WriteAllTextAsync(Path.Combine(directory, "observations.csv"), csv.ToString(), new UTF8Encoding(false), cancellationToken);
        var hourly = new StringBuilder("startUtc,localHour,observed,expected,meanDemandMW\n");
        foreach (var hour in analysis.Hourly)
            hourly.AppendJoin(',', hour.StartUtc, hour.LocalHour, hour.Observed.ToString(CultureInfo.InvariantCulture), "4", Number(hour.MeanDemandMW)).Append('\n');
        await File.WriteAllTextAsync(Path.Combine(directory, "hourly.csv"), hourly.ToString(), new UTF8Encoding(false), cancellationToken);

        var series = new DataField<string>("series_id", true);
        var timestamps = new DateTimeDataField("timestamp_utc", DateTimeFormat.Timestamp, isAdjustedToUTC: true, unit: DateTimeTimeUnit.Micros, isNullable: true);
        var values = new DataField<double?>("value");
        var units = new DataField<string>("unit", true);
        var stations = new DataField<string>("station", true);
        var schema = new ParquetSchema(series, timestamps, values, units, stations);
        await using var stream = File.Create(Path.Combine(directory, "observations.parquet"));
        await using var writer = await ParquetWriter.CreateAsync(schema, stream, cancellationToken: cancellationToken);
        using var group = writer.CreateRowGroup();
        await group.WriteAsync(series, rows.Select(r => r.SeriesId).ToArray());
        await group.WriteAsync<DateTime>(timestamps, rows.Select(r => (DateTime?)r.Timestamp.UtcDateTime).ToArray().AsMemory(), cancellationToken: cancellationToken);
        await group.WriteAsync<double>(values, rows.Select(r => r.Value).ToArray().AsMemory(), cancellationToken: cancellationToken);
        await group.WriteAsync(units, rows.Select(r => r.Unit).ToArray());
        await group.WriteAsync(stations, rows.Select(r => r.Station).ToArray());
        group.CompleteValidate();
    }
}
