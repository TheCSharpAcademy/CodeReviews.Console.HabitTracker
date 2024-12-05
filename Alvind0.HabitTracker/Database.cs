using System.Globalization;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Alvind0.HabitTracker;

internal class Database
{
    private const string ConnectionString = @"Data Source=habit-Tracker.db";
    internal record WalkingRecord(int Id, DateTime Date, int Quantity);

    internal static void CreateDatabase()
    {
        using (SqliteConnection connection = new SqliteConnection(ConnectionString))
        {
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS walkingHabit(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER)";
                tableCmd.ExecuteNonQuery();
            }
        }
    }

    internal static void AddRecord()
    {
        var date = GetDate("\nEnter the date (format - dd-MM-yy) or insert 0 to go back to Main Menu:\n");
        var quantity = GetNumber("\nEnter the number of meters walked(positive integers only) or enter 0 to go back to Main Menu:\n");
        Console.Clear();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO walkingHabit(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
        }

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
        Console.Clear();
    }

    internal static void DeleteRecord()
    {
        GetRecords();

        var id = GetNumber("Please type the id of the record you want to delete.");

        using (var connection = new SqliteConnection(ConnectionString))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = @$"DELETE FROM walkingHabit WHERE Id = {id}";

                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
        Console.Clear();
    }

    internal static void UpdateRecord()
    {
        GetRecords();
        var id = GetNumber("Please type the id of the record you want to delete.");

        string date = "";
        bool updateDate = AnsiConsole.Confirm("Update date?");
        if (updateDate)
        {
            date = GetDate("\nEnter the date (format : dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");
        }

        int quantity = 0;
        bool updateQuantity = AnsiConsole.Confirm("Update distance?");
        if (updateQuantity)
        {
            quantity = GetNumber("\nPlease enter number of meters walked (no decimals or negatives allowed) or enter 0 to go back to Main Menu.");
        }

        string query;
        if (updateDate && updateQuantity)
        {
            query = $"UPDATE walkingHabit SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
        }
        else if (updateDate && !updateQuantity)
        {
            query = $"UPDATE walkingHabit SET Date = '{date}' WHERE Id = {id}";
        }
        else
        {
            query = $"UPDATE walkingHabit SET Quantity = '{quantity}' WHERE Id = {id}";
        }

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = query;

            tableCmd.ExecuteNonQuery();
        }

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
        Console.Clear();
    }

    internal static void ViewRecords(List<WalkingRecord> records)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Amount");

        foreach (var record in records)
        {
            table.AddRow(record.Id.ToString(), record.Date.ToString(), record.Quantity.ToString());
        }

        AnsiConsole.Write(table);

    }

    internal static void GetRecords()
    {
        List<WalkingRecord> records = new List<WalkingRecord>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM walkingHabit";

            using (SqliteDataReader reader = tableCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            records.Add(
                                new WalkingRecord(
                                    reader.GetInt32(0),
                                    DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                    reader.GetInt32(2))
                                );
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing data: {ex.Message}. Skipping this record.");
                        }
                    }
                }
                else Console.WriteLine("No rows found.");
            }
        }
        ViewRecords(records);
    }

    internal static string GetDate(string message)
    {
        Console.WriteLine(message);

        string? dateInput = Console.ReadLine();

        if (dateInput == "0") Menu.MainMenu();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            Console.Clear();
            Console.WriteLine("Invalid date (format dd-MM-yy). Please try again.\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    private static int GetNumber(string message)
    {
        Console.WriteLine(message);

        string? numberInput = Console.ReadLine();
        int output;

        if (numberInput == "0") Menu.MainMenu();

        while (!int.TryParse(numberInput, out output) || Convert.ToInt32(numberInput) <= 0)
        {
            Console.Clear();
            Console.WriteLine("Invalid number (positive integers only). Please try again.\n");
            numberInput = Console.ReadLine();
        }
        return output;
    }
}
