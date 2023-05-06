using Microsoft.Data.Sqlite;

namespace HabitTrackerLibrary;
public static class SqlCommands
{
    public static List<DrinkingWater> GetAllRecords()
    {
        throw new NotImplementedException();
    }

    public static void InitializeDB(string connString)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water
        (Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        Quantity INTEGER)";

            cmd.ExecuteNonQuery();
        }
    }

    public static void InsertRecord(string date, int quantity)
    {
        throw new NotImplementedException();
    }

    public static void UpdateRecord(int recordId)
    {
        throw new NotImplementedException();
    }
}
