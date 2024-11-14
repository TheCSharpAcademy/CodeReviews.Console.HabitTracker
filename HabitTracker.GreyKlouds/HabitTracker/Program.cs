using System.Globalization;
using Microsoft.Data.Sqlite; // using statement for sqlite 
using System.Collections.Generic;
class Program
{
    // create a connection string for our database
    static string connectionString = @"Data Source=habitTracker.db";
    static void Main(string[] args)
    {
        // establish the connection to our database
        using (var connection = new SqliteConnection(connectionString))
        {
            // open the connection and call the createCommand
            connection.Open();
            var tableCmd = connection.CreateCommand();
            // create a table with a sql query
            tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS DrinkingWater
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
            )";
            // call the execute not query
            tableCmd.ExecuteNonQuery();
            // close teh connection
            connection.Close();

        }
        GetUserinput();
    }
    static void GetUserinput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\t\tMAIN MENU");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1 -> Insert A Record");
            Console.WriteLine("2 -> Update A Record");
            Console.WriteLine("3 -> Delete A Record.");
            Console.WriteLine("4 -> View All Records.");
            Console.WriteLine("5 -> Close Application");
            Console.WriteLine("-----------------------------------------");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    Insert();
                    break;
                case "2":
                    Update();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    GetAllRecords();
                    break;
                case "5":
                    Console.WriteLine("Goodbye!");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid command. Press any key to start over");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }
    private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses or mesurment of your choice (Whole numbers only)\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
            $"INSERT INTO DrinkingWater(date,quantity) VALUES('{date}',{quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    internal static void Update()
    {
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM DrinkingWater WHERE Id = {recordId})";
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
            tableCmd.CommandText = $"UPDATE DrinkingWater SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }


    }
    private static void GetAllRecords()
    {
        Console.Clear();
        // create a connecton to the database:
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            //create a table command to show the data:
            var tableCmd = connection.CreateCommand();

            // SQL query:
            tableCmd.CommandText =
            @"select * from DrinkingWater";

            //Create a list to display to the user:
            List<DrinkingWater> tableData = new();
            // read the SQL database:
            SqliteDataReader reader = tableCmd.ExecuteReader();

            Console.WriteLine("\t\tRecent Records:");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //add data to the DrinkingWater Class
                    tableData.Add
                    (
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "mm-dd-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();

            Console.WriteLine("-----------------------------------------");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("mm-dd-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("-----------------------------------------");
        }

    }
    private static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease select a record you wish to delete. Press 'n' to return to the Main Menu.");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"Delete from DrinkingWater Where Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.");
                Delete();
            }
        }
        Console.WriteLine($"\n\nRecord withd Id {recordId} has been deleted.\n\n");

        GetUserinput();
    }
    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserinput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number.Please try again:");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: mm-dd-yy). Type 0 to return to main menu.");

        string dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserinput();

        while (!DateTime.TryParseExact(dateInput, "mm-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: mm-dd-yy). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }
}
public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}