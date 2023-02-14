using Microsoft.Data.Sqlite;
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        string connectionString = @"Data Source=habit-Tracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            // AUTOINCREMENT - everytime an entry is added, it will increment
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
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
        Console.Clear();
        bool closeApp = false;
        bool invalidCommand = false;
        while (!closeApp)
        {
            Console.Clear();
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 do Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("----------------------------------------");
            if(invalidCommand)
            {
                Console.Write("Invalid Command. Please choose one of the commands above");
            }
            Console.Write("\n");
            string? commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0": closeApp = true; break;
                case "1": ViewAllRecords(); break;
                case "2": InsertRecord(); break;
                case "3": DeleteRecord(); break;
                case "4": UpdateRecord(); break;
                default:
                    invalidCommand = true;
                    break;
            }
        }
    }

    private static void UpdateRecord()
    {
        throw new NotImplementedException();
    }

    private static void DeleteRecord()
    {
        throw new NotImplementedException();
    }

    private static void InsertRecord()
    {
        string date = GetDateInput();
        if (date == null) return;

        int quantity = GetNumberInput();
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu");

        string? dateInput = Console.ReadLine();

        if (dateInput == "0") dateInput = null;

        return dateInput;
    }

    private static void ViewAllRecords()
    {
        throw new NotImplementedException();
    }
}