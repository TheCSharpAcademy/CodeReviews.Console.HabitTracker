using Habit_Logger.Services;
using Microsoft.Data.Sqlite;

namespace Habit_Logger.Data
{
    internal class Database
    {
        internal static string ConnectionString { get; } = @"Data Source=habbitlogger.db";

        internal static void CreateDatabase()
        {
            using (SqliteConnection connection = new(ConnectionString))
            {
                using (SqliteCommand tableCommand = connection.CreateCommand())
                {
                    connection.Open();

                    tableCommand.CommandText =
                        @"CREATE TABLE IF NOT EXISTS progress (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER,
                        HabitId INTEGER,
                        FOREIGN KEY(habitId) REFERENCES habits(Id) ON DELETE CASCADE
                        )";

                    tableCommand.ExecuteNonQuery();

                    tableCommand.CommandText =
                        @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        MeasurementUnit TEXT
                        )";

                    tableCommand.ExecuteNonQuery();
                }
            }

            // Seed the database with initial data if needed
            Helpers.SeedData();
        }
    }
}
