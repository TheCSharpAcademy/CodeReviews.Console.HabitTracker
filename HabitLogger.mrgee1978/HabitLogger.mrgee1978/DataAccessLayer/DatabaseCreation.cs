using Microsoft.Data.Sqlite;

namespace HabitLogger.mrgee1978.DataAccessLayer;

public static class DatabaseCreation
{
    public static string DatabaseConnectionString { get; } = @"Data Source=habit_logger.db";

    /// <summary>
    /// Creates the database and the tables if they don't already exist
    /// </summary>
    public static void CreateDatabase()
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseConnectionString))
            {
                connection.Open();
                try
                {
                    using (SqliteCommand tableCommand = connection.CreateCommand())
                    {
                        tableCommand.CommandText =
                            @"CREATE TABLE IF NOT EXISTS habits (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Name TEXT,
                                UnitOfMeasurement TEXT)";

                        tableCommand.ExecuteNonQuery();

                        tableCommand.CommandText =
                            @"CREATE TABLE IF NOT EXISTS records (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER,
                                HabitId INTEGER,
                                FOREIGN KEY(habitId) REFERENCES habits(Id) ON DELETE CASCADE)";

                        tableCommand.ExecuteNonQuery();
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }
}
