// See https://aka.ms/new-console-template for more information

using Microsoft.Data.Sqlite;
using System.Threading.Channels;

string connectionString = @"Data Source=C:\Users\M\source\repos\HabitLoggerV2\HabitLoggerV2\HabitLogger.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER)";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}

MainMenu();

void MainMenu()
{
    Console.Clear();

    bool isAppRunning = true;
    while (isAppRunning == true)
    {
        Console.WriteLine("------------------");
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("------------------");
        Console.WriteLine("What do you want to do?");
        Console.WriteLine();
        Console.WriteLine("Type 0 to close.");
        Console.WriteLine("Type 1 to view all records.");
        Console.WriteLine("Type 2 to insert a record");
        Console.WriteLine("Type 3 to delete a record");
        Console.WriteLine("Type 4 to update a record");
        Console.WriteLine("------------------\n");

        var userInput = Console.ReadLine();
        switch (userInput)
        {
            case "0":
                Console.WriteLine("Cya!");
                isAppRunning = false; 
                Environment.Exit(0);
                break;
            case "1":
                ViewAllRecords();
                break;
            case "2":
                InsertRecord();
                break;
            case "3":
                DeleteRecord();
                break;
            case "4":
                UpdateRecord();
                break;
            default:
                Console.WriteLine("Invalid command.");
                break;
        }
    }
}

void UpdateRecord()
{
    Console.Clear();
    Console.WriteLine("---------------");
    Console.WriteLine("Updating a record.");
    Console.WriteLine("---------------");

    int parsedIDInteger = 0;
    bool isValidID = false;
    while (isValidID == false)
    {
        ViewAllRecords();

        //ID logic and validation
        Console.WriteLine("Enter the ID of the record you want to update. Or press 0 to cancel and go to main menu.");
        var userInputID = Console.ReadLine();

        if (userInputID == "0")
        {
            MainMenu();
        }
        else if (string.IsNullOrEmpty(userInputID) || !int.TryParse(userInputID, out parsedIDInteger) || parsedIDInteger < 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer and correct ID, ENTER to try again.");
            Console.ReadLine();
        }
        else
        {
            isValidID = true;
        }
        Console.Clear();
    }

    //date logic and validation
    DateTime parsedDate = DateTime.MinValue;
    bool isValidDate = false;
    while (isValidDate == false)
    {
        Console.WriteLine("Enter a date, DD-MM-YY format. Or press 0 to cancel and go to main menu.");
        var dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            MainMenu();
        }
        else if (string.IsNullOrEmpty(dateInput) || !DateTime.TryParseExact(dateInput, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
        {
            Console.WriteLine("Invalid input. Please enter a date with format dd-MM-yy, or 0 to cancel.");
        }
        else
        {
            parsedDate = parsedDate.Date;
            isValidDate = true;
        }
        Console.Clear();
    }


    //quantity logic and validation
    int quantityInteger = 0;
    bool isValidInteger = false;
    while (isValidInteger == false)
    {
        Console.WriteLine("Enter a quantity, only positive integer pls. Or press 0 to cancel and go to main menu.");
        var quantityInput = Console.ReadLine();

        if (quantityInput == "0")
        {
            MainMenu();
        }
        else if (string.IsNullOrEmpty(quantityInput) || !int.TryParse(quantityInput, out quantityInteger) || quantityInteger < 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer, or 0 to cancel.");
        }
        else
        {
            isValidInteger = true;
        }
        Console.Clear();
    }

    // write to db
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"UPDATE drinking_water " +
            $"SET Date = '{parsedDate:dd-MM-yyyy}', Quantity = {quantityInteger} " +
            $"WHERE Id = {parsedIDInteger}";


        int rowsAdded = tableCmd.ExecuteNonQuery();
        Console.WriteLine($"You updated {rowsAdded} row(s).");

        connection.Close();
    }
}

void DeleteRecord()
{
    Console.Clear();
    Console.WriteLine("---------------");
    Console.WriteLine("Deleting a record.");
    Console.WriteLine("---------------");

    int parsedIDInteger = 0;
    bool isValidID = false;
    while (isValidID == false)
    {
        ViewAllRecords();
        Console.WriteLine("Enter the ID of the record you want to delete. Or press 0 to cancel and go to main menu.");
        var userInputID = Console.ReadLine();

        if (userInputID == "0")
        {
            MainMenu();
        }
        else if (string.IsNullOrEmpty(userInputID) || !int.TryParse(userInputID, out parsedIDInteger) || parsedIDInteger < 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer and correct ID, ENTER to try again.");
            Console.ReadLine();
        }
        else
        {
            isValidID = true;
        }
        Console.Clear();
    }
    // delete from db
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"DELETE FROM drinking_water WHERE Id = {parsedIDInteger}";


        int rowsEdited = tableCmd.ExecuteNonQuery();

        if (rowsEdited == 0)
        {
            Console.WriteLine("That record doesn't exist brotato! Enter a correct ID. Press Enter to continue.");
            Console.ReadLine();
        }
        Console.WriteLine($"You edited {rowsEdited} row(s).");

        connection.Close();
    }
}

void InsertRecord()
{
    Console.Clear();
    Console.WriteLine("---------------");
    Console.WriteLine("Inserting new record.");
    Console.WriteLine("---------------");

    //date logic and validation
    DateTime parsedDate = DateTime.MinValue;
    bool isValidDate = false;
    while (isValidDate == false)
    {
        Console.WriteLine("Enter a date, DD-MM-YY format. Or press 0 to cancel and go to main menu.");
        var dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            MainMenu();
        }
        else if (string.IsNullOrEmpty(dateInput) || !DateTime.TryParseExact(dateInput, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
        {
            Console.WriteLine("Invalid input. Please enter a date with format dd-MM-yy, or 0 to cancel.");
        }
        else
        {
            parsedDate = parsedDate.Date;
            isValidDate = true;
        }
        Console.Clear();
    }
    

    //quantity logic and validation
    int quantityInteger = 0;
    bool isValidInteger = false;
    while (isValidInteger == false)
    {
        Console.WriteLine("Enter a quantity, only positive integer pls. Or press 0 to cancel and go to main menu.");
        var quantityInput = Console.ReadLine();

        if (quantityInput == "0")
        {
            MainMenu();
        }
        else if (string.IsNullOrEmpty(quantityInput) || !int.TryParse(quantityInput, out quantityInteger) || quantityInteger < 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer, or 0 to cancel.");
        }
        else
        {
            isValidInteger = true;
        }
        Console.Clear();
    }

    // write to db
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"INSERT INTO drinking_water (Date, Quantity)" +
            $"VALUES ('{parsedDate:dd-MM-yyyy}', {quantityInteger})";


        int rowsAdded = tableCmd.ExecuteNonQuery();
        Console.WriteLine($"You added {rowsAdded} row.");

        connection.Close();
    }
}

void ViewAllRecords()
{
    Console.Clear();
    List<DrinkingWater> DrinkingWaterList = new List<DrinkingWater>();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"SELECT * FROM drinking_water";

        using (var reader = tableCmd.ExecuteReader())
        {
            while(reader.Read())
            {
                DrinkingWaterList.Add(
                    new DrinkingWater {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", null),
                        Quantity = reader.GetInt32(2)
                });
            }
        }
        foreach (DrinkingWater v in DrinkingWaterList)
        {
            Console.WriteLine($"ID: {v.Id} - {v.Date:dd-MM-yyyy}, {v.Quantity} amounts");
        }
        Console.WriteLine("------------------");
            connection.Close();
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}