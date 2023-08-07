using Microsoft.Data.Sqlite;

using HabitTracker.MartinL_no.Models;

namespace HabitTracker.MartinL_no.DataAccessLayers;

internal class HabitRepository
{
    private readonly string _connectionString;

    internal HabitRepository()
    {
        _connectionString = "Data Source=habitTracker.db";
        CreateTable();
    }

    internal void CreateTable()
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
                        var date = DateOnly.Parse(reader.GetString(3).Split(' ')[0]);
                        var count = reader.GetInt32(4);
                        dates.Add(new HabitDate(habitDateId, date, count, habitId));
                    }
                    catch
                    {
                        continue;
                    }
                }

                return new Habit(habitId, name, dates.OrderByDescending(d => d.Date).ToList());
            }
        }
    }

    internal HabitTotal GetHabitTotal(int habitId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                SELECT H.Name, SUM(HabitDate.Count) AS Total
                FROM Habit AS H
                LEFT JOIN HabitDate ON H.Id = HabitDate.HabitId
                WHERE H.Id = $id;
                """;

            command.Parameters.AddWithValue("$id", habitId);

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows) throw new InvalidOperationException();

                var name = "";
                var total = 0;
                while (reader.Read())
                {
                    name = reader.GetString(0);
                    total = reader.GetInt32(1);

                }
                return new HabitTotal(name, total);
            }
        }
    }

    internal HabitTotal GetHabitTotalSinceDate(int habitId, DateOnly date)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                SELECT H.Name, SUM(HabitDate.Count) AS Total
                FROM Habit AS H
                LEFT JOIN HabitDate ON H.Id = HabitDate.HabitId
                WHERE H.Id = $id AND HabitDate.Date > $date;
                """;

            command.Parameters.AddWithValue("$id", habitId);
            command.Parameters.AddWithValue("$date", ToSqlDate(date));

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows) throw new InvalidOperationException();

                var name = "";
                var total = 0;
                while (reader.Read())
                {
                    name = reader.GetString(0);
                    total = reader.GetInt32(1);

                }
                return new HabitTotal(name, total);
            }
        }
    }

    internal void AddHabit(Habit habit)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = """
                INSERT INTO Habit (Name)
                VALUES ($name)
                """;

            command.Parameters.AddWithValue("$name", habit.Name);
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

    internal void AddDate(HabitDate habitDate)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = """
                INSERT OR REPLACE INTO HabitDate (Date, Count, HabitId)
                VALUES ($date, $count, $habitId);
                """;

            command.Parameters.AddWithValue("$date", (habitDate.Date.ToString("yyyy-MM-dd") + " 00:00:00.000"));
            command.Parameters.AddWithValue("$count", habitDate.Count);
            command.Parameters.AddWithValue("$habitId", habitDate.HabitId);

            command.ExecuteNonQuery();
        }
    }

    internal void UpdateDate(HabitDate habitDate)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = """
                UPDATE HabitDate
                SET Count = $count
                WHERE
                    Date = $date;
                """;

            command.Parameters.AddWithValue("$date", (habitDate.Date.ToString("yyyy-MM-dd") + " 00:00:00.000"));
            command.Parameters.AddWithValue("$count", habitDate.Count);

            command.ExecuteNonQuery();
        }
    }

    internal void DeleteDate(DateOnly date)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = """
                DELETE FROM HabitDate
                WHERE Date = $date;
                """;

            command.Parameters.AddWithValue("$date", (date.ToString("yyyy-MM-dd") + " 00:00:00.000"));

            command.ExecuteNonQuery();
        }
    }

    private string ToSqlDate(DateOnly date)
    {
        return date.ToString("yyyy-MM-dd");
    }
}

