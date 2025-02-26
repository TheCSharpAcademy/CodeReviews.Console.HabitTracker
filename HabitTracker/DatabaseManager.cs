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

        // Checks for existence of database/table in local file system and creates it if it isn't present
        public void EnsureDatabaseExists()
        {
            try
            {
                bool dbExists = File.Exists(dbPath);

                using SqliteConnection connection = new(connectionString);
                connection.Open();

                if (!dbExists)
                {
                    Console.WriteLine("Database not found. Creating a new one...");

                    // Opting for 'completed' column instead of 'number_of_times_done' because
                    // it makes more sense from a UX standpoint imo
                    var createTable = @"
                    CREATE TABLE IF NOT EXISTS habits (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT NOT NULL,
                        completed INTEGER DEFAULT 0,
                        created_at TEXT DEFAULT CURRENT_TIMESTAMP
                    );";

                    using (SqliteCommand command = new(createTable, connection))
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

        // Adds a habit to the table
        public void AddHabit(string title)
        {
            try
            {
                using SqliteConnection connection = new(connectionString);
                connection.Open();

                using SqliteCommand command = new("INSERT INTO habits (title) VALUES (@title);", connection);
                command.Parameters.AddWithValue("@title", title);
                command.ExecuteNonQuery();

                Console.WriteLine("Habit added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to database: {ex.Message}");
            }
        }
    }
}
