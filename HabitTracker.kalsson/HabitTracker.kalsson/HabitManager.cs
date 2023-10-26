using System.Data.SQLite;

namespace HabitTracker.kalsson;

public class HabitManager
{
    private static readonly string ConnectionString = "Data Source=HabitTracker.db;Version=3;";
    
    /// <summary>
    /// Insert a new habit into the 'habits' table.
    /// </summary>
    /// <param name="name">The name of the habit.</param>
    /// <param name="quantity">The quantity associated with the habit.</param>
    /// <param name="unit">The unit of measurement for the quantity.</param>
    public static void InsertHabit(string name, int quantity, string unit)
    {
        try
            {
            using (var connection = new SQLiteConnection(ConnectionString))
                {
                // SQL query to insert a new habit
                string sql = "INSERT INTO habits (name, quantity, unit) VALUES (@name, @quantity, @unit);";
                using (var command = new SQLiteCommand(sql, connection))
                    {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@unit", unit);
                
                    connection.Open();
                    command.ExecuteNonQuery();
                    }
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"An error occurred while inserting a habit: {ex.Message}");
            }
    }

    /// <summary>
    /// Deletes a habit from the database.
    /// </summary>
    /// <param name="habit">The habit to delete.</param>
    public static void DeleteHabit(string habit)
    {
        try
            {
            using (var connection = new SQLiteConnection(ConnectionString))
                {
                string sql = "DELETE FROM habits WHERE name=@name;";
                using (var command = new SQLiteCommand(sql, connection))
                    {
                    command.Parameters.AddWithValue("@name", habit);
                    connection.Open();
                    command.ExecuteNonQuery();
                    }
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"An error occurred while trying delete a habit: {ex.Message}");
            }
    }

    /// <summary>
    /// Updates a habit in the database.
    /// </summary>
    /// <param name="oldHabit">The old habit name.</param>
    /// <param name="newHabit">The new habit name.</param>
    public static void UpdateHabit(string oldHabit, string newHabit)
    {
        try
            {
            using (var connection = new SQLiteConnection(ConnectionString))
                {
                string sql = "UPDATE habits SET name=@newName WHERE name=@oldName;";
                using (var command = new SQLiteCommand(sql, connection))
                    {
                    command.Parameters.AddWithValue("@oldName", oldHabit);
                    command.Parameters.AddWithValue("@newName", newHabit);
                    connection.Open();
                    command.ExecuteNonQuery();
                    }
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"An error occurred while updating a habit: {ex.Message}");
            }
    }

    /// <summary>
    /// Reads all habits from the database.
    /// </summary>
    /// <returns>A list of formatted string representing habits.</returns>
    public static List<string>? GetAllHabits()
    {
        try
            {
            List<string> habits = new List<string>();
            using (var connection = new SQLiteConnection(ConnectionString))
                {
                string sql = "SELECT * FROM habits;";
                using (var command = new SQLiteCommand(sql, connection))
                    {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                        {
                        while (reader.Read())
                            {
                            string name = reader["name"].ToString();
                            int quantity = int.Parse(reader["quantity"].ToString());
                            string unit = reader["unit"].ToString();
                        
                            habits.Add($"{name} - {quantity} {unit}");
                            }
                        }
                    }
                }
            return habits;
            }
        catch (Exception ex)
            {
            Console.WriteLine($"An error occurred while trying to fetch all habits: {ex.Message}");
            }
        return null;
    }
    
    /// <summary>
    /// Checks if a habit with the given name exists in the database.
    /// </summary>
    /// <param name="habitName">The name of the habit to check.</param>
    /// <returns>True if the habit exists, false otherwise.</returns>
    public static bool DoesHabitExist(string habitName)
    {
        try
            {
            using (var connection = new SQLiteConnection(ConnectionString))
                {
                string sql = "SELECT COUNT(*) FROM habits WHERE name = @name;";
                using (var command = new SQLiteCommand(sql, connection))
                    {
                    command.Parameters.AddWithValue("@name", habitName);
                    connection.Open();
                    
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    
                    connection.Close();
                    
                    return count > 0;
                    }
                }
            }
        catch (Exception ex)
            {
            Console.WriteLine($"An error occurred while trying to check if a habit exists: {ex.Message}");
            }

        return false;
    }
}