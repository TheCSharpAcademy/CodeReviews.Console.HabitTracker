using Microsoft.Data.Sqlite;
using Spectre.Console;

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
        VALUES (0, '')
        ";

        command.ExecuteNonQuery();

        command.CommandText =
            @"
            UPDATE habits
            SET habitcount = $habitcount, habitname = $habitname
            WHERE ID = 1
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
                if (!reader.Read())
                {
                    AnsiConsole.Markup("[fuchsia]\nDatabase is empty\n\n[/]");
                }
                else
                {
                    var habitCount = reader.GetString(1);
                    var habitName = reader.GetString(2);
                    AnsiConsole.Markup($"[fuchsia]\n\nFor {habitName} your current Count is - {habitCount}\n\n[/]");
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
                    UPDATE habits
                    SET habitcount = ($habitcount)
                    WHERE ID = 1
                ";
            command.Parameters.AddWithValue("$habitcount", habitCount);

            command.ExecuteNonQuery();
        }
    }
}


