using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

namespace habitTracker;

public class Program
{
    private static string connectionString = @"Data Source=habit-Tracker.db";

    public Program()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            // Create database table.
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

    public static void Main(string[] args) {
        GetUserInput();
    }

    static void GetUserInput()
    {

        Console.Clear();
        bool closeApp = false;

        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Appplication.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert a Record.");
            Console.WriteLine("Type 3 to Delete a Record.");
            Console.WriteLine("Type 4 to Update a Record.");
            Console.WriteLine("----------------------------------------------\n");

            string commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0":
                    Console.WriteLine("Goodbye!");
                    closeApp = true;
                    break;

                // case "1":
                //     GetAllRecords();
                //     break;

                case "2":
                    Insert();
                    break;

                    // case "3":
                    //     Delete();
                    //     break;

                    // case "4":
                    //     Update();
                    //     break;

                    // default:
                    //     Console.WriteLine("Invalid command.  Please type a number from 0 to 4.\n");
                    //     break;
            }
        }
    }

    private static void Insert() {
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert a number of glasses or other measure of your choice (no decimals allowed)\n\n");

        // Now that we have user input, insert the record into the database.
        // Pattern is the same.  Open the connection, create command, execute, and close connection.
        using (var connection = new SqliteConnection(connectionString)) {

            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
            $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal static string GetDateInput() {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
        string dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserInput();

        return dateInput;
    }

    internal static int GetNumberInput(string message) {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserInput();

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

}
