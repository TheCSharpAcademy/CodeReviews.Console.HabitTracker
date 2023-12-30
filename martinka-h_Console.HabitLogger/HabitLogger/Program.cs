using Microsoft.Data.Sqlite;
using System.Globalization;

class Program
{
    static string connectionString = @"Data Source = habitlogger.db";

    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
DATE TEXT,
Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        GetUserInput();
    }
    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine(
    @"
Type 0 to exit the app
Type 1 to view all records.
Type 2 to insert record.
Type 3 to delete record.
Type 4 to update record.
---------------------------------------------");

            string? command = Console.ReadLine();

            switch (command)
            {
                case "0":
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
                    DeleteRecord();
                    break;
               case "4":
                    UpdateRecord();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    } 
    static void Insert()
    {
        string date = GetDateInput();
        int quantity = GetNumberInput("\n\nPlease insert the number of glasses or other measure of your choice(no decimals allowed). Type 0 to return to main menu.\n\n");

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

    static void GetAllRecords()
    {
        Console.Clear();
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water ";

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
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                        });
                }
            } else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();

            Console.WriteLine("-------------------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("-------------------------------------------------\n");

        }
    }

    private static void DeleteRecord()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete or type 0 to return to main menu.");

        if (recordId == 0)
        {
            GetUserInput();
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\nRecord with Id {recordId} doesn't exist;\n");
                DeleteRecord();
            }

            Console.WriteLine($"\nRecord with Id {recordId} was deleted. Click Enter to continue. \n");
            Console.ReadLine();
            GetUserInput();
        }
    }

    static void UpdateRecord()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\nPlease type the Id of the record you would like to update.Type 0 to return to main menu.\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
                connection.Close();
                UpdateRecord();
            }

            string date = GetDateInput();
            int quantity = GetNumberInput("\nPlease insert the number of glasses or other measure of your choice (no decimals allowed). Type 0 to return to main menu.\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
            Console.WriteLine("The record was updated. Press Enter to continue.");

            tableCmd.ExecuteNonQuery();
            connection.Close();

            Console.ReadLine();
            GetUserInput();
        }
    }
    static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-MM-yyyy). Type 0 to return to main menu.");
        string? dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserInput();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nInvalid date. (Format: dd-MM-yyyy). Type 0 to return to main menu or try again.");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string? numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\nInvalid number. Try again.\n");
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}