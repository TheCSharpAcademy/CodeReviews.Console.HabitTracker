using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.frockett;

internal class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    static void Main(string[] args)
    {
        bool shouldSeedData = CheckForTable("drinking_water");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER
                                        )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        GetUserInput();
    }

    static void GetUserInput()
    {
        //Console.Clear();
        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine("\t\tMain Menu");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Please select an option from the menu below\n");
            Console.WriteLine("1. View All Records");
            Console.WriteLine("2. Insert Record");
            Console.WriteLine("3. Delete Record");
            Console.WriteLine("4. Update Record");
            Console.WriteLine("5. Close Application");
            Console.WriteLine("_______________________________________________");
            string? readResult = Console.ReadLine();

            if (!int.TryParse(readResult, out int input))
            {
                Console.WriteLine("Invalid selection, please input a valid integer choice");
                readResult = Console.ReadLine();
            }

            switch (input) { 
                case 1:
                    PrintAllRecords();
                    break;
                case 2:
                    InsertRecord();
                    break;
                case 3:
                    DeleteRecord();
                    break;
                case 4:
                    UpdateRecord();
                    break;
                case 5:
                    closeApp = true;
                    break;
                default:
                    Console.WriteLine("Selection not recognized, please input a selection between 1 and 5");
                    break;
            }
            
        }
        return;
    }

    // This method checks to see if the table exists at application startup. It returns a bool which is used to determine whether or not to generate seed data
    private static bool CheckForTable(string tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'";

            var result = checkCmd.ExecuteScalar();
            int resultCount = Convert.ToInt32(result);

            if (resultCount > 0)
            {
                return true;
            }
        }
        return false;
    }

    private static void UpdateRecord()
    {
        Console.Clear();
        PrintAllRecords();

        var recordId = GetQuantityInput("\nPlease enter the ID of the record you want to update, or enter 0 to return to the main menu\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 from drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with ID {recordId} does not exist.\n");
                connection.Close();
                UpdateRecord();
            }

            string date = GetDateInput();
            int quantity = GetQuantityInput("\nPlease enter number of glasses or other metric (number must be an integer), or enter 0 to return to main menu\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            int rowCount = tableCmd.ExecuteNonQuery();
        }
    }

    private static void DeleteRecord()
    {
        Console.Clear();
        PrintAllRecords();

        int recordId = GetQuantityInput("\nPlease enter the ID of the record you want to delete, or enter 0 to return to the main menu\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with ID {recordId} doesn't exist. \n\n");
                DeleteRecord();
            }
            else
            {
                Console.WriteLine($"\n\nRecord with ID {recordId} deleted successfully. \n\n");
            }
        }

    }

    private static void PrintAllRecords()
    {
        Console.Clear();
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new List<DrinkingWater>();

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
                        Quantity = reader.GetInt32(2),
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();
            Console.WriteLine("-----------------------------------------------\n");
            foreach(var entry in tableData)
            {
                Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MMM-yyyy")} - Quantity: {entry.Quantity}");
            }
            Console.WriteLine("-----------------------------------------------\n");
        }
    }

    private static void InsertRecord()
    {
        string date = GetDateInput();
        int quantity = GetQuantityInput("\n\nPlease enter number of glasses or other metric (number must be an integer)\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static string GetDateInput()
    {
        Console.Clear();
        Console.WriteLine("\n\nPlease insert the date: (Format dd-mm-yy). Type 0 to return to main menu\n\n");

        string? dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserInput();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime cleanDate))
        {
            Console.WriteLine("\nInvalid date. Format must be (dd-mm-yy). Enter 0 to return to main menu\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }
    private static int GetQuantityInput(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        string? quantityInput = Console.ReadLine();

        if (!int.TryParse(quantityInput, out int cleanQuantityInput))
        {
            Console.WriteLine("Invalid input, please enter a whole number");
            quantityInput = Console.ReadLine();
        }
        else if (quantityInput == "0")
        {
            GetUserInput();
        }
   
        return cleanQuantityInput;
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
