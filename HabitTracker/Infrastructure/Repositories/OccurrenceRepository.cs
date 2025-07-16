using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;
using HabitTracker.Domain.Repositories;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Infrastructure.Repositories;

public class OccurrenceRepository(string connectionString) : IOccurrenceRepository
{
    public IReadOnlyList<Occurrence> GetAllOccurrences()
    {
        List<Occurrence> occurrences = [];
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM occurrences ORDER BY datetime(date) DESC";
            var command = new SqliteCommand(query, connection);
            var response = command.ExecuteReader();
            
            while (response.Read())
            {
                occurrences.Add(new Occurrence(response.GetInt32(0),
                                             response.GetString(1),
                                            response.GetInt32(2)
                                             )
                );
            }
        }
        return occurrences;
    }
    
    public IReadOnlyCollection<Occurrence> GetOccurrencesByHabitId(int habitId)
    {
        List<Occurrence> occurrences = [];
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM occurrences WHERE habit_id = @hid ORDER BY datetime(date) DESC";
            SqliteCommand command = new(query, connection);
            command.Parameters.AddWithValue("@hid", habitId);
            var response = command.ExecuteReader();

            while (response.Read())
            {
                occurrences.Add(new Occurrence(response.GetInt32(0),
                                             response.GetString(1),
                                            response.GetInt32(2)
                                             )
                );
            }
        }
        return occurrences;
    }

    public bool CreateOccurrence(OccurrenceCreationDto occurrence)
    {
        var success = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO occurrences (date, habit_id) VALUES (@date, @hid)";
            SqliteCommand command = new(query, connection);
            command.Parameters.AddWithValue("@date", occurrence.Date);
            command.Parameters.AddWithValue("@hid", occurrence.HabitId);
            try
            {
                success = command.ExecuteNonQuery();
            }
            catch (SqliteException e) when (e.SqliteErrorCode == 19)
            {
                throw new InvalidOperationException($"Habit number {occurrence.HabitId} does not exist.");
            }
        }
        return success > 0;
    }

    public bool DeleteOccurrenceById(int id)
    {
        var success = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM occurrences WHERE id = @id";
            SqliteCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", id);
            success = command.ExecuteNonQuery();
        }

        return success > 0;
    }
}