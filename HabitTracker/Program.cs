using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

var connectionString = @"Data Source=habit-tracker.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS pet_the_dog(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";

    tableCmd.ExecuteNonQuery();
    connection.Close();
}

GetMenuOption();

static void DisplayMainMenu()
{
    Console.WriteLine("\nMAIN MENU\n");
    Console.WriteLine("What would you like to do?\n");

    var menuBuilder = new StringBuilder();
    menuBuilder.AppendLine("Type 0 to Close the Application.");
    menuBuilder.AppendLine("Type 1 to View All Records.");
    menuBuilder.AppendLine("Type 2 to Insert a Record.");
    menuBuilder.AppendLine("Type 3 to Update a Record.");
    menuBuilder.AppendLine("Type 4 to Delete a Record.");
    menuBuilder.AppendLine("-----------------------------------");

    Console.WriteLine(menuBuilder.ToString());
    Console.Write("Option: ");
}

void GetMenuOption()
{
    Console.Clear();
    var closeApp = false;

    while (closeApp == false)
    {
        DisplayMainMenu();
        var input = Console.ReadLine();
        int option;

        while (!int.TryParse(input, out option) || option < 0 || option > 4)
        {
            Console.WriteLine("Invalid input. Please enter a valid option.");
            Console.Write("Option: ");
            input = Console.ReadLine();
        }

        switch (option)
        {
            case 0:
                Console.WriteLine("This application is now closing. Goodbye!");
                closeApp = true;
                break;
            case 1:
                ViewAllRecords();
                break;
            case 2:
                InsertRecord();
                break;
            case 3:
                // UpdateRecord();
                break;
            case 4:
                DeleteRecord();
                break;
        }
    }
}

void ViewAllRecords()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"SELECT * FROM pet_the_dog";

        List<DogPets> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read()) // returns a bool value. True if more rows, false if none
            {
                tableData.Add(new DogPets
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy",
                        new CultureInfo(
                            "en-Gb")), // Parses string from Sqlite to DateTime format. Couldn't get it to go in format yy-MM-dd
                    Quantity = reader.GetInt32(2)
                });
            }

            connection.Close();

            Console.WriteLine("\n==========Dog Petting Tracker==========");
            foreach (var row in tableData)
            {
                Console.WriteLine(
                    $"{row.Id} - {row.Date: yyyy-MMM-dd} - No. of Dog Pets: {row.Quantity}"); // Was able to parse DateTime to new format here
            }

            Console.WriteLine("=======================================");
        }
        else
        {
            Console.WriteLine("\nNo rows in table to display.");
            Console.WriteLine("=======================================");
        }

    }
}

void InsertRecord()
{
    Console.Clear();

    var date = GetDateInput(
        "\nPlease enter a date for the record (format: yy-mm-dd). Type 0 to return to the main menu: ");
    var quantity =
        GetNumberInput(
            "Please enter the number of times you pet the dog on this date or type 0 to return to the menu: ");

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        $"INSERT INTO pet_the_dog (Date, Quantity) VALUES ('{date}', {quantity})";

    tableCmd.ExecuteNonQuery();
    connection.Close();
}

string? GetDateInput(string message)
{
    Console.WriteLine(message);
    var dateInput = Console.ReadLine();

    if (dateInput == "0") GetMenuOption();

    while (!ValidateDateFormat(dateInput))
    {
        Console.WriteLine(
            $"Invalid input format.\n{message}");
        dateInput = Console.ReadLine();
        if (dateInput == "0") GetMenuOption();
    }

    return dateInput;
}

static bool ValidateDateFormat(string? input)
{
    var pattern = @"\d\d-\d\d-\d\d";
    var regex = new Regex(pattern);

    return input != null && regex.IsMatch(input);
}

int GetNumberInput(string message)
{
    Console.WriteLine(message);
    var input = Console.ReadLine();
    int numberInput;

    if (input == "0") GetMenuOption();

    while (!int.TryParse(input, out numberInput))
    {
        Console.WriteLine(
            $"Invalid input format.\n{message}");
        input = Console.ReadLine();
    }

    return numberInput;
}

void DeleteRecord()
{
    ViewAllRecords();

    var recordId =
        GetNumberInput("Please enter the record id number you wish to delete or type 0 to return to the main menu: ");

    using var connection = new SqliteConnection(connectionString);
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"DELETE FROM pet_the_dog WHERE Id = '{recordId}'";

        var rowCount = tableCmd.ExecuteNonQuery();

        Console.WriteLine(rowCount == 0
            ? $"Record {recordId} does not exist in the database. Please try again."
            : $"\nRecord ID {recordId} has been successfully deleted");

        connection.Close();
    }
}

public class DogPets
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}