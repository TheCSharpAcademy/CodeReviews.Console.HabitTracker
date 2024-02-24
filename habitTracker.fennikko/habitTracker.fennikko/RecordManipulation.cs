using System.Globalization;
using Microsoft.Data.Sqlite;

namespace habitTracker.fennikko;
public class RecordManipulation
{
    public static readonly string ConnectionString = @"Data Source=HabitTracker.db";

    public static void InitialDatabaseCreation()
    {
        using var connection = new SqliteConnection(ConnectionString);
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
    public static void GetUserInput()
    {
        Console.Clear();
        var appRunning = true;
        while (appRunning)
        {
            Console.WriteLine(@"MAIN MENU

What would you like to do?
0 - Close application
1 - View all records
2 - Insert a record
3 - Delete a record
4 - Update a record");
            Console.WriteLine("-----------------------------");

            var command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Goodbye!");
                    appRunning = false;
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
                    Console.WriteLine("Invalid command. Please type a number between 0 to 4");
                    break;
            }
        }
    }

    public static void GetAllRecords()
    {
        Console.Clear();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $"Select * FROM drinking_water";
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
                        Date = DateTime.ParseExact(reader.GetString(1),"dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }
        connection.Close();
        Console.WriteLine("------------------------------------------");
        foreach (var dw in tableData)
        {
            Console.WriteLine($"{dw.Id} - {dw.Date:dd-MMM-yyyy} - Quantity: {dw.Quantity}");
        }
        Console.WriteLine("------------------------------------------");
    }

    public static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("Please type the Id of the record you want to delete. or type 0 to go back to the main menu");

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

        var rowCount = tableCmd.ExecuteNonQuery();
        if (rowCount == 0)
        {
            Console.WriteLine($"Record with ID {recordId} doesn't exist.");
            Delete();
        }

        Console.WriteLine($"Record with ID {recordId} was deleted.");
        Thread.Sleep(3000);
        Console.Clear();
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    public static void Update()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("Please type the ID of the record you would like to update. Type 0 to return to the main menu.");
        using var connection = new SqliteConnection( ConnectionString);
        connection.Open();
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
        var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
        if (checkQuery == 0)
        {
            Console.WriteLine($"Record with ID {recordId} doesn't exist.");
            connection.Close();
            Update();
        }

        var date = GetDateInput();
        var quantity = GetNumberInput("Please insert number of glasses (no decimals allowed)");
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    public static void Insert()
    {
        var date = GetDateInput();
        var quantity = GetNumberInput("Please insert a number of glasses (no decimals allowed)");

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    public static string GetDateInput()
    {
        Console.WriteLine("Please insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu");
        var dateInput = Console.ReadLine();
        if(dateInput == "0") GetUserInput();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"),DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again.");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }

    public static int GetNumberInput(string? message)
    {
        Console.WriteLine(message);
        var numberInput = Console.ReadLine();
        if (numberInput == "0") GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("Invalid number. Try again.");
            numberInput = Console.ReadLine();
        }

        var finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }
}