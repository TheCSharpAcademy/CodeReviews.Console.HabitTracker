using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    internal class DatabaseManager
    {
        private readonly string _dbPath = "habits.db";
        private readonly string _connectionString;

        internal DatabaseManager()
        {
            _connectionString = $"Data Source={_dbPath}";
        }

        internal void EnsureDatabaseExists()
        {
            try
            {
                bool dbExists = File.Exists(_dbPath);

                using SqliteConnection connection = new(_connectionString);
                connection.Open();

                if (!dbExists)
                {
                    Console.WriteLine("Database not found. Creating a new one...");

                    // Opting for 'completed' column instead of 'number_of_times_done' because
                    // it makes more sense from a UX standpoint imo
                    var createTable = @"
                    CREATE TABLE IF NOT EXISTS habits (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT NOT NULL CHECK(length(title) <= 50),
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

        internal void OutputTable()
        {
            try
            {
                using SqliteConnection connection = new(_connectionString);
                connection.Open();

                using SqliteCommand command = new("SELECT * FROM habits", connection);
                using SqliteDataReader reader = command.ExecuteReader();

                List<Habit> habits = [];

                while (reader.Read())
                {
                    Habit habit = new()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        IsCompleted = reader.GetInt32(2) == 1,
                        DateCreated = reader.GetString(3)
                    };

                    habits.Add(habit);
                }

                Console.WriteLine("{0,-5} {1,-50} {2,-15} {3,-20}\n", "ID", "Habit", "Is Completed", "Date Created");

                foreach (Habit habit in habits)
                {
                    Console.WriteLine(
                        "{0,-5} {1,-5" +
                        "0} {2,-15} {3,-20}", 
                        habit.Id, habit.Title, habit.IsCompleted, habit.DateCreated
                    );
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine($"Error reading database: {ex.Message}");
            }
        }

        internal void AddHabit(string title)
        {
            try
            {
                using SqliteConnection connection = new(_connectionString);
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

        internal void UpdateHabit(string id, string column, string value)
        {
            try
            {
                HashSet<string> allowedColumns = ["title", "completed"];

                if (!allowedColumns.Contains(column))
                {
                    Console.WriteLine("You either cannot modify this column or it does not exist.");
                    return;
                }

                // Handles converting user input to match table syntax if updating 'completed' column
                if (column == "completed")
                {
                    if (value == "true")
                    {
                        value = "1";
                    }
                    else if (value == "false")
                    {
                        value = "0";
                    }
                    else
                    {
                        Console.WriteLine("Value must be either 'true' or 'false'");
                        return;
                    }
                }

                using SqliteConnection connection = new(_connectionString);
                connection.Open();

                using SqliteCommand command = new(
                    $@"
                    UPDATE habits
                    SET {column} = @value
                    WHERE id = @id
                    ", connection
                );

                command.Parameters.AddWithValue("@value", value);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Habit updated successfully!");
                }
                else
                {
                    Console.WriteLine("Something went wrong; please check for typos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating table: {ex.Message}");
            }
        }

        internal void DeleteHabit(string id)
        {
            try
            {
                using SqliteConnection connection = new(_connectionString);
                connection.Open();

                using SqliteCommand command = new("DELETE FROM habits WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Habit deleted successfully!");
                } 
                else
                {
                    Console.WriteLine("Specified ID does not exist within table.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting habit: {ex.Message}");
            }
        }
    }
}
