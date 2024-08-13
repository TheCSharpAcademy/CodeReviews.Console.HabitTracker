using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Reflection.PortableExecutable;

namespace Habits;

internal class HabitTracker
{
    private static readonly string connectionString = @"Data Source=habit-tracker.db";
    public HabitTracker()
    {
        // Create table
        string commandText = @"CREATE TABLE IF NOT EXISTS cardio_minutes (
                                   Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                   Date TEXT,
                                   Quantity INTEGER
                               )";
        SqlNonQuery(commandText);

    }

    private static List<CardioMinutes> SqlQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = cmd;

        var reader = tableCmd.ExecuteReader();
        
        List<CardioMinutes> cardioMinutes = [];

        while (reader.Read())
        {
            CardioMinutes entry = new()
            {
                Id = reader.GetInt32(0),
                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                Quantity = reader.GetInt32(2)
            };

            cardioMinutes.Add(entry);
        }

        connection.Close();

        return cardioMinutes;
    }

    private static void SqlNonQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = cmd;

        var result = tableCmd.ExecuteNonQuery();

        connection.Close();

        return;
    }

    public static void StartCLI()
    {
        Console.WriteLine("Welcome to the Habit Tracker!\n\n");

        while (true)
        {
            Console.WriteLine("MAIN MENU\n");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("-----------------------------------------------\n");

            string? userInput = Console.ReadLine() ?? "";

            // Exit on 0.
            if (userInput.Equals("0"))
            {
                Console.WriteLine("Thank you for using the Habit Tracker.\n");
                return;
            }

            // Handle user selection.
            switch (userInput)
            {
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    //TODO DeleteRecord()
                    break;
                case "4":
                    //TODO UpdateRecord()
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public static void ViewAllRecords()
    {
        var cardioMinutes = GetAllRecords();

        foreach (var row in cardioMinutes)
        {
            Console.WriteLine($"{row.Id}. {row.Date.ToString("dd-MM-yy")}, {row.Quantity} minutes.");
        }
    }

    public static List<CardioMinutes> GetAllRecords()
    {
        return SqlQuery("SELECT * FROM cardio_minutes");
    }

    private static void InsertRecord()
    {
        string date = GetDateInput();
        int quantity = GetNumberInput();

        SqlNonQuery($"INSERT INTO cardio_minutes(date, quantity) VALUES('{date}', {quantity})");
    }

    internal static string GetDateInput()
    {
        while (true)
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy).\n");
            string dateInput = Console.ReadLine() ?? string.Empty;
            
            if (!dateInput.Equals("")) //TODO validate date input
            {
                return dateInput;
            }

            Console.WriteLine("Invalid date. Please try again.");
        }
    }

    internal static int GetNumberInput()
    {
        while (true)
        {
            Console.WriteLine("Please insert the number of cardio zone minutes for the day:\n");
            string numInput = Console.ReadLine() ?? string.Empty;
            if (int.TryParse(numInput, out int number))
            {
                return number;
            }
            Console.WriteLine("Invalid input. Please enter an integer value.");
        }
    }
}
