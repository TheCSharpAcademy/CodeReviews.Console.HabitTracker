using Microsoft.Data.Sqlite;
namespace HabitTrackerLibrary;

public static class HabitDatabase
{
    private static string _connectionString = "DataSource=HabitTracker.db";
    public static Dictionary<string, Habit.Amount?> HabitTypes = new Dictionary<string, Habit.Amount?>();
    public static void CreateDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string createTable = @"
            CREATE TABLE IF NOT EXISTS Habits (
            HabitType TEXT NOT NULL,
            LoggingDate DATE NOT NULL,
            Quantity TEXT NULL)";
            using (var cmd = new SqliteCommand(createTable, connection))
            {
                cmd.ExecuteNonQuery();  // This will create the 'Users' table if it doesn't already exist
            }
            connection.Close();
            UpdateHabitTypes();
           
        }
        
    }

    public static void UpdateHabitTypes()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string getHabitTypes = @"SELECT DISTINCT HabitType , Quantity FROM Habits";
            using (var cmd = new SqliteCommand(getHabitTypes, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!HabitTypes.ContainsKey(reader.GetString(0)))
                        {
                            HabitTypes.Add(reader["HabitType"].ToString(),
                                Habit.Amount.ParseAmount(reader["Quantity"].ToString()));
                        }
                    }

                    Console.ReadLine();
                }
            }

            connection.Close();
        }
    }

    public static void DisplayHabitsHistory()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new SqliteCommand(@"SELECT HabitType, LoggingDate, Quantity FROM habits", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Habits:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["HabitType"]}, {reader["LoggingDate"]}, {reader["Quantity"]}");
                    }
                    Console.WriteLine("press enter to continue...");
                    Console.ReadLine();
                }
            }

            connection.Close();
        }
    }
    public static void AddHabit(Habit habit)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string insertCommand = @"INSERT INTO Habits (HabitType,LoggingDate,Quantity) VALUES (@type, @date, @quantity);";
            using (var cmd = new SqliteCommand(insertCommand, connection))
            {
                cmd.Parameters.AddWithValue("@type", habit.Name);
                cmd.Parameters.AddWithValue("@date", habit.GetDate());
                //check if value is null and return suitable value
                if (habit.GetAmount() == null)
                {
                    cmd.Parameters.Add("@quantity", SqliteType.Text).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@quantity", SqliteType.Text).Value = habit.GetAmount();
                }
                
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            connection.Close();
        }
    }
    
}