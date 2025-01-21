using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace habit_logger.Data
{
    public static class Database
    {
        private const string ConnectionString = "Data Source=habit-logger.db";

        public static SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static void InitializeDatabase()
        {
            if (File.Exists("habit-logger.db"))
            {
                File.Delete("habit-logger.db");
                Console.WriteLine("Existing database deleted for testing.");
            }

            using (var connection = GetConnection())
            {
                var command = connection.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Unit TEXT NOT NULL 
                    );";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit_records (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        HabitId INTEGER NOT NULL,
                        Date TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        FOREIGN KEY (HabitId) REFERENCES habits(Id) ON DELETE CASCADE
                    );";
                command.ExecuteNonQuery();

                SeedData(connection);
            }
        }

        private static void SeedData(SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            // Insert default habits
            string[] habitNames = { "Drink Water", "Exercise", "Read", "Meditate", "Code" };
            string[] units = { "ml", "minutes", "pages", "minutes", "hours" };

            for (int i = 0; i < habitNames.Length; i++)
            {
                command.CommandText =
                    "INSERT INTO habits (Name, Unit) VALUES (@name, @unit);";
                command.Parameters.AddWithValue("@name", habitNames[i]);
                command.Parameters.AddWithValue("@unit", units[i]);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int habitId = random.Next(1, habitNames.Length + 1);
                string date = DateTime.Now.AddDays(-random.Next(0, 30)).ToString("yyyy-MM-dd");
                int quantity = random.Next(1, 101);

                command.CommandText =
                    "INSERT INTO habit_records (HabitId, Date, Quantity) VALUES (@habitId, @date, @quantity);";
                command.Parameters.AddWithValue("@habitId", habitId);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            Console.WriteLine("Database seeded with sample data.");
        }
    }
}
