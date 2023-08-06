using Microsoft.Data.Sqlite;
using HabitTracker.MartinL_no.Models;

namespace HabitTracker.MartinL_no;

internal class HabitRepository
{
    private readonly string _connectionString;

    internal HabitRepository()
    {
        _connectionString = "Data Source=habitTracker.db";
        CreateTable();
    }

    private void CreateTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS Habit (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS HabitDate (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Date STRING NOT NULL,
                    Count INTEGER NOT NULL,
                    HabitId INTEGER NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES Habit(Id)
                );
                """;
            command.ExecuteNonQuery();
        }
    }

    internal List<Habit> GetAllHabits()
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
                var habits = new List<Habit>();
                var habitDates = new List<HabitDate>();
                var name = "";
                var id = 0;

                while (reader.Read())
                {
                    if (id != 0 && id != reader.GetInt32(0))
                    {
                        habits.Add(new Habit(id, name, habitDates));
                        habitDates = new List<HabitDate>();
                    }

                    id = reader.GetInt32(0);
                    name = reader.GetString(1);

                    try
                    {
                        var habitDateId = reader.GetInt32(2);
                        var date = DateOnly.Parse(reader.GetString(3));
                        var count = reader.GetInt32(4);
                        habitDates.Add(new HabitDate(habitDateId, date, count));
                    }
                    catch
                    {
                        continue;
                    }
                }

                habits.Add(new Habit(id, name, habitDates));
                return habits;
            }
        }
    }

    internal Habit GetHabitByName(string name)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = """
                SELECT H.Id, H.Name, HabitDate.Id as HabitDateId, HabitDate.Date, HabitDate.Count
                FROM Habit as H
                LEFT JOIN HabitDate ON H.Id = HabitDate.HabitId
                WHERE H.Name = @name;
                """;

            command.Parameters.AddWithValue("@name", name);

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows) throw new InvalidOperationException();

                var id = -1;
                var _name = "";
                var habitDates = new List<HabitDate>();

                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    _name = reader.GetString(1);

                    try
                    {
                        var habitDateId = reader.GetInt32(2);
                        var date = DateOnly.Parse(reader.GetString(3));
                        var count = reader.GetInt32(4);
                        habitDates.Add(new HabitDate(habitDateId, date, count));
                    }
                    catch
                    {
                        continue;
                    }
                }

                return new Habit(id, name, habitDates);
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

}

