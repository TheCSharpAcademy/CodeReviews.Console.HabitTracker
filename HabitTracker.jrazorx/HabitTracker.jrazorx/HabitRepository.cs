using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace HabitTracker
{
    public class HabitRepository
    {
        private readonly string connectionString;

        public HabitRepository(string connectionString)
        {
            this.connectionString = connectionString;
            CreateDatabaseAndTable();
        }

        private void CreateDatabaseAndTable()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
            @"
                    CREATE TABLE IF NOT EXISTS Habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Date DATE NOT NULL
                    )
                ";
            createTableCommand.ExecuteNonQuery();
        }

        public void InsertHabit(Habit habit)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText =
                @"
                        INSERT INTO Habits (Name, Quantity, Date)
                        VALUES ($name, $quantity, $date)
                    ";
                insertCommand.Parameters.AddWithValue("$name", habit.Name);
                insertCommand.Parameters.AddWithValue("$quantity", habit.Quantity);
                insertCommand.Parameters.AddWithValue("$date", habit.Date.ToString("yyyy-MM-dd"));

                insertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting habit: {ex.Message}");
            }
        }

        public List<Habit> GetAllHabits()
        {
            List<Habit> habits = [];

            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Habits";

                using var reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                        Date = DateTime.Parse(reader.GetString(3))
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving habits: {ex.Message}");
                Console.ReadLine();
            }

            return habits;
        }
    }
}
