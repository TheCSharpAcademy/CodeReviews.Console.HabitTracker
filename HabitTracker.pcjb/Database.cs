using Microsoft.Data.Sqlite;

namespace HabitTracker;

class Database
{
    private readonly string databaseFilename;

    public Database(string databaseFilename)
    {
        this.databaseFilename = databaseFilename;
    }

    public bool AddHabitLogRecord(HabitLogRecord record)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            INSERT INTO habitlog
            (date, quantity) 
            VALUES 
            ($date, $quantity)
            ";
            command.Parameters.AddWithValue("$date", record.Date);
            command.Parameters.AddWithValue("$quantity", record.Quantity);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool UpdateHabitLogRecord(HabitLogRecord record)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            UPDATE habitlog
            SET date=$date, quantity=$quantity
            WHERE id=$id
            ";
            command.Parameters.AddWithValue("$id", record.ID);
            command.Parameters.AddWithValue("$date", record.Date);
            command.Parameters.AddWithValue("$quantity", record.Quantity);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public bool DeleteHabitLogRecord(long id)
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            DELETE FROM habitlog
            WHERE id=$id
            ";
            command.Parameters.AddWithValue("$id", id);
            return command.ExecuteNonQuery() == 1;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    public List<HabitLogRecord> GetHabitLogRecords()
    {
        List<HabitLogRecord> habitlog = new();

        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id, date, quantity 
            FROM habitlog
            ORDER BY id ASC
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt64(0);
                var date = DateOnly.FromDateTime(reader.GetDateTime(1));
                var quantity = reader.GetInt32(2);
                habitlog.Add(new HabitLogRecord(id, date, quantity));
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return habitlog;
    }

    public bool CreateDatabaseIfNotPresent()
    {
        try
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS habitlog (
                id INTEGER PRIMARY KEY,
                date TEXT NOT NULL,
                quantity INTEGER NOT NULL
            )
            ";
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    private string GetConnectionString()
    {
        return String.Format("Data Source={0}", databaseFilename);
    }
}