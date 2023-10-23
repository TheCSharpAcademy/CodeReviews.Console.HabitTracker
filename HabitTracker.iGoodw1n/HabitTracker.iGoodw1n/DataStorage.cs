using Microsoft.Data.Sqlite;

namespace HabitTracker.iGoodw1n;

public class DataStorage
{
    private readonly string _dbPath;
    public DataStorage(string dbPath)
    {
        _dbPath = dbPath;
        using var connection = new SqliteConnection(_dbPath);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"CREATE TABLE IF NOT EXISTS code_tracker (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    language TEXT NOT NULL,
                    number_of_lines INTEGER NOT NULL,
                    date INTEGER NOT NULL);";

        command.ExecuteNonQuery();
    }

    public List<WrittenCode> GetAllRecords()
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT * FROM code_tracker;";

        var reader = command.ExecuteReader();

        var records = new List<WrittenCode>();
        while (reader.Read())
        {
            records.Add(new WrittenCode
            {
                Id = reader.GetInt32(0),
                Language = reader.GetString(1),
                Lines = reader.GetInt32(2),
                Date = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(3)).UtcDateTime
            });
        }

        return records;
    }

    public void CreateRecord(WrittenCode record)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @$"INSERT INTO code_tracker (language, number_of_lines, date)
                  VALUES ('{record.Language}', {record.Lines}, {(int)record.Date.Subtract(DateTime.UnixEpoch).TotalSeconds});";

        command.ExecuteNonQuery();
    }

    public WrittenCode? GetRecord(int id)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @$"SELECT EXISTS (SELECT * FROM code_tracker WHERE id = {id});";
        var check = Convert.ToBoolean(command.ExecuteScalar());

        if (!check) return null;

        command.CommandText =
            @$"SELECT * FROM code_tracker WHERE id = {id};";

        var reader = command.ExecuteReader();

        reader.Read();

        var record = new WrittenCode
        {
            Id = reader.GetInt32(0),
            Language = reader.GetString(1),
            Lines = reader.GetInt32(2),
            Date = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(3)).UtcDateTime
        };

        return record;
    }

    public void DeleteRecord(int id)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @$"DELETE FROM code_tracker WHERE id = {id};";

        command.ExecuteNonQuery();
    }

    public void UpdateRecord(WrittenCode record)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @$"UPDATE code_tracker
                    SET language = '{record.Language}',
                        number_of_lines = {record.Lines}
                    WHERE id = {record.Id};";

        command.ExecuteNonQuery();
    }
}
