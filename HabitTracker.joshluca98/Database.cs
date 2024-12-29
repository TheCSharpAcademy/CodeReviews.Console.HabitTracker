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
        int quantity = Helper.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
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
        //Console.Clear();
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
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();
            Console.Clear();
            Console.WriteLine("------------------------------------------\n");
            foreach (var entry in tableData)
            {
                Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MMM-yyyy")} - Quantity: {entry.Quantity}");
            }
            Console.WriteLine("------------------------------------------\n");
            Console.WriteLine("Press ENTER to continue..");
            Console.ReadLine();
        }
    }

    public void Delete()
    {
        Console.Clear();
        GetAllRecords();

        int recordId = Helper.GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");
        if (recordId == 0) return;

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";
            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Delete();
            }
            connection.Close();
        }
        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
    }

    public void Update()
    {
        Console.Clear();
        GetAllRecords();

        int recordId = Helper.GetNumberInput("\n\nPlease type the Id of the record you want to update or type 0 to go back to Main Menu\n\n");
        if (recordId == 0) return;

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                connection.Close();
                Update();
            }

            string date = Helper.GetDateInput();
            if (date == "0") { return; }
            int quantity = Helper.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id =  {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        Console.WriteLine($"\n\nRecord with Id {recordId} was updated. \n\n");
    }

}
