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
    }
}
