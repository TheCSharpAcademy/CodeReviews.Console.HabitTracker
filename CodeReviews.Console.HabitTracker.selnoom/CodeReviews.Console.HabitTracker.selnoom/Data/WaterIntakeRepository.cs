using CodeReviews.Console.HabitTracker.selnoom.Model;
using Microsoft.Data.Sqlite;

namespace CodeReviews.Console.HabitTracker.selnoom.Data;

internal class WaterIntakeRepository
{
    private const string connectionString = @"Data Source=habit-Tracker.db";

    public List<WaterIntake> GetRecords()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT Id, Date, Quantity FROM drinking_water";

            using var reader = tableCmd.ExecuteReader();

            List<WaterIntake> records = new();

            while (reader.Read())
            {
                WaterIntake record = new WaterIntake()
                {
                    Id = reader.GetInt32(0),
                    Date = reader.GetString(1),
                    Quantity = reader.GetInt32(2)
                };
                records.Add(record);
            }
            return records;
        }
    }

    public void CreateRecord(string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"
                INSERT INTO drinking_water (Date, Quantity)
                VALUES (@date, @quantity)";

            tableCmd.Parameters.AddWithValue("@date", date);
            tableCmd.Parameters.AddWithValue("@quantity", quantity);
            tableCmd.ExecuteNonQuery();
        }
    }

    public void DeleteRecord(int? id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"
            DELETE FROM drinking_water
            WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", id);
        tableCmd.ExecuteNonQuery();
    }

    internal void UpdateRecord(int? id, string date, int? quantity)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"
            UPDATE drinking_water
            SET Date = @date,
                Quantity = @quantity
            WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", id);
        tableCmd.Parameters.AddWithValue("@date", date);
        tableCmd.Parameters.AddWithValue("@quantity", quantity);
        tableCmd.ExecuteNonQuery();
    }
}
