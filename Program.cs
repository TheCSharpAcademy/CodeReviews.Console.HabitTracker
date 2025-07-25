using System.Reflection.Metadata;
using Microsoft.Data.Sqlite;
namespace HabitTracker.mxrt0;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net.WebSockets;
using Microsoft.VisualBasic.FileIO;

class Program
{
    static readonly string connectionString = @"Data Source=../../../habit-Tracker.db";
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText
            = @"CREATE TABLE IF NOT EXISTS learning_js (
                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                  Date TEXT NOT NULL,
                  Quantity INTEGER
                  )";

            tableCommand.ExecuteNonQuery();

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
            Console.WriteLine("\n\nMAINMENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("----------------------------------------\n");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
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
                    Console.WriteLine("Invalid command! Enter number from 0-4.");
                    break;
            }
        }    
    }

    private static void Update()
    {
        Console.Clear();

        int recordToUpdateId = GetNumberInput("\n\nPlease enter the Id of the record you wish to update. Type 0 to return to the Main Menu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM learning_js WHERE Id = {recordToUpdateId})";
            int checkQuery = Convert.ToInt32(checkCommand.ExecuteScalar());
            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordToUpdateId} doesn't exist!\n\n");
                connection.Close();
                Update();
            }
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of hours/minutes: (no decimals)\n\n");

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText
            = $"UPDATE learning_js SET Date = '{date}', Quantity = '{quantity}' WHERE Id = '{recordToUpdateId}'";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

    }

    private static void Delete()
    {
        Console.Clear();

        int recordToDeleteId = GetNumberInput("\n\nPlease enter the Id of the record you wish to delete. Type 0 to return to the Main Menu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText
            = $"DELETE from learning_js WHERE Id = '{recordToDeleteId}'";

            int rowCount = tableCommand.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nNo record with Id {recordToDeleteId} was found in the database!");
                Delete();
            }
        }

        Console.WriteLine($"\n\nRecord with Id {recordToDeleteId} was successfully deleted! \n\n");
        
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText
            = $"SELECT * FROM learning_js ";

            List<LearningJS> tableData = [];

            SqliteDataReader reader = tableCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new LearningJS
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", CultureInfo.CurrentCulture),
                        Quantity = reader.GetInt32(2)
                    });
                }
                for (int i = 0; i < tableData.Count; i++)
                {
                    Console.WriteLine($"Item {i + 1} -> {tableData[i]}");
                }
            }
            else
            {
                Console.WriteLine("No data found!");
            }
          
            connection.Close();
        }
    }

    private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of hours/minutes: (no decimals)\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText
            = $"INSERT INTO learning_js(Date, Quantity) VALUES('{date}', {quantity})";
            tableCommand.ExecuteNonQuery();

            connection.Close();
        }

        GetUserInput();
    }

    private static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0")
        {
            GetUserInput();
        }

        while (!int.TryParse(numberInput, out _) || int.Parse(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid input. Please enter a valid non-negative number!\n\n");
            numberInput = Console.ReadLine();     
        }

        int quantityNum = int.Parse(numberInput);
        return quantityNum;
        
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-MM-yyyy). Type 0 to return to Main Menu or type 'Today' to select current date:");

        string dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            GetUserInput();
        }
        if (dateInput.ToLower() == "today")
        {
            dateInput = new string(DateTime.Today.ToString("dd-MM-yyyy").TakeWhile(c => c != ' ').ToArray());
        }
        else
        {
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. {Format: dd-MM-yyyy}. Type 0 to return to Main Menu or try again:\n\n");
                dateInput = Console.ReadLine();
                if (dateInput == "0")
                {
                    GetUserInput();
                }
                if (dateInput.ToLower() == "Today")
                {
                    dateInput = new string(DateTime.Today.ToShortDateString().TakeWhile(c => c != ' ').ToArray());
                }
            }
        }
        
        return dateInput;
    }
}
