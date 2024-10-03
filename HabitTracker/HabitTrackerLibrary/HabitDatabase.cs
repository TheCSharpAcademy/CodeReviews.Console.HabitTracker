using Microsoft.Data.Sqlite;

namespace HabitTrackerLibrary;

public static class HabitDatabase
{
    private static readonly string _connectionString = "DataSource=HabitTracker.db";
    public static Dictionary<string, Habit.Amount?> HabitTypes = new();

    public static void CreateDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var createTable = @"
            CREATE TABLE IF NOT EXISTS Habits (
            HabitType TEXT NOT NULL,
            LoggingDate DATE NOT NULL,
            Quantity TEXT NULL)";
            using (var cmd = new SqliteCommand(createTable, connection))
            {
                cmd.ExecuteNonQuery(); // This will create the 'Users' table if it doesn't already exist
            }

            connection.Close();
            UpdateHabitTypes();
        }
    }

    public static void DeleteHabit(string type, string date)
    {
        var deleteCommand = string.IsNullOrWhiteSpace(date)
            ? "DELETE FROM Habits WHERE HabitType = @type"
            : "DELETE FROM Habits WHERE HabitType = @type AND LoggingDate = @date";
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new SqliteCommand(deleteCommand, connection))
            {
                cmd.Parameters.AddWithValue("@type", type);
                if (!string.IsNullOrWhiteSpace(date)) cmd.Parameters.AddWithValue("@date", date);

                cmd.ExecuteNonQuery();
            }
        }

        UpdateHabitTypes();
    }

    public static void UpdateHabitLog(string command, string habitType, string date, string newAmount)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new SqliteCommand(command, connection))
            {
                cmd.Parameters.AddWithValue("@HabitType", habitType);
                cmd.Parameters.AddWithValue("@LoggingDate", date);
                cmd.Parameters.AddWithValue("@Quantity", newAmount);
                try
                {
                    var rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine($"{rowsAffected} habit(s) updated successfully.");
                    else
                        Console.WriteLine("No habit found with the specified type and date.");
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            connection.Close();
        }
    }

    public static void UpdateHabitTypes()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var getHabitTypes = @"SELECT DISTINCT HabitType , Quantity FROM Habits";
            using (var cmd = new SqliteCommand(getHabitTypes, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        if (!HabitTypes.ContainsKey(reader.GetString(0)))
                            HabitTypes.Add(reader["HabitType"].ToString(),
                                Habit.Amount.ParseAmount(reader["Quantity"].ToString()));

                }
            }

            connection.Close();
        }
    }

    public static void DisplayHabitsHistory(string habitType, string date)
    {
        Console.Clear();
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = @"SELECT HabitType, LoggingDate, Quantity FROM habits ";
            if (!string.IsNullOrWhiteSpace(date) && !string.IsNullOrWhiteSpace(habitType))
                command += "WHERE LoggingDate = @date AND HabitType = @type";
            if (string.IsNullOrWhiteSpace(date) && !string.IsNullOrWhiteSpace(habitType))
                command += "WHERE HabitType = @type";
            if (!string.IsNullOrWhiteSpace(date) && string.IsNullOrWhiteSpace(habitType))
                command += "WHERE LoggingDate = @date ";

            using (var cmd = new SqliteCommand(command, connection))
            {
                if (!string.IsNullOrEmpty(habitType)) cmd.Parameters.AddWithValue("@type", habitType);

                if (!string.IsNullOrWhiteSpace(date)) cmd.Parameters.AddWithValue("@date", date);
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Habits:");
                    while (reader.Read())
                        Console.WriteLine(
                            $"{reader["HabitType"]}, {DateTime.Parse(reader["LoggingDate"].ToString()).ToString("dd-MM-yyyy")}, {reader["Quantity"]}");
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
            var insertCommand =
                @"INSERT INTO Habits (HabitType,LoggingDate,Quantity) VALUES (@type, @date, @quantity);";
            using (var cmd = new SqliteCommand(insertCommand, connection))
            {
                cmd.Parameters.AddWithValue("@type", habit.Name);
                cmd.Parameters.AddWithValue("@date", habit.GetDate());
                //check if value is null and return suitable value
                if (habit.GetAmount() == null)
                    cmd.Parameters.Add("@quantity", SqliteType.Text).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@quantity", SqliteType.Text).Value = habit.GetAmount();

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