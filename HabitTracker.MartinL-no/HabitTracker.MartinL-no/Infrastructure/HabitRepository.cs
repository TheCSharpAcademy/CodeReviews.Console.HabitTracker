using Microsoft.Data.Sqlite;
using HabitTracker.MartinL_no.Models;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;

namespace HabitTracker.MartinL_no;

internal class HabitRepository
{
    private readonly string _connectionString;

    internal HabitRepository()
    {
        _connectionString = "Data Source=habitTracker.db";
        CreateTable();
    }

    public void CreateTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS Habit (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS HabitDate (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Date STRING NOT NULL UNIQUE,
                    Count INTEGER NOT NULL,
                    HabitId INTEGER NOT NULL,
                    FOREIGN KEY (HabitId)
                        REFERENCES Habit(Id)
                        ON DELETE CASCADE
                );
                """;
            command.ExecuteNonQuery();
        }
    }

    internal Habit GetHabit()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                SELECT H.Id, H.Name, HabitDate.Id as HabitDateId, HabitDate.Date, HabitDate.Count
                FROM Habit AS H
                LEFT JOIN HabitDate ON H.Id = HabitDate.HabitId;
                """;

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows) throw new InvalidOperationException();

                var dates = new List<HabitDate>();
                var name = "";
                var habitId = 0;

                while (reader.Read())
                {
                    habitId = reader.GetInt32(0);
                    name = reader.GetString(1);

                    try
                    {
                        var habitDateId = reader.GetInt32(2);
                        var date = DateOnly.Parse(reader.GetString(3));
                        var count = reader.GetInt32(4);
                        dates.Add(new HabitDate(habitDateId, date, count, habitId));
                    }
                    catch
                    {
                        continue;
                    }
                }

                return new Habit(habitId, name, dates);
            }
        }
    }

    internal void AddHabit(string name)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = """
                INSERT INTO Habit (Name)
                VALUES (@name)
                """;

            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
        }
    }

    internal void DeleteHabit()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM Habit;";
            command.ExecuteNonQuery();

            command.CommandText = "DELETE FROM HabitDate;";
            command.ExecuteNonQuery();
        }
    }

    internal void AddHabitRecord(HabitDate habitDate)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = """
                INSERT OR REPLACE INTO HabitDate (Date, Count, HabitId)
                VALUES (@date, @count, @habitId);
                """;

            command.Parameters.AddWithValue("@date", habitDate.Date);
            command.Parameters.AddWithValue("@count", habitDate.Count);
            command.Parameters.AddWithValue("@habitId", habitDate.HabitId);

            command.ExecuteNonQuery();
        }
    }
}

