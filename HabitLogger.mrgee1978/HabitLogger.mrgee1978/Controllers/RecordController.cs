using Microsoft.Data.Sqlite;
using mrgee1978.HabitLogger.Models.Records;
using System.Globalization;

namespace mrgee1978.HabitLogger.Controllers;

// This class is responsible for handling any tasks relating to habits in the database
public class RecordController
{
    /// <summary>
    /// Adds an individual record to the database
    /// </summary>
    /// <param name="date"></param>
    /// <param name="quantity"></param>
    /// <param name="habitId"></param>
    public void AddRecordToDatabase(string date, int quantity, int habitId)
    {
        using (var connection = new SqliteConnection(DatabaseController.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO records(Date, Quantity, HabitId) VALUES ('{date}', '{quantity}', '{habitId}')";

            tableCmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Removes an individual record from the database
    /// </summary>
    /// <param name="id"></param>
    public void DeleteRecordFromDatabase(int id)
    {
        using (var connection = new SqliteConnection(DatabaseController.ConnectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText =
                    $"DELETE FROM records WHERE Id = {id}";
            }
        }
    }

    /// <summary>
    /// Handles updating an individual record in the database
    /// </summary>
    /// <param name="queryString"></param>
    public void UpdateRecordInDatabase(string queryString)
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
    /// Reads all records in the database into a list of Record objects
    /// </summary>
    /// <returns>As a list all records in the database</returns>
    public List<Record> GetRecords()
    {
        List<Record> records = new List<Record>();

        using (var connection = new SqliteConnection(DatabaseController.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM records";

            using (SqliteDataReader reader = tableCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            records.Add(
                                new Record(
                                    reader.GetInt32(0),
                                    DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-Us")),
                                    reader.GetInt32(2),
                                    reader.GetInt32(3)));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing date: {ex.Message}. Record skipped.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
            }
        }
        return records;
    }
}
