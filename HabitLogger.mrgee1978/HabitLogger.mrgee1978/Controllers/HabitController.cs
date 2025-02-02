using Microsoft.Data.Sqlite;
using mrgee1978.HabitLogger.Models.Habits;

namespace mrgee1978.HabitLogger.Controllers;

// This class is responsible for handling any tasks relating to habits in the database
public class HabitController
{
    /// <summary>
    /// Inserts an individual habit into the database.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    public void AddHabitToDatabase(string name, string description)
    {
        using (var connection = new SqliteConnection(DatabaseController.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO habits(Name, Description) VALUES ('{name}', '{description}')";

            tableCmd.ExecuteNonQuery();
        }
    }
    /// <summary>
    /// Removes an individual habit from the database
    /// </summary>
    /// <param name="id"></param>
    public void DeleteHabitFromDatabase(int id)
    {
        using (var connection = new SqliteConnection(DatabaseController.ConnectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText =
                    $"DELETE FROM habits WHERE Id = {id}";
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Handles updating an individual habit in the database
    /// </summary>
    /// <param name="queryString"></param>
    public void UpdateHabitInDatabase(string queryString)
    {
        using (var connection = new SqliteConnection(DatabaseController.ConnectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = queryString;

            tableCmd.ExecuteNonQuery();
        }
    }
    /// <summary>
    /// Reads all habits from the database into a list of Habit objects
    /// </summary>
    /// <returns>As a list all habits in the database</returns>
    public List<Habit> GetHabits()
    {
        List<Habit> habits = new List<Habit>();

        using (var connection = new SqliteConnection(DatabaseController.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM habits";

            using (SqliteDataReader reader = tableCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            habits.Add(
                                new Habit(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2)));
                        }
                        catch (SqliteException ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }
                    }
                }
            }
        }
        return habits;
    }
}
