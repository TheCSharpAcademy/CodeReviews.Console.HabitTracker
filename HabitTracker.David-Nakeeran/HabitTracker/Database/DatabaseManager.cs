using Microsoft.Data.Sqlite;
using System.Globalization;
using HabitTracker.Utilities;
namespace HabitTracker.Database;
internal class DatabaseManager
{
    private readonly string connectionString = "Data Source=habit-tracker.db";
    private readonly HabitTrackerHistory _habitTrackerHistory;

    internal DatabaseManager(HabitTrackerHistory habitTrackerHistory)
    {
        _habitTrackerHistory = habitTrackerHistory;
    }
    internal void CreateDatabaseTable()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            QUANTITY INTEGER
        )";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
    internal void Insert(string date, int quantity)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES(@Date, @Quantity)";
        tableCmd.Parameters.AddWithValue("@Date", date);
        tableCmd.Parameters.AddWithValue("@Quantity", quantity);
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
    internal void GetAllRecords()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM drinking_water";

        using var reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                if (!_habitTrackerHistory.CheckForExistingEntries(reader.GetInt32(0)))
                {
                    _habitTrackerHistory.AddToHabitTrackerHistory(reader.GetInt32(0), DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-GB")), reader.GetInt32(2));
                }
            }
        }
        else
        {
            Console.WriteLine("No rows found");
        }
        connection.Close();
        _habitTrackerHistory.PrintHabitTrackerHistory();
    }
    internal void Delete(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "DELETE FROM drinking_water WHERE Id = @Id";
        tableCmd.Parameters.AddWithValue("@Id", id);
        int rowCount = tableCmd.ExecuteNonQuery();
        _habitTrackerHistory.RemoveMatchingIdFromHabitTRackerHistory(id);
        if (rowCount == 0)
        {
            Console.WriteLine($"\n\n Record with the id of {id} does not exist.\n\n");
        }
        else
        {
            Console.WriteLine($"\n\nRecord with id of {id} has been deleted.\n\n");
        }
    }
    internal void Update(int id, string date, int quantity)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "UPDATE drinking_water SET date = @Date, quantity = @Quantity WHERE Id = @Id";
        tableCmd.Parameters.AddWithValue("@Id", id);
        tableCmd.Parameters.AddWithValue("@Date", date);
        tableCmd.Parameters.AddWithValue("@Quantity", quantity);
        tableCmd.ExecuteNonQuery();
        connection.Close();
        _habitTrackerHistory.UpdateHabitTrackerHistory(id, DateTime.ParseExact(date, "dd-MM-yy", new CultureInfo("en-GB")), quantity);
    }
    internal bool DoesRecordExist(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var queryCmd = connection.CreateCommand();
        queryCmd.CommandText = "SELECT Id FROM drinking_water WHERE Id = @Id";
        queryCmd.Parameters.AddWithValue("@Id", id);
        var checkQuery = Convert.ToInt32(queryCmd.ExecuteScalar());
        if (checkQuery == 0)
        {
            connection.Close();
            return false;
        }
        return true;
    }
}