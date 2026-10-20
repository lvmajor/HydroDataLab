using System.Globalization;
using System.Text.Json.Nodes;

namespace HydroDataLab.Etl;

/// <summary>Pure calculations shared by the HTTP and command-line adapters.</summary>
public static class Calculations
{
    public static readonly IReadOnlyList<SeriesDefinition> Series = [
        new("demand", "Demande électrique", "Québec (hors réseaux autonomes)", "MW", "demand", 15),
        new("water_level", "Niveau d’eau", "Réservoir Gouin Nord", "m", "water_level", 60),
        new("temperature_min", "Température minimale horaire", "Gouin Bois (CM2T)", "°C", "temperature_min", 60),
        new("temperature_max", "Température maximale horaire", "Gouin Bois (CM2T)", "°C", "temperature_max", 60)
    ];

    public static Normalized Normalize(RawBundle bundle, DateTimeOffset now)
    {
        var (start, end) = bundle.Period.Bounds();
        var records = new Dictionary<(string, DateTimeOffset), Observation>();
        int invalid = 0, missing = 0, duplicates = 0;
        void Add(string id, JsonObject item, JsonNode? raw, string unit)
        {
            var metadata = Series.Single(s => s.Id == id);
            if (!Time.TryInstant(Json.Text(item["date"]), out var timestamp) || timestamp < start || timestamp >= end
                || timestamp > now || unit != metadata.Unit) { invalid++; return; }
            double? value = null;
            if (raw is null || raw is JsonValue empty && empty.TryGetValue<string>(out var text) && text == "") missing++;
            else if (raw is JsonValue scalar && scalar.GetValueKind() is System.Text.Json.JsonValueKind.Number or System.Text.Json.JsonValueKind.String
                && double.TryParse(raw is JsonValue str && str.TryGetValue<string>(out var number) ? number : raw.ToJsonString(),
                    NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) && double.IsFinite(parsed)) value = parsed;
            else invalid++;
            if (records.ContainsKey((id, timestamp))) duplicates++;
            records[(id, timestamp)] = new(id, timestamp, value, unit, metadata.Station);
        }
        foreach (var item in bundle.Demand) Add("demand", item, item["valeurs_demandetotal"], "MW");
        foreach (var item in bundle.Hydrometeo)
        {
            var id = (Json.Text(item["identifiant"]), Json.Text(item["composition_depil_type_point_donnee"])) switch
            {
                ("1-15412", "Niveau") => "water_level",
                ("1-10622", "Température Minimum") => "temperature_min",
                ("1-10622", "Température Maximum") => "temperature_max",
                _ => null
            };
            if (id is not null) Add(id, item, item["valeur"], Json.Text(item["composition_depil_nom_unite_mesure"]));
        }
        List<string> warnings = [];
        if (invalid > 0) warnings.Add($"{invalid} observation(s) invalide(s), future(s) ou hors période : exclue(s) ou valeur laissée absente.");
        if (missing > 0) warnings.Add($"{missing} valeur(s) absente(s) : aucun remplacement par zéro.");
        if (duplicates > 0) warnings.Add($"{duplicates} doublon(s) supprimé(s) ; la dernière occurrence de chaque série/instant est conservée.");
        return new(records.Values.OrderBy(r => r.SeriesId, StringComparer.Ordinal).ThenBy(r => r.Timestamp).ToList(), invalid, duplicates, warnings);
    }

    public static Coverage Coverage(IEnumerable<DateTimeOffset> timestamps, Period period, int intervalMinutes)
    {
        if (intervalMinutes <= 0 || 60 % intervalMinutes != 0) throw new ArgumentException("L'intervalle doit être positif et diviser 60");
        var (start, end) = period.Bounds();
        var step = TimeSpan.FromMinutes(intervalMinutes).Ticks;
        var expected = checked((int)((end - start).Ticks / step));
        var observed = timestamps.Where(t => t >= start && t < end && (t - start).Ticks % step == 0).Distinct().Count();
        return new(observed, expected, Math.Round(100.0 * observed / expected, 2, MidpointRounding.ToEven));
    }

    public static Analysis Analyze(IEnumerable<Observation> observations, Period period, DateTimeOffset now)
    {
        var rows = observations.ToList();
        var (start, end) = period.Bounds();
        var demand = rows.Where(r => r.SeriesId == "demand").GroupBy(r => r.Timestamp).ToDictionary(g => g.Key, g => g.Last().Value);
        List<Hour> hourly = [];
        for (var hour = start; hour < end && hour.AddHours(1) <= now; hour = hour.AddHours(1))
        {
            var values = Enumerable.Range(0, 4).Select(i => demand.GetValueOrDefault(hour.AddMinutes(15 * i))).ToArray();
            var observed = values.Count(v => v.HasValue);
            hourly.Add(new(Time.Utc(hour), Time.Local(hour, period.Timezone), observed, 4, observed == 4 ? values.Average() : null));
        }
        var complete = hourly.Where(h => h.MeanDemandMW.HasValue).ToList();
        var peak = complete.MaxBy(h => h.MeanDemandMW);
        var fullPeriod = end <= now && complete.Count == hourly.Count && hourly.Count > 0;
        var water = rows.Where(r => r.SeriesId == "water_level" && r.Value.HasValue && r.Timestamp >= start && r.Timestamp < end && r.Timestamp <= now)
            .GroupBy(r => r.Timestamp).Select(g => g.Last()).OrderBy(r => r.Timestamp).ToList();
        var endpoints = water.Count >= 2;
        return new(hourly, complete.Count, hourly.Count, new(peak?.MeanDemandMW, peak?.StartUtc,
            fullPeriod ? complete.Sum(h => h.MeanDemandMW!.Value) : null,
            endpoints ? Math.Round((water[^1].Value!.Value - water[0].Value!.Value) * 100, 6, MidpointRounding.ToEven) : null,
            endpoints ? Time.Utc(water[0].Timestamp) : null, endpoints ? Time.Utc(water[^1].Timestamp) : null));
    }
}
