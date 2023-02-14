using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading.Channels;

internal class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    private static void Main(string[] args)
    {
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

        string? quantity = GetNumberInput("\n\nPlease insert the number of glasses. Integers only");
        if (!int.TryParse(quantity, out int quantity_number)) return;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES ('{date}',{quantity_number})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu");

        string? dateInput = Console.ReadLine();

        if (dateInput == "0" || dateInput == "") dateInput = null;

        return dateInput;
    }

    private static string? GetNumberInput(string message)
    {
        Console.WriteLine(message);
        Console.Write("\n");
        int number;
        string? input = null;
        input = Console.ReadLine();

        if (int.TryParse(input, out number))
        {
            return input;
        }
        else { return input; }
        
    }

    private static void ViewAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
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
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    }); ;
                }
            } else { Console.WriteLine("No rows found"); }

            connection.Close();

            Console.WriteLine("-----------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}"); 
            }
            Console.WriteLine("\n-----------------------------\n");
        }
        Console.ReadLine();
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}