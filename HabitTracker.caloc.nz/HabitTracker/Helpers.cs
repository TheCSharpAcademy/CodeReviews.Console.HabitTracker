using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker;

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}

internal class Helpers
{
    internal static string connectionString = @"Data Source=habit-Tracker.db";
    internal static void GetAllRecords()
    {
        Console.Clear();
        using var connection = new SqliteConnection(connectionString);
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water ";
            List<DrinkingWater> tableData = [];
            SqliteDataReader reader = tableCmd.ExecuteReader();

            string[] formats = ["dd-MM-yy", "dd-MM-yyyy", "dd/MM/yy", "dd/MM/yyyy"];
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (DateTime.TryParseExact(reader.GetString(1), formats, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime date))
                    {
                        tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = date,
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();
            tableData = tableData.OrderBy(dw => dw.Date).ToList();

            Console.WriteLine("-----------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date:dd-MMM-yyyy} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("-----------------------------\n");
        }
    }

    internal static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

        using var connection = new SqliteConnection(connectionString);
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
               $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    internal static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"DELETE from drinking_water WHERE Id = '{recordId}'";
            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} does not exist. \n\n");
                Delete();
            }
        }
        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
        Menu.GetUserInput();
    }

    internal static void Update()
    {
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

        using var connection = new SqliteConnection(connectionString);
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
                connection.Close();
                Update();
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy, dd-mm-yyyy, dd/mm/yy, dd/mm/yyyy). Type 0 to return to main manu.\n\n");

        string dateInput = Console.ReadLine()!;

        if (dateInput == "0")
            Menu.GetUserInput();

        string[] formats = ["dd-MM-yy", "dd-MM-yyyy", "dd/MM/yy", "dd/MM/yyyy"];
        while (!DateTime.TryParseExact(dateInput, formats, new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy, dd-mm-yyyy, dd/mm/yy, dd/mm/yyyy). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine()!;
        }
        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine()!;

        if (numberInput == "0")
            Menu.GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine()!;
        }

        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }
}