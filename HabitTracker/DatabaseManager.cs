using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    internal class DatabaseManager
    {
        private readonly string dbPath = "habits.db";
        private readonly string connectionString;

        public DatabaseManager()
        {
            connectionString = $"Data Source={dbPath}";
        }

        public void EnsureDatabaseExists()
        {
            try
            {
                bool dbExists = File.Exists(dbPath);

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                if (!dbExists)
                {
                    Console.WriteLine("Database not found. Creating a new one...");

                    var createTable = @"
                    CREATE TABLE IF NOT EXISTS habits (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT NOT NULL,
                        completed INTEGER DEFAULT 0,
                        created_at TEXT DEFAULT CURRENT_TIMESTAMP
                    );";

                    using (var command = new SqliteCommand(createTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Database created successfully.");
                }
                else
                {
                    Console.WriteLine("Database found. Proceeding with normal operation.");
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error establishing database connection: {ex.Message}");
            }
        }
    }
}
