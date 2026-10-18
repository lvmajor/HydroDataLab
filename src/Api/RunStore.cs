using Microsoft.Data.Sqlite;
namespace HydroDataLab.Api;

public sealed class RunStore(string databasePath)
{
    private SqliteConnection Open()
    {
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = databasePath, DefaultTimeout = 10 }.ToString());
        connection.Open();
        return connection;
    }
    public void Initialize()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(databasePath))!);
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            PRAGMA journal_mode=WAL;
            CREATE TABLE IF NOT EXISTS runs (
              id TEXT PRIMARY KEY, source TEXT NOT NULL, status TEXT NOT NULL, stage TEXT NOT NULL,
              created TEXT NOT NULL, started TEXT, completed TEXT, error TEXT, scenario TEXT NOT NULL, report TEXT);
            CREATE UNIQUE INDEX IF NOT EXISTS one_active_run ON runs ((1)) WHERE status IN ('queued','running');
            CREATE TABLE IF NOT EXISTS settings (id INTEGER PRIMARY KEY CHECK(id=1), enabled INTEGER NOT NULL, minutes INTEGER NOT NULL);
            INSERT OR IGNORE INTO settings VALUES(1,0,5);
            """;
        command.ExecuteNonQuery();
    }
    public string? TryCreate(RunRequest request)
    {
        using var connection = Open();
        using var transaction = connection.BeginTransaction();
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "SELECT count(*) FROM runs WHERE status IN ('queued','running')";
        if ((long)command.ExecuteScalar()! > 0) return null;
        var id = Guid.NewGuid().ToString();
        command.CommandText = "INSERT INTO runs(id,source,status,stage,created,scenario) VALUES($id,$source,'queued','queued',$created,$scenario)";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$source", request.Source);
        command.Parameters.AddWithValue("$created", DateTimeOffset.UtcNow.ToString("O"));
        command.Parameters.AddWithValue("$scenario", request.QualityScenario);
        command.ExecuteNonQuery();
        transaction.Commit();
        return id;
    }
    public bool TryStart(string id)
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "UPDATE runs SET status='running',stage='extract-transform-export',started=$now WHERE id=$id AND status='queued'";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue("$now", DateTimeOffset.UtcNow.ToString("O"));
        return cmd.ExecuteNonQuery() == 1;
    }
    public void Complete(string id, string? report, string? error, bool interrupted = false)
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "UPDATE runs SET status=$status,stage=$stage,completed=$now,error=$error,report=$report WHERE id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue("$status", interrupted ? "interrupted" : error is null ? "succeeded" : "failed");
        cmd.Parameters.AddWithValue("$stage", error is null ? "complete" : "extract-transform-export");
        cmd.Parameters.AddWithValue("$now", DateTimeOffset.UtcNow.ToString("O"));
        cmd.Parameters.AddWithValue("$error", (object?)error ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$report", (object?)report ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }
    public void InterruptInFlight()
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "UPDATE runs SET status='interrupted',completed=$now,error='Application arrêtée avant la fin du traitement. Relance explicite possible.' WHERE status IN ('queued','running')";
        cmd.Parameters.AddWithValue("$now", DateTimeOffset.UtcNow.ToString("O"));
        cmd.ExecuteNonQuery();
    }
    public IReadOnlyList<RunRecord> List()
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT id,source,status,stage,created,started,completed,error,scenario FROM runs ORDER BY created DESC LIMIT 100";
        using var reader = cmd.ExecuteReader();
        var result = new List<RunRecord>();
        while (reader.Read()) result.Add(Read(reader));
        return result;
    }
    public RunRecord? Get(string id)
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT id,source,status,stage,created,started,completed,error,scenario FROM runs WHERE id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? Read(reader) : null;
    }
    private static RunRecord Read(SqliteDataReader r) => new(r.GetString(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetString(4), r.IsDBNull(5) ? null : r.GetString(5), r.IsDBNull(6) ? null : r.GetString(6), r.IsDBNull(7) ? null : r.GetString(7), r.GetString(8));
    public string? LatestReport()
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT report FROM runs WHERE status='succeeded' ORDER BY completed DESC LIMIT 1";
        return cmd.ExecuteScalar() as string;
    }
    public ScheduleSettings ReadSchedule()
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT enabled,minutes FROM settings WHERE id=1";
        using var r = cmd.ExecuteReader();
        return r.Read() ? new(r.GetInt32(0) == 1, r.GetInt32(1)) : new();
    }
    public void WriteSchedule(ScheduleSettings settings)
    {
        using var c = Open(); using var cmd = c.CreateCommand();
        cmd.CommandText = "UPDATE settings SET enabled=$enabled,minutes=$minutes WHERE id=1";
        cmd.Parameters.AddWithValue("$enabled", settings.Enabled ? 1 : 0);
        cmd.Parameters.AddWithValue("$minutes", settings.IntervalMinutes);
        cmd.ExecuteNonQuery();
    }
}
