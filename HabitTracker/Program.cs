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
                // ViewAllRecords();
                break;
            case 2:
                InsertRecord();
                break;
            case 3:
                // UpdateRecord();
                break;
            case 4:
                // DeleteRecord();
                break;
        }
    }
}

void InsertRecord()
{
    var date = GetDateInput();
    var quantity = GetNumberInput();

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        $"INSERT INTO pet_the_dog (Date, Quantity) VALUES ('{date}', {quantity})";

    tableCmd.ExecuteNonQuery();
    connection.Close();
}

string? GetDateInput()
{
    Console.WriteLine(
        "\nPlease enter a date for the record (format: yy-mm-dd). Type 0 to return to the main menu: ");
    var dateInput = Console.ReadLine();

    if (dateInput == "0") GetMenuOption();

    while (!ValidateDateFormat(dateInput))
    {
        Console.WriteLine(
            "Invalid input format.\nPlease enter a date for the record (format: yy-mm-dd). Type 0 to return to the main menu: ");
        dateInput = Console.ReadLine();
        if (dateInput == "0") GetMenuOption();
    }

    return dateInput;
}

static bool ValidateDateFormat(string input)
{
    var pattern = @"\d\d-\d\d-\d\d";
    var regex = new Regex(pattern);

    return regex.IsMatch(input);
}

int GetNumberInput()
{
    Console.WriteLine(
        "Please enter the number of times you pet the dog on this date or type 0 to return to the menu: ");
    var input = Console.ReadLine();
    var numberInput = 0;
    
    if (input == "0") GetMenuOption();

    while (!int.TryParse(input, out numberInput))
    {Console.WriteLine(
            "Invalid input format.\nPlease enter the number of times you pet the dog on this date or type 0 to return to the menu: ");
        input = Console.ReadLine();
    }
    
    return numberInput;
}