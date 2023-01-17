using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary;


public class HabitLoggerConnection : IDisposable
{
    private static string DatabaseFilePath { get; } = @"Data Source=habitlogger.db";

    private SqliteConnection Connection { get; }

    public HabitLoggerConnection()
    {
        Connection = new(DatabaseFilePath);
        Connection.Open();
        EnsureTablesExist();
    }

    private void EnsureTablesExist()
    {
        string createTable =
                @"CREATE TABLE IF NOT EXISTS DRINK_WATER (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
                )";

        ExecuteNonQuery(createTable);
    }

    // Returns it was successfully created.
    public bool CreateLog(string date, int cupsOfWater)
    {
        string createLog =
            $@"INSERT INTO DRINK_WATER (Date, Quantity)
            VALUES ('{date}', {cupsOfWater})";

        return ExecuteNonQuery(createLog);
    }


    // Returns a list of tuples of the records in the database
    // Within the tuple, Item1 = id, Item2 = date, Item3 = quantity
    // If no records exist in the database, an empty list is returned.
    public List<Tuple<int, string, int>> GetAllLogs()
    {
        string getAllLogs =
            $@" SELECT * FROM DRINK_WATER";

        var command = new SqliteCommand(getAllLogs, Connection);
        var reader = command.ExecuteReader();

        var logs = new List<Tuple<int, string, int>>();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string date = reader.GetString(1);
            int quantity = reader.GetInt32(2);
            logs.Add(Tuple.Create(id, date, quantity));
        }
        return logs;
    }

    // Returns whether the record was successfully deleted.
    public bool DeleteLog(int id)
    {
        string deleteLog =
            $@"DELETE FROM DRINK_WATER WHERE id = {id}";
        return ExecuteNonQuery(deleteLog);
    }

    // Returns whether the record was successfully updated.
    public bool UpdateLog(int id, int cupsOfWater)
    {
        string updateLog =
            $@"UPDATE DRINK_WATER
            SET Quantity = {cupsOfWater}
            WHERE id = {id}";
        return ExecuteNonQuery(updateLog);
    }

    private bool ExecuteNonQuery(string updateLog)
    {
        var command = new SqliteCommand(updateLog, Connection);
        return command.ExecuteNonQuery() != 0;
    }

    public void Dispose() => Connection.Dispose();
}

