using System.Globalization;
using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=habit-Tracker.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS pet_the_dog ( 
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}

GetUserInput();

void GetUserInput()
{
    Console.Clear();
    
    bool closeApp = false;
    while (closeApp == false)
    {
        DisplayMenu();

        string? commandInput = Console.ReadLine();

        switch (commandInput)
        {
            case "0":
                Console.WriteLine("Application is now closing. Goodbye!");
                closeApp = true;
                break;
            case "1":
                ViewAllRecords();
                break;
            case "2":
                Insert();
                break;
            case "3":
                Update();
                break;
            case "4":
                Delete();
                break;
            default:
                Console.WriteLine("\nInvalid input. Please provide a number from the list provided.");
                break;
        }
    }
}

void DisplayMenu()
{
    Console.WriteLine("\nMAIN MENU\n");
    Console.WriteLine("What would you like to do?\n");
    Console.WriteLine("Type 0 to Close the Application.");
    Console.WriteLine("Type 1 to View All Records.");
    Console.WriteLine("Type 2 to Insert a Record.");
    Console.WriteLine("Type 3 to Update a Record.");
    Console.WriteLine("Type 4 to Delete a Record.");
    Console.WriteLine("-----------------------------------");
    Console.Write("Option: ");
}

void ViewAllRecords()
{
    Console.Clear();
    // Console.WriteLine("Placeholder, this would show all records.");
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"SELECT * FROM pet_the_dog";

        List<DogPets> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        while (reader.Read())
        {
            tableData.Add(new DogPets
            {
                Id = reader.GetInt32(0),
                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-Gb")),
                Quantity = reader.GetInt32(2)
            });
        }

        connection.Close();

        Console.WriteLine("\n=========Dog Petting Tracker=========");
        foreach (var row in tableData)
        {
            Console.WriteLine($"{row.Id} - {row.Date:yyyy-MM-dd} - Quantity: {row.Quantity}");
        }

        Console.WriteLine("=====================================");
    }
    
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();

}

void Insert()
{
    string? date = GetDateInput();

    int quantity = GetNumber("\nHow many times did you pet the dog on this date? Type 0 to return to the main menu: ");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"INSERT INTO pet_the_dog(date, quantity) VALUES ('{date}', {quantity})";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    Console.Clear();
    Console.WriteLine("Your input has been received.\n");
}

string? GetDateInput()
{
    Console.WriteLine("\nPlease enter the date for the record (format dd-mm-yy). Type 0 to return to the main menu: ");
    string? dateInput = Console.ReadLine();

    if (dateInput == "0") GetUserInput();
    return dateInput;
}

int GetNumber(string message)
{
    Console.WriteLine(message);
    string? quantityInput = Console.ReadLine();

    if (quantityInput == "0") GetUserInput();

    int quantity = Convert.ToInt32(quantityInput);
    return quantity;
}

void Update()
{
    Console.WriteLine("Placeholder, this would update a record. Press any key to return to menu...");
    Console.ReadKey();
    Console.Clear();
}

void Delete()
{
    ViewAllRecords();
    
    using (var connection = new SqliteConnection(connectionString))
    {
        int idToDelete = GetNumber("\nEnter the record ID that would like to delete or type 0 to return to the menu: ");

        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            $"DELETE FROM pet_the_dog WHERE Id = '{idToDelete}'";

        tableCmd.ExecuteNonQuery();

        connection.Close();

        Console.WriteLine(
            $"\nRecord ID {idToDelete} has been successfully deleted. Press any key to return to the menu...");
        Console.ReadKey();
    }
}

public class DogPets
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}