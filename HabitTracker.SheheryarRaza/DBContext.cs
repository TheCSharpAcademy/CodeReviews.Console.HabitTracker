

using Microsoft.Data.Sqlite;


namespace HabitTracker.SheheryarRaza
{
    public class DBContext
    {
        private static string dbFileName = "HabitTracker.db";
        private static string ConnectionString => $"Data Source={dbFileName};";

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();


                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Habits (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Quantity INTEGER NOT NULL,
                            Unit TEXT NOT NULL, -- New column for unit of measurement
                            CreatedAt DATETIME NOT NULL
                        );
                    ";
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database initialized successfully.");

                    // Check if the Habits table is empty and seed data if it is
                    if (!HasData(connection))
                    {
                        Console.WriteLine("Database is empty. Seeding data...");
                        SeedData(connection);
                        Console.WriteLine("Data seeding complete.");
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
                }
            }
        }


        private static bool HasData(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Habits;";
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }


        private static void SeedData(SqliteConnection connection)
        {
            var random = new Random();
            var habitsToSeed = new List<(string Name, string Unit)>
            {
                ("Water Intake", "glasses"),
                ("Running", "km"),
                ("Reading", "pages"),
                ("Meditation", "minutes")
            };

            foreach (var habit in habitsToSeed)
            {

                for (int i = 0; i < 100; i++)
                {
                    var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"
                        INSERT INTO Habits (Name, Quantity, Unit, CreatedAt)
                        VALUES (@name, @quantity, @unit, @createdAt);
                    ";


                    int quantity = 0;
                    switch (habit.Name)
                    {
                        case "Water Intake": quantity = random.Next(1, 11); break; // 1-10 glasses
                        case "Running": quantity = random.Next(1, 16); break;    // 1-15 km
                        case "Reading": quantity = random.Next(10, 101); break;  // 10-100 pages
                        case "Meditation": quantity = random.Next(5, 61); break; // 5-60 minutes
                        default: quantity = random.Next(1, 10); break;
                    }


                    DateTime createdAt = DateTime.Now.AddDays(-random.Next(1, 366)).AddHours(random.Next(1, 24)).AddMinutes(random.Next(1, 60));

                    insertCommand.Parameters.AddWithValue("@name", habit.Name);
                    insertCommand.Parameters.AddWithValue("@quantity", quantity);
                    insertCommand.Parameters.AddWithValue("@unit", habit.Unit);
                    insertCommand.Parameters.AddWithValue("@createdAt", createdAt);

                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}