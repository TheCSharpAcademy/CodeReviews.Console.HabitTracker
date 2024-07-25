using Microsoft.Data.Sqlite;

namespace Habit_Logger_Application;

internal class DatabaseServices
{
    public void CreateDatabaseAndTable()
    {
        var connectionString = "Data Source=habits.db;";

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS habits (
                id INTEGER PRIMARY KEY, 
                habitcount INTEGER,     
                habitname TEXT
            );  
        ";
        command.ExecuteNonQuery();
    }


    public void PostToDatabase(UserHabit habit)
    {
        using var connection = new SqliteConnection("Data Source=habits.db");
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"
                INSERT INTO habits (habitcount, habitname)
                VALUES ($habitcount, $habitname)
            ";
        command.Parameters.AddWithValue("$habitcount", habit.HabitCounter);
        command.Parameters.AddWithValue("$habitname", habit.HabitName);

        command.ExecuteNonQuery();
    }


    public void GetFromDatabase()
    {
        using (var connection = new SqliteConnection("Data Source=habits.db"))
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText =
                @"
                    SELECT * 
                    FROM habits
                ";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var habitName = reader.GetString(2);
                    var habitCount = reader.GetString(1);

                    Console.WriteLine($"\n\nFor {habitName} your current Count is - {habitCount}");
                }
            }
        }
    }


    public void DeleteFromDatabase()
    {
        using (var connection = new SqliteConnection("Data Source=habits.db"))
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText =
                @"
                    DELETE FROM habits
                    WHERE id = 1
                ";
            command.ExecuteNonQuery();
        }
    }


    public void UpdateToDatabase(int habitCount)
    {
        using (var connection = new SqliteConnection("Data Source=habits.db"))
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText =
                @"
                    INSERT INTO habits (habitcount)
                    VALUES ($habitcount)
                ";
            command.Parameters.AddWithValue("$habitcount", habitCount);

            command.ExecuteNonQuery();
        }
    }
}
