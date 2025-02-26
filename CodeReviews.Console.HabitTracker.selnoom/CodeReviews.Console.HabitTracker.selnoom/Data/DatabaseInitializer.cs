using Microsoft.Data.Sqlite;

namespace CodeReviews.Console.HabitTracker.selnoom.Data;

internal static class DatabaseInitializer
{
    private const string connectionString = @"Data Source=habit-Tracker.db";

    public static void EnsureDatabaseCreated()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
            )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
