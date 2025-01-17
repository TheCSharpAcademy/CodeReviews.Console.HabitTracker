using System.Collections.Generic;
using habit_logger.Data;
using habit_logger.Models;
using Microsoft.Data.Sqlite;

namespace habit_logger.Repositories
{
    public class HabitRepository
    {
        public int? GetHabitIdByName(string habitName)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id FROM habits WHERE Name = @Name";
            command.Parameters.AddWithValue("@Name", habitName);

            var result = command.ExecuteScalar();
            return result != null ? (int?)Convert.ToInt32(result) : null;
        }

        public IEnumerable<Habit> GetAllHabits()
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Unit FROM habits";

            using var reader = command.ExecuteReader();
            var habits = new List<Habit>();

            while (reader.Read())
            {
                habits.Add(new Habit
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Unit = reader.GetString(2)
                });
            }

            return habits;
        }

        public void InsertHabit(Habit habit)
        {
            using var connection = Database.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO habits (Name, Unit) VALUES (@Name, @Unit)";
            command.Parameters.AddWithValue("@Name", habit.Name);
            command.Parameters.AddWithValue("@Unit", habit.Unit);
            command.ExecuteNonQuery();
        }
    }
}
