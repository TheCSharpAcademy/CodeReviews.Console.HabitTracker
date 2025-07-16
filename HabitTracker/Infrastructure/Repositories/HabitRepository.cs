using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;
using HabitTracker.Domain.Repositories;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Infrastructure.Repositories;

public class HabitRepository(string connectionString) : IHabitRepository
{
    public IReadOnlyList<HabitDisplayDto> GetAllHabits()
    {
        List<HabitDisplayDto> habits = [];
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT habits.id, habits.name, habits.unit, count(occurrences.id) FROM habits " +
                           "LEFT JOIN occurrences ON habits.id = occurrences.habit_id " +
                           "GROUP BY habits.id, habits.name, habits.unit";
            var command = new SqliteCommand(query, connection);
            var response = command.ExecuteReader();
            
            while (response.Read())
            {
                habits.Add(new HabitDisplayDto(response.GetInt32(0),
                                     response.GetString(1),
                                       response.GetString(2),
                                 response.GetInt32(3)
                                     )
                );
            }  
        }
        return habits;
    }

    public HabitDisplayDto? GetHabitById(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT habits.id, habits.name, habits.unit, count(occurrences.id) FROM habits " +
                              "LEFT JOIN occurrences ON habits.id = occurrences.habit_id " +
                              "WHERE habits.id = @id " +
                              "GROUP BY habits.id, habits.name, habits.unit";
            SqliteCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", id);
            var response = command.ExecuteReader();

            if (response.Read())
            {
                HabitDisplayDto habitDisplay = new(response.GetInt32(0),
                                  response.GetString(1),
                                    response.GetString(2),
                              response.GetInt32(3)
                );
                return habitDisplay;
            }
            return null;
        }
    }

    public bool CreateHabit(HabitCreationDto habit)
    {
        var success = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO habits (name, unit) VALUES (@name, @unit)";
            SqliteCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", habit.Name);
            command.Parameters.AddWithValue("@unit", habit.Unit);
            success = command.ExecuteNonQuery();
        }
        return success > 0;
    }

    public bool UpdateHabit(Habit habit)
    {
        var success = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var query = "UPDATE habits SET name = @name, unit = @unit WHERE id = @id";
            SqliteCommand command = new(query, connection);
            command.Parameters.AddWithValue("@name", habit.Name);
            command.Parameters.AddWithValue("@unit", habit.Unit);
            command.Parameters.AddWithValue("@id", habit.Id);
            success = command.ExecuteNonQuery();
        }

        return success > 0;
    }

    public bool DeleteHabitById(int id)
    {
        var success = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM habits WHERE id = @id";
            SqliteCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", id);
            success = command.ExecuteNonQuery();
        }

        return success > 0;
    }
}