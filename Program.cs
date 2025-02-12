using System;
using System.Globalization;
using Microsoft.Data.Sqlite;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Habit_Logger
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    public static bool isMain;
    public static Dictionary<string, int> operation = new Dictionary<string, int>()
        {
            { "View", 1 },
            { "Insert", 2},
            { "Delete", 3},
            { "Update", 4}
        };

    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
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

        GetUserInput();
    }

    private static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;

        while (closeApp == false)
        {
            Console.WriteLine("--------HABIT LOGGER--------");
            Console.WriteLine("         MAIN MENU         ");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Records.");
            Console.WriteLine("Type 3 to Delete Records.");
            Console.WriteLine("Type 4 to Update Records.");
            Console.WriteLine("----------------------------");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Goodbye!");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    isMain = true;
                    ViewAllRecords();
                    break;
                case "2":
                    InsertRecords();
                    break;
                case "3":
                    DeleteRecords();
                    break;
                case "4":
                    UpdateRecords();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    private static void ViewAllRecords()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * from drinking_water";

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2),
                    });
                }
            }

            connection.Close();

            Console.WriteLine("--------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("--------------------\n");
        }

        if (isMain) AskForNextAction(0, "View");
    }
    private static void InsertRecords()
    {
        Console.Clear();

        DateTime date = GetDateInput();
        var dateString = date.ToString("dd-MM-yy");
        int quantity = GetNumberInput("Please insert number of glasses or other mesure of your choice (no decimals allowed)");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES('{dateString}', {quantity})";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        AskForNextAction(1, "Insert");
    }
    private static void DeleteRecords()
    {
        isMain = false;
        int checkQuery;
        int recordId;

        Console.Clear();
        ViewAllRecords();

        using (var connection = new SqliteConnection(connectionString))
        {
            do
            {
                recordId = GetNumberInput("Please type the Id of the record you want to delete or type 0 to go back to Main Menu.");

                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText =
                    $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.Clear() ;
                    ViewAllRecords();
                    Console.WriteLine($"Record with Id {recordId} doesn't exist.\n Please type another Id.");
                    connection.Close();
                }
            } while (checkQuery == 0);

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE from drinking_water WHERE Id = '{recordId}'";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        AskForNextAction(recordId, "Delete");
    }
    private static void UpdateRecords()
    {
        isMain = false;
        int checkQuery;
        int recordId;

        ViewAllRecords();

        using (var connection = new SqliteConnection(connectionString))
        {
            do
            {
                recordId = GetNumberInput("Please type the Id of the record you want to update or type 0 to go back to Main Menu.");

                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText =
                    $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.Clear();
                    ViewAllRecords();
                    Console.WriteLine($"Record with Id {recordId} doesn't exist.\n Please type another Id.");
                    connection.Close();
                }
            } while (checkQuery == 0);

            DateTime date = GetDateInput();
            var dateString = date.ToString("dd-MM-yy");
            int quantity = GetNumberInput("Please insert number of glasses or other mesure of your choice (no decimals allowed)");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{dateString}', quantity = {quantity} WHERE Id = {recordId}";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        AskForNextAction(recordId, "Update");
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
    private static DateTime GetDateInput()
    {
        DateTime date;
        bool success;

        do
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.");
            string dateInput = Console.ReadLine();
            success = DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out date);
            if (dateInput == "0") GetUserInput();
            if (!success)
            {
                Console.Clear();
                ViewAllRecords();
                Console.WriteLine("Invalid date format. Please enter the date in the format dd-MM-yy.");
            }
        } while (!success);

        return date;
    }
    private static int GetNumberInput(string message)
    {
        bool success = true;
        int finalInput;

        do
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();
            success = int.TryParse(numberInput, out finalInput);

            if (!success)
            {
                Console.Clear();
                ViewAllRecords();
                Console.WriteLine("\nInvalid Command. Please type a id of records.\n");
            }
        }
        while (success == false && finalInput >=0 && finalInput <=4);

        return finalInput;
    }
    private static void AskForNextAction(int recordId, string userInput)
    {
        int user;

        if (userInput == "Insert")
        {
            user = GetNumberInput($"The record has been inserted, please enter {operation[userInput]} to {userInput} another one or 0 to go back to Main Menu");
        } else if (userInput == "View")
        {
            user = GetNumberInput("Enter 0 to go back to Main Menu.");
        }
        else
        {
            user = GetNumberInput($"The {recordId} has been {userInput}d, please enter {operation[userInput]} to {userInput} another one or 0 to go back to Main Menu");
        }

        switch (user)
        {
            case 0:
                GetUserInput();
                break;

            case 2:
                InsertRecords();
                break;

            case 3:
                DeleteRecords();
                break;

            case 4:
                UpdateRecords();
                break;
        }
    }
}

