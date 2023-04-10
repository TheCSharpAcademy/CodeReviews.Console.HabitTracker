
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace Habit_Tracker;
class Program
{
    static string connectionString = "Data Source=habit-Tracker.db";
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Date TEXT,
                                    Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool closeapp = false;
        while (closeapp == false)
        {
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("\nWhat would you like to do today");
            Console.WriteLine("\nType 0 to exit programme");
            Console.WriteLine("Type 1 to view all records");
            Console.WriteLine("Type 2 to add a record");
            Console.WriteLine("Type 3 to remove a record");
            Console.WriteLine("Type 4 to update a record");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("GoodBye");
                    closeapp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    AddToRecord();
                    break;
                case "3":
                    RemoveRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
        }
    }

    private static void AddToRecord()
    {
        Console.Clear();
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses, no decimals allowed\n\n");

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
        Console.WriteLine("Please enter date in format dd-mm-yyyy");

        string dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserInput();

        while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nInvalid date (Format: dd-mm-yy).");
            GetDateInput();
        }

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\nInvalid number. Try again.");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;

    }

    private static void ViewAllRecords()
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
                        });
                }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();
            
                Console.WriteLine("-------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("---------------------------------------\n");

            }
        
        
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }

    private static void RemoveRecord()
    {
        Console.Clear();
        ViewAllRecords();

        var recordId = GetNumberInput("\n\nPlease enter Id of record you wish to delete or press 0 to return to the main menu");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesnt exist \n\n");
                RemoveRecord();
            }
        }
        Console.WriteLine($"Record Id {recordId} deleted.");
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        ViewAllRecords();

        var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update or type 0 to return to the main menu\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand() ;
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar()) ;

           

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesnt exist,\n\n");
                connection.Close();
                UpdateRecord();
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("Please insert number of glasses no decimals allowed");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();       
            
            connection.Close() ;
        }
    }
}

