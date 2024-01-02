using HabitTracker.StevieTV.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.StevieTV;
internal class HabitTraker
{
    static string connectionString = @"Data Source=HabitTracker.db";
    static SqliteConnection connection = new(connectionString);
    public static void Main()
    {
        CreateDatabaseIfRequired();
        GetUserInput();
    }

    private static void CreateDatabaseIfRequired()
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

    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine(@"
MAIN MENU

What would you like to do?

0 - Close Application
1 - View All Records
2 - Insert Record
3 - Delete Record
4 - Update Record
-------------------------");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4\n");
                    break;
            }
        }
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT * FROM drinking_water";

        List<DrinkingWater> tableData = new();

        SqliteDataReader tableDataReader = tableCmd.ExecuteReader();

        if (tableDataReader.HasRows)
        {
            while (tableDataReader.Read())
            {
                tableData.Add(
                    new DrinkingWater
                    {
                        Id = tableDataReader.GetInt32(0),
                        Date = DateTime.ParseExact(tableDataReader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = tableDataReader.GetInt32(2),
                    });
            }
        }
        else
        {
            Console.WriteLine("No entries found");
        }
        connection.Close();

        Console.WriteLine("--------------------------------------");
        foreach (DrinkingWater drinkingWaterEntry in tableData)
        {
            Console.WriteLine($"{drinkingWaterEntry.Id} - {drinkingWaterEntry.Date.ToString("dd-MMM-yyyy")} - Quantity: {drinkingWaterEntry.Quantity}");
        }
        Console.WriteLine("--------------------------------------");
    }

    private static void Insert()
    {
        var date = GetDateInput();

        var quantity = GetNumberInput("Please insert number of glasses or other measure of your choice (whole numbers only)\n");

        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
        tableCmd.ExecuteNonQuery();
        connection.Close();

    }

    private static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("Please type the ID of the record you want to delete, or type 0 to return to the Main Menu");

        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";
        int rowCount = tableCmd.ExecuteNonQuery();

        if (rowCount == 0)
        {
            Console.WriteLine($"Record with Id {recordId} doesn't exist. \nPress Enter to try again.\n");
            Console.ReadLine();
            connection.Close();
            Delete();
        }

        Console.WriteLine($"Record with Id {recordId} was deleted \n\n");

        connection.Close();

    }
    private static void Update()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("Please type the ID of the record you want to update, or type 0 to return to the Main Menu");

        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
        var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"Record with Id {recordId} doesn't exist\nPress Enter to try again");
            Console.ReadLine();
            connection.Close();
            Update();
        }

        var date = GetDateInput();
        var quantity = GetNumberInput("Please insert number of glasses or other measure of your choice (whole numbers only)\n");

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

        tableCmd.ExecuteNonQuery();
        connection.Close();

    }


    internal static string GetDateInput()
    {
        Console.WriteLine("Please type the date (dd-mm-yy) or type 0 to return to the main menu");
        var input = Console.ReadLine();

        if (input == "0") GetUserInput();

        while (!DateTime.TryParseExact(input, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid Input. Please type the date (dd-mm-yy) or type 0 to return to the main menu");
            input = Console.ReadLine();
        }

        return input;

    }

    private static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        var input = Console.ReadLine();

        if (input == "0") GetUserInput();

        while (!Int32.TryParse(input, out _) || Convert.ToInt32(input) < 0)
        {
            Console.WriteLine($"Invalid Input. {message}");
            input = Console.ReadLine();
        }

        return Convert.ToInt32(input);
    }

}
