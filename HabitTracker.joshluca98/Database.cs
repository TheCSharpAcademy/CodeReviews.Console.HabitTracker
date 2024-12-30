using HabitTracker.joshluca98.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.joshluca98;

public class Database
{
    private readonly string _connectionString;

    public Database(string connectionString)
    {
        _connectionString = connectionString;
        CreateDatabaseTable();
    }

    public void CreateDatabaseTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Insert()
    {
        Console.Clear();
        string date = Helper.GetDateInput();
        if (date == "0") return;
        int quantity = Helper.GetNumberInput("\nPlease type number of glasses (no decimals allowed):\n");
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void GetAllRecords()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM drinking_water";
            tableCmd.ExecuteNonQuery();

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    }); ;
                }
            }

            connection.Close();
            Console.Clear();

            Console.WriteLine("------------------------------------------\n");
            if (tableData.Count > 0)
            {
                foreach (var entry in tableData)
                {
                    Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MMM-yyyy")} - Quantity: {entry.Quantity}");
                }
                Console.WriteLine("\n------------------------------------------\n");
            }
            else Console.WriteLine("(!) No records exist in the database\n");
        }
    }

    public void Delete()
    {
        Console.Clear();
        GetAllRecords();
        int recordId = Helper.GetNumberInput("\nPlease type the ID of the record to delete or type 0 to go back to the menu\n");
        if (recordId == 0) return;
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";
            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\nRecord with ID '{recordId}' does NOT exist.\n");
                Delete();
            }
            else
            {
                Console.WriteLine($"\nRecord with ID number '{recordId}' was deleted.\n");
                Console.WriteLine("Press ENTER to continue..");
                Console.ReadLine();
            }
            connection.Close();
        }
    }

    public void Update()
    {
        Console.Clear();
        GetAllRecords();
        int recordId = Helper.GetNumberInput("\n\nPlease type the ID of the record you want to update or type 0 to go back to the menu:\n");
        if (recordId == 0) return;
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with ID {recordId} doesn't exist.\n\n");
                connection.Close();
                Update();
            }
            string date = Helper.GetDateInput();
            if (date == "0") { return; }
            int quantity = Helper.GetNumberInput("\nPlease type number of glasses (no decimals allowed):\n");
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id =  {recordId}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.WriteLine($"\nRecord with Id {recordId} was updated.\n");
        Console.WriteLine("Press ENTER to continue..");
        Console.ReadLine();
    }
}