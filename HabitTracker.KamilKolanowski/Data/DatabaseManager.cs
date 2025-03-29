using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker.KamilKolanowski.Data
{
   public class DatabaseManager
    {
        public void CreateDatabaseIfNotExists()
        {
            string databasePath = "dbapp.db";
            if (!File.Exists(databasePath))
            {
                using (var connection = OpenConnection())
                {
                    CreateHabitLoggerTable(connection);
                    Console.WriteLine("Database and table created successfully.");
                }
            }
            else
            {
                Console.WriteLine("Database already exists.");
            }
        }

        public SqliteConnection OpenConnection()
        {
            var connection = new SqliteConnection("Data Source=dbapp.db");
            connection.Open();
            return connection;
        }

        public void CreateHabitLoggerTable(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Habits(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Habit TEXT NOT NULL,
                    Quantity REAL NOT NULL,
                    UnitOfMeasure TEXT NOT NULL,
                    Timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                )";

            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table 'Habits' created successfully.");
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SQLite Error: {ex.Message}");
            }
        }

        public Tuple<int, string, double, string, string>[] ListHabits(SqliteConnection connection)
        {
            var command = new SqliteCommand("SELECT Id, ROW_NUMBER() OVER(ORDER BY Id) AS app_id,  Habit, Quantity, UnitOfMeasure, Timestamp FROM Habits", connection);
            var reader = command.ExecuteReader();

            var habits = new List<Tuple<int, string, double, string, string>>();
            while (reader.Read())
            {
                int id = reader.GetInt32(1);
                string habit = reader.GetString(2);
                double quantity = reader.GetDouble(3);
                string unitOfMeasure = reader.GetString(4);
                string timestamp = reader.GetString(5);
                
                habits.Add(new Tuple<int, string, double, string, string>(id, habit, quantity, unitOfMeasure, timestamp));
            }
            return habits.ToArray();
        }

        public void AddHabit(SqliteConnection connection)
        {
            Console.Write("Add Habit: ");
            var habit = Console.ReadLine();

            Console.Write("Add Quantity: ");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double quantity)) // Adding CultureInfo, so user can add either in format 123.456 as well as 123,456.
            {
                Console.WriteLine("Invalid quantity!");
                return;
            }

            Console.Write("Add Unit of Measure: ");
            var uom = Console.ReadLine();

            string? userTimestamp = null;
            Console.WriteLine("Do you want to add timestamp to Habit? Y/N");
            if (Console.ReadLine().ToLower() == "y")
            {
                Console.Write("Timestamp [yyyy-MM-dd HH:mm:ss]: ");
                userTimestamp = Console.ReadLine();
            }
            
            if (string.IsNullOrWhiteSpace(userTimestamp))
            {
                userTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            using var command = connection.CreateCommand();
            command.CommandText = @"
                                INSERT INTO Habits (Habit, Quantity, UnitOfMeasure, Timestamp)
                                VALUES (@habit, @quantity, @uom, @timestamp)";

            command.Parameters.AddWithValue("@habit", habit);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@uom", uom);
            command.Parameters.AddWithValue("@timestamp", userTimestamp);

            command.ExecuteNonQuery();
            Console.WriteLine("\nHabit added successfully.\nPress any key to go back.");
            Console.ReadKey();
        }


        public void DeleteHabit(SqliteConnection connection)
        {
            int id;
            int[] habits = ListHabits(connection).Select(x => x.Item1).ToArray();
            while (true)
            {
                Console.Write("Specify the id of the Habit to delete: ");
                if (int.TryParse(Console.ReadLine(), out id) && habits.Contains(id))
                {
                    break;
                }
                Console.WriteLine("Invalid Id, try again.");
            }
            

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Habits WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
            Console.WriteLine("\nHabit deleted successfully.\nPress any key to go back.");
            Console.ReadKey();
        }

        public void UpdateHabit(SqliteConnection connection)
        {
            int id;
            int[] habits = ListHabits(connection).Select(x => x.Item1).ToArray();
            
            while (true)
            {
                Console.Write("Specify the id of the Habit to update: ");
                if (int.TryParse(Console.ReadLine(), out id) && habits.Contains(id))
                {
                    break;
                }
                Console.WriteLine("Invalid Id, try again.");
            }

            string column;
            while (true)
            {
                Console.Write("What would you like to update [Habit (h), Quantity (q), Unit of Measure (u)]: ");
                var choice = Console.ReadLine()?.ToLower();

                column = choice switch
                {
                    "h" => "Habit",
                    "q" => "Quantity",
                    "u" => "UnitOfMeasure",
                    _ => null
                };

                if (column != null)
                    break;
                Console.WriteLine("Invalid choice. Try again.");
            }

            Console.Write($"Enter new value for {column}: ");
            var newValue = Console.ReadLine();

            var command = connection.CreateCommand();
            command.CommandText = $"UPDATE Habits SET {column} = @newValue WHERE Id = @id";
            command.Parameters.AddWithValue("@newValue", newValue);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
            Console.WriteLine("\nHabit updated successfully.\nPress any key to go back.");
            Console.ReadKey();
        }

        public Tuple<string, double, string>[] CreateReport(SqliteConnection connection)
        {
            var command = new SqliteCommand("SELECT Habit, SUM(Quantity), UnitOfMeasure FROM Habits GROUP BY  Habit, UnitOfMeasure", connection);
            var reader = command.ExecuteReader(); 

            var habitsReport = new List<Tuple<string, double, string>>();
            while (reader.Read())
            {
                string habit = reader.GetString(0);
                double quantity = reader.GetDouble(1);
                string unitOfMeasure = reader.GetString(2);
                
                habitsReport.Add(new Tuple<string, double, string>(habit, quantity, unitOfMeasure));
            }
            return habitsReport.ToArray();
            
        }
        
        public void CloseConnection(SqliteConnection connection)
        {
            connection.Close();
        }
    }
}

