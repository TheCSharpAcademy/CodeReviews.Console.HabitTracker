using System.Globalization;
using Microsoft.Data.Sqlite;

namespace iamryanmacdonald.Console.HabitTracker;

public class Database
{
    private readonly string _path;

    public Database(string path)
    {
        _path = path;

        InitializeDatabase();
    }

    internal void AddRecord(DateOnly date, int quantity)
    {
        using (var connection = new SqliteConnection($"Data Source={_path}"))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES('{date.ToString("dd/MM/yyyy")}', '{quantity}')";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal void DeleteRecord(DrinkingWater record)
    {
        using (var connection = new SqliteConnection($"Data Source={_path}"))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"DELETE FROM drinking_water WHERE Id = '{record.Id}'";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal List<DrinkingWater> GetAllRecords()
    {
        using (var connection = new SqliteConnection($"Data Source={_path}"))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = [];

            var tableReader = tableCmd.ExecuteReader();

            if (tableReader.HasRows)
                while (tableReader.Read())
                    tableData.Add(new DrinkingWater
                    {
                        Date = DateOnly.ParseExact(tableReader.GetString(1), "dd/MM/yyyy", new CultureInfo("en-US")),
                        Id = tableReader.GetInt32(0),
                        Quantity = tableReader.GetInt32(2)
                    });

            connection.Close();

            return tableData;
        }
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection($"Data Source={_path}"))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText =
                """
                CREATE TABLE IF NOT EXISTS drinking_water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                )
                """;
            tableCommand.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal void UpdateRecord(DrinkingWater record, DateOnly date, int quantity)
    {
        using (var connection = new SqliteConnection($"Data Source={_path}"))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {record.Id}";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}