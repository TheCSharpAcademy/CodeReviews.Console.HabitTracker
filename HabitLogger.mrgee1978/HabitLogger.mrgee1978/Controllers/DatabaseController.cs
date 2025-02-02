using Microsoft.Data.Sqlite;

namespace mrgee1978.HabitLogger.Controllers;

// This class handles tasks related to database creation
public static class DatabaseController
{
    public static string ConnectionString { get; set; } = @"Data Source = habit-logger.db";
    /// <summary>
    /// When running the program will create the database tables
    /// if they do not already exist
    /// </summary>
    public static void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS records (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date Text,
                        Quantity INTEGER,
                        HabitId INTEGER,
                        FOREIGN KEY(habitId) REFERENCES habits(Id) ON DELETE CASCADE
                        )";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description VARCHAR(100) NOT NULL
                        )";
                tableCmd.ExecuteNonQuery();
            }
        }
    }
}
