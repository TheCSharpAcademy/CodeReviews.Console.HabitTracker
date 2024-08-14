using Microsoft.Data.Sqlite;
using System.Globalization;

namespace Habits;

internal class HabitTracker
{
    private static readonly string connectionString = @"Data Source=habit-tracker.db";
    public HabitTracker()
    {
        // Create table
        string commandText = @"CREATE TABLE IF NOT EXISTS heart_points (
                                   Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                   Date TEXT,
                                   Quantity INTEGER
                               )";
        SqlNonQuery(commandText);

    }

    /// <summary>
    /// Connect to the database, run the query command, disconnect, and return the results.
    /// </summary>
    /// <param name="cmd">The SQL command to execute.</param>
    /// <returns>A List of HeartPoints objects from the database.</returns>
    private static List<HeartPoints> SqlQuery(string cmd)
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = cmd;

        var reader = tableCmd.ExecuteReader();
        
        List<HeartPoints> heartPoints = [];

        while (reader.Read())
        {
            HeartPoints entry = new()
            {
                Id = reader.GetInt32(0),
                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                Quantity = reader.GetInt32(2)
            };

            heartPoints.Add(entry);
        }

        connection.Close();

        return heartPoints;
    }

    /// <summary>
    /// Connects to the database, runs the command, and disconnects.
    /// Use for SQL commands that do not require a response.
    /// </summary>
    /// <param name="cmd">The SQL command to execute.</param>
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

    /// <summary>
    /// Entry point for running the Habit Tracker from the CLI.
    /// </summary>
    public static void StartCLI()
    {
        Console.WriteLine("Welcome to the Habit Tracker!\n\n");

        while (true)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("\n\tMAIN MENU\n");
            Console.WriteLine("\tWhat would you like to do?\n");
            Console.WriteLine("\tType 0 to Close Application.");
            Console.WriteLine("\tType 1 to View All Records.");
            Console.WriteLine("\tType 2 to Insert Record.");
            Console.WriteLine("\tType 3 to Delete Record.");
            Console.WriteLine("\tType 4 to Update Record.");
            Console.WriteLine("-----------------------------------------------\n");

            string? userInput = Console.ReadLine() ?? "";
            Console.WriteLine();

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
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

    public static void ViewAllRecords()
    {
        var heartPoints = GetAllRecords();

        Console.WriteLine("Your cardio activity so far:");

        foreach (var row in heartPoints)
        {
            Console.WriteLine(row.Display());
        }
    }

    public static List<HeartPoints> GetAllRecords()
    {
        return SqlQuery("SELECT * FROM heart_points");
    }

    private static void InsertRecord()
    {
        Console.WriteLine("Please insert the date: (Format: dd-mm-yy).");
        string date = GetDateInput();
        Console.WriteLine("\nPlease insert the number of heart points for the day:");
        int quantity = GetNumberInput();

        SqlNonQuery($"INSERT INTO heart_points(date, quantity) VALUES('{date}', {quantity})");
    }

    internal static string GetDateInput()
    {
        while (true)
        {
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
            string numInput = Console.ReadLine() ?? string.Empty;
            if (int.TryParse(numInput, out int number))
            {
                return number;
            }
            Console.WriteLine("Invalid input. Please enter an integer value.");
        }
    }

    internal static void DeleteRecord()
    {
        Console.WriteLine("Enter the ID of the record you would like to delete, or 0 to return to the main menu.");
        int userChoice = GetNumberInput();

        if (userChoice == 0)
        {
            return;
        }

        SqlNonQuery($"DELETE FROM heart_points WHERE id = {userChoice}");
    }

    internal static void UpdateRecord()
    {
        Console.WriteLine("Enter the ID of the record you would like to update, or 0 to return to the main menu.");
        int userChoice = GetNumberInput();

        if (userChoice == 0)
        {
            return;
        }

        var results = SqlQuery($"SELECT * FROM heart_points WHERE id = {userChoice}");

        if (results.Count == 0)
        {
            Console.WriteLine("No entry found. Please try again.");
            return;
        }

        var entry = results.First();

        Console.WriteLine($"Current data: << {entry.Display()} >>");
        Console.WriteLine("Please enter the new date: (Format: dd-mm-yy).\n");
        string newDate = GetDateInput();
        Console.WriteLine("Please enter the new number of heart points:");
        int newQuantity = GetNumberInput();

        SqlNonQuery($"UPDATE heart_points SET date = '{newDate}', quantity = {newQuantity} WHERE id = {entry.Id};");

    }
}
