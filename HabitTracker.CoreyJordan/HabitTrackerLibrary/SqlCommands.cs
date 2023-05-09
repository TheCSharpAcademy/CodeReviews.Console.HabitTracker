using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTrackerLibrary;
public static class SqlCommands
{
    public static int DeleteRecord(int entryId, string tableName)
    {
        int rowCount;
        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"DELETE FROM {tableName} WHERE Id = {entryId}";
            rowCount = cmd.ExecuteNonQuery();
        }
        return rowCount;
    }

    public static List<Habit> GetAllRecords(string tableName)
    {
        List<Habit> habits = new();

        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"SELECT * FROM {tableName}";
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                habits.Add(new Habit
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yy", new CultureInfo("en-US")),
                    Quantity = reader.GetInt32(2)
                });
            }
        }
        return habits;
    }

    public static List<string> GetTables()
    {
        List<string> tables = new();
        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }
        }
        return tables;
    }

    public static void InitializeDB(string connString, string tableName)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {tableName}
                (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER)";

            cmd.ExecuteNonQuery();
        }
    }

    public static void InsertRecord(string date, int quantity, string tableName)
    {
        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"INSERT INTO {tableName}(date, quantity) VALUES('{date}', {quantity})";
            cmd.ExecuteNonQuery();
        }
    }

    public static bool RecordExists(int entryId, string tableName)
    {
        bool recordExists = true;
        int checkQuery;
        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE Id = {entryId})";
            checkQuery = Convert.ToInt32(cmd.ExecuteScalar());
        }

        if (checkQuery == 0)
        {
            recordExists = false;
        }

        return recordExists;
    }

    public static void UpdateRecord(Habit habit)
    {
        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"UPDATE {habit.HabitName} SET date = '{habit.Date:MM-dd-yy}', quantity = {habit.Quantity} WHERE Id = {habit.Id}";
            cmd.ExecuteNonQuery();
        }
    }
}
