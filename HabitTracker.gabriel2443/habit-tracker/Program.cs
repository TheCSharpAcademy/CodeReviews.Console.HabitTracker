using Microsoft.Data.Sqlite;
using System.Globalization;

internal class Program
{
    private static string connectionString = @"Data Source=habit-tracker.db";

    private static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
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

        Menu();
    }

    private static void Menu()
    {
        bool closeApp = false;

        do
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
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
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        } while (closeApp);
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM drinking_water";

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
            }
            else
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();
            Console.WriteLine("------------------------------------------\n");

            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("------------------------------------------\n");
        }
    }

    private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("Please enter the number of glases you drinked, no decimals allowed");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        Console.Clear();
        Menu();
    }

    private static void Update()
    {
        GetAllRecords();
        var id = GetNumberInput("Please select the id you want to Update");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id={id})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n Record with Id{id} does not exist");
                connection.Close();
                Update();
            }

            var date = GetDateInput();
            var quantity = GetNumberInput("Please type the amount of drinking glasses you want to update");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET Date = '{date}', Quantity={quantity} WHERE Id={id}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Menu();
    }

    private static void Delete()
    {
        GetAllRecords();
        Console.WriteLine("Please select a number to delete the row\n");

        var id = GetNumberInput("Please type the Id you want deleted from the record");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{id}'";

            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {id} doesn't exist. \n\n");
                Delete();
            }
        }
        Menu();
    }

    internal static string GetDateInput()
    {
        Console.WriteLine("Please insert the date in this format: dd-mm-yy).Type 0 to return to main menu");

        var dateInput = Console.ReadLine();

        if (dateInput == "0") Menu();

        while (!DateTime.TryParseExact(dateInput, "dd-mm-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nInvalid date format, type 0 to return to main menu or try again");

            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        var numInput = Console.ReadLine();

        if (numInput == "0") Menu();

        while (!Int32.TryParse(numInput, out _) || Convert.ToInt32(numInput) < 0)
        {
            Console.WriteLine("Please enter a valid Number");
            numInput = Console.ReadLine();
        }

        var finalInput = Convert.ToInt32(numInput);

        return finalInput;
    }
}

public class DrinkingWater
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }
}