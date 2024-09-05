using Microsoft.Data.Sqlite;
using habitTracker.models;
using System.Globalization;

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

    public static void Main(string[] args)
    {
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
                    Console.WriteLine("Invalid command.  Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert a number of glasses or other measure of your choice (no decimals allowed)\n\n");

        // Now that we have user input, insert the record into the database.
        // Pattern is the same.  Open the connection, create command, execute, and close connection.
        using (var connection = new SqliteConnection(connectionString))
        {

            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
            $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
        string dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserInput();

        while(!DateTime.TryParseExact(dateInput,"dd-MM-yy",new CultureInfo("en-US"),DateTimeStyles.None, out _)) {
            Console.WriteLine("\n\nInvalid date. (Format: mm-dd-yy). Type 0 to return to main menu or try again:\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0) {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water";

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
                        }
                    );
                }
            }

            else
            {
                Console.WriteLine("No rows found.");
            }

            connection.Close();

            // Display data
            Console.WriteLine("-----------------------------------------\n");

            foreach (DrinkingWater drinkingWater in tableData)
            {
                Console.WriteLine($"{drinkingWater.Id}\t{drinkingWater.Date.ToString("dd-MM-yyyy")}\tQuantity: {drinkingWater.Quantity}");
            }

            Console.WriteLine("-----------------------------------------\n");
        }

    }

    private static void Delete()
    {
        Console.Clear();

        GetAllRecords();

        int recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = '{recordId}'";

            // Executing this command returns the number of rows effected by the command.
            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");

                Delete();
            }
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted.\n\n");

        GetUserInput();

    }

    internal static void Update()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to return to main menu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
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

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = '{quantity}' WHERE Id = '{recordId}'";

            tableCmd.ExecuteNonQuery();

            connection.Close();

        }
    }
}
