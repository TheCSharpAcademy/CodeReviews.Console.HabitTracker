using System.Data.SQLite;

namespace Buutyful.HabitTracker;



public class DataAccess(string databasePath)
{
    private readonly string connectionString = $"Data Source={databasePath};Version=3;";

    public void CreateDatabase()
    {
        using SQLiteConnection connection = new(connectionString);
        connection.Open();

        
        string createTableQuery = "CREATE TABLE IF NOT EXISTS Habits (Id INTEGER PRIMARY KEY, Name TEXT, CreatedAt TIMESTAMP)";
        using (SQLiteCommand command = new(createTableQuery, connection))
        {
            command.ExecuteNonQuery();
        }

        connection.Close();
    }
    public void InsertHabit(string habitName)
    {
        using SQLiteConnection connection = new(connectionString);
        connection.Open();

        var createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        string insertQuery = $"INSERT INTO Habits (Name, CreatedAt) VALUES ('{habitName}', '{createdAt}')";
        using (SQLiteCommand command = new(insertQuery, connection))
        {
            command.ExecuteNonQuery();
        }

        connection.Close();
    }
    public void DisplayHabits()
    {
        using SQLiteConnection connection = new(connectionString);
        connection.Open();
        
        string selectQuery = "SELECT * FROM Habits";
        using (SQLiteCommand command = new(selectQuery, connection))
        using (SQLiteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}, CreatedAt: {reader["CreatedAt"]}");
            }
        }

        connection.Close();
    }
    public void DeleteHabit(int habitId)
    {
        using SQLiteConnection connection = new(connectionString);
        connection.Open();

        string deleteQuery = $"DELETE FROM Habits WHERE Id = {habitId}";
        using (SQLiteCommand command = new(deleteQuery, connection))
        {
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine($"No habit with Id {habitId} found");
            }
            else
            {
                Console.WriteLine($"Habit with Id {habitId} deleted successfully.");
            }
        }

        connection.Close();
    }
    public void UpdateHabit(int habitId, string newName)
    {
        using SQLiteConnection connection = new(connectionString);
        connection.Open();

        string updateQuery = $"UPDATE Habits SET Name = '{newName}' WHERE Id = {habitId}";
        using (SQLiteCommand command = new(updateQuery, connection))
        {
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                Console.WriteLine($"No habit with Id {habitId} found");
            }
            else
            {
                Console.WriteLine($"Habit with Id {habitId} updated successfully.");
            }
        }

        connection.Close();
    }

}
