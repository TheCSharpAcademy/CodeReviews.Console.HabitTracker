using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

public class DatabaseManager
{
    private const string ConnectionString = "Data Source=habit_tracker.db";

    public DatabaseManager()
    {
        CreateDatabase();
        SeedData();
    }

    private void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS HabitTypes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Unit TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Habits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Quantity INTEGER NOT NULL,
                Date TEXT NOT NULL,
                HabitTypeId INTEGER NOT NULL,
                FOREIGN KEY (HabitTypeId) REFERENCES HabitTypes(Id)
            )";
            command.ExecuteNonQuery();
        }
    }

    private void SeedData()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            // Check if data already exists
            command.CommandText = "SELECT COUNT(*) FROM HabitTypes";
            int count = Convert.ToInt32(command.ExecuteScalar());

            if (count == 0)
            {
                // Seed HabitTypes
                var habitTypes = new[]
                {
                   ("WATER", "GLASSES"),
                   ("EXERCISE", "MINUTES"),
                   ("READING", "PAGES"),
                   ("MEDITATION", "MINUTES")
               };

                foreach (var (name, unit) in habitTypes)
                {
                    command.CommandText = "INSERT INTO HabitTypes (Name, Unit) VALUES ($name, $unit)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("$name", name);
                    command.Parameters.AddWithValue("$unit", unit);
                    command.ExecuteNonQuery();
                }

                // Seed Habits
                var random = new Random();
                var startDate = DateTime.Now.AddDays(-100);

                for (int i = 0; i < 100; i++)
                {
                    var habitTypeId = random.Next(1, 5); // Assuming 4 habit types
                    var quantity = random.Next(1, 11); // Random quantity between 1 and 10
                    var date = startDate.AddDays(i);

                    command.CommandText = "INSERT INTO Habits (Quantity, Date, HabitTypeId) VALUES ($quantity, $date, $habitTypeId)";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("$quantity", quantity);
                    command.Parameters.AddWithValue("$date", date.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("$habitTypeId", habitTypeId);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Database seeded successfully.");
            }
        }
    }

    public void InsertHabitType(string name, string unit)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO HabitTypes (Name, Unit)
            VALUES ($name, $unit)";
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$unit", unit);
            command.ExecuteNonQuery();
        }
    }

    public List<(int Id, string Name, string Unit)> GetHabitTypes()
    {
        var habitTypes = new List<(int, string, string)>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Unit FROM HabitTypes";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    habitTypes.Add((reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                }
            }
        }
        return habitTypes;
    }

    public void UpdateHabitType(int id, string name, string unit)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
            UPDATE HabitTypes
            SET Name = $name, Unit = $unit
            WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$unit", unit);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new Exception("Habit Type ID does not exist.");
            }
        }
    }

    public void DeleteHabitType(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
            DELETE FROM HabitTypes
            WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new Exception("Habit Type ID does not exist.");
            }
        }
    }


    public void InsertHabit(Habit habit)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Habits (Quantity, Date, HabitTypeId)
                VALUES ($quantity, $date, $habitTypeId)";
            command.Parameters.AddWithValue("$quantity", habit.Quantity);
            command.Parameters.AddWithValue("$date", habit.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("$habitTypeId", habit.HabitTypeId);
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
            command.CommandText = @"
                SELECT h.Id, h.Quantity, ht.Name, ht.Unit, h.Date
                FROM Habits h
                JOIN HabitTypes ht ON h.HabitTypeId = ht.Id";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Quantity = reader.GetInt32(1),
                        HabitTypeName = reader.GetString(2),
                        Unit = reader.GetString(3),
                        Date = DateTime.Parse(reader.GetString(4))
                    });
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
            SET Quantity = $quantity, Date = $date, HabitTypeId = $habitTypeId
            WHERE Id = $id";
            command.Parameters.AddWithValue("$id", habit.Id);
            command.Parameters.AddWithValue("$quantity", habit.Quantity);
            command.Parameters.AddWithValue("$date", habit.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("$habitTypeId", habit.HabitTypeId);
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
