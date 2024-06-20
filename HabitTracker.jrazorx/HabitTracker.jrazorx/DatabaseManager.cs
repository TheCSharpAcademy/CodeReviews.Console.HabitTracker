using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

public class DatabaseManager
{
    private const string ConnectionString = "Data Source=habit_tracker.db";

    public DatabaseManager()
    {
        CreateDatabase();
    }

    private void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Quantity INTEGER NOT NULL,
                    Date TEXT NOT NULL
                )";
            command.ExecuteNonQuery();
        }
    }

    public void InsertHabit(Habit habit)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Habits (Quantity, Date)
                VALUES ($quantity, $date)";
            command.Parameters.AddWithValue("$quantity", habit.Quantity);
            command.Parameters.AddWithValue("$date", habit.Date.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();
        }
    }

    public List<Habit> GetHabits()
    {
        var habits = new List<Habit>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Quantity, Date FROM Habits";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var habit = new Habit
                    {
                        Id = reader.GetInt32(0),
                        Quantity = reader.GetInt32(1),
                        Date = DateTime.Parse(reader.GetString(2))
                    };
                    habits.Add(habit);
                }
            }
        }
        return habits;
    }

    public void UpdateHabit(Habit habit)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
            UPDATE Habits
            SET Quantity = $quantity, Date = $date
            WHERE Id = $id";
            command.Parameters.AddWithValue("$id", habit.Id);
            command.Parameters.AddWithValue("$quantity", habit.Quantity);
            command.Parameters.AddWithValue("$date", habit.Date.ToString("yyyy-MM-dd"));
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new Exception("Habit ID does not exist.");
            }
        }
    }


    public void DeleteHabit(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Habits WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new Exception("Habit ID does not exist.");
            }
        }
    }

}
