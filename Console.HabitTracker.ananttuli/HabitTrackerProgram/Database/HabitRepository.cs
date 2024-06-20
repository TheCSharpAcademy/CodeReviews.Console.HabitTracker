using Microsoft.Data.Sqlite;

namespace HabitTrackerProgram.Database
{
    public class HabitRepository
    {
        public static string TableName
        {
            get
            {
                return "HabitLogs";
            }
        }

        public static void CreateTable()
        {
            try
            {
                using (SqliteConnection connection = Database.GetConnection())
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText = $@"
                    CREATE TABLE IF NOT EXISTS {TableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Quantity INTEGER NOT NULL,
                        LogTime TEXT NOT NULL
                )";

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                Console.WriteLine("Could not initialise application. Exiting...");
                Environment.Exit(1);
            }
        }

        public static List<Model.Habit> ReadHabitsQuery()
        {
            List<Model.Habit> habits = new();

            try
            {
                using (SqliteConnection connection = Database.GetConnection())
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                        $@"SELECT * FROM {TableName}";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(reader.GetOrdinal("Id"));
                            var quantity = reader.GetDecimal(reader.GetOrdinal("Quantity"));
                            var logTime = reader.GetDateTime(reader.GetOrdinal("LogTime"));

                            habits.Add(new Model.Habit(id, quantity, logTime));
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("\n\tERROR: Could not display habits.");
            }

            return habits;
        }

        public static void CreateHabit(decimal quantity, DateTime logTime)
        {
            try
            {
                using (SqliteConnection connection = Database.GetConnection())
                {
                    connection.Open();

                    SqliteCommand? command = connection.CreateCommand();

                    if (command == null)
                    {
                        Console.WriteLine("Could not create habit.");
                        return;
                    }

                    command.CommandText =
                        $@"INSERT INTO {TableName} (Quantity, LogTime) VALUES ($quantity, $logTime)";

                    command.Parameters.AddWithValue("$quantity", quantity);
                    command.Parameters.AddWithValue("$logTime", logTime);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine(
                        rowsAffected == 0 ? "\nHabit could not be created." : "\nHabit created. Good job!"
                    );
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Could not create habit");
            }
        }

        public static void UpdateHabit(int id, decimal quantity, DateTime logTime)
        {
            try
            {
                using (SqliteConnection connection = Database.GetConnection())
                {
                    connection.Open();

                    SqliteCommand? command = connection.CreateCommand();

                    if (command == null)
                    {
                        throw new Exception("Could not create command");
                    }

                    command.CommandText =
                        $@"UPDATE {TableName} 
                        SET Quantity = $quantity, LogTime = $logTime
                        WHERE Id = $id";

                    command.Parameters.AddWithValue("$id", id);
                    command.Parameters.AddWithValue("$quantity", quantity);
                    command.Parameters.AddWithValue("$logTime", logTime);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine(
                        rowsAffected == 0 ? "\nNo updates made." : "\nUpdated habit."
                    );
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Could not update habit");
            }
        }

        public static void DeleteHabit(int id)
        {
            try
            {
                using (SqliteConnection connection = Database.GetConnection())
                {
                    connection.Open();

                    SqliteCommand? command = connection.CreateCommand();

                    if (command == null)
                    {
                        throw new Exception("Could not create command");
                    }

                    command.CommandText =
                        $@"DELETE FROM {TableName} 
                        WHERE Id = $id";

                    command.Parameters.AddWithValue("$id", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine(
                        rowsAffected == 0 ? "\nHabit could not be deleted." : $"\nDeleted habit ID {id}."
                    );
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Could not delete habit");
            }
        }
    }
}