using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary;


// A wrapper class for the SqliteConnection class which connects
// to the Habit Logger data base.
//
// This class is only fitted for the HabitLogger Application. This will
// cause errors if two instances of this clas are made within the same scope.
// This class is not suited for multi-threaded uses.
//
// This class interacts with the database using raw sql statements which leaves
// it prone to sql injection. While this is a not big deal for this application,
// it is still something to be aware of.
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
                Date DATETIME,
                Quantity INTEGER
                )";

        ExecuteNonQuery(createTable);
    }

    // Creates a new record in the database.
    // Returns it was successfully created.
    public bool CreateLog(int cupsOfWater)
    {
        string createLog =
            $@"INSERT INTO DRINK_WATER (Date, Quantity)
            VALUES (CURRENT_DATE, '{cupsOfWater}')";

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
            string date = reader.GetDateTime(1).ToString().Split(" ")[0];
            int quantity = reader.GetInt32(2);
            logs.Add(Tuple.Create(id, date, quantity));
        }
        return logs;
    }

    // Deletes a record in the database.
    // Returns whether the record was successfully deleted.
    public bool DeleteLog(int id)
    {
        string deleteLog =
            $@"DELETE FROM DRINK_WATER WHERE id = {id}";
        return ExecuteNonQuery(deleteLog);
    }

    // Updates a record in the database.
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

