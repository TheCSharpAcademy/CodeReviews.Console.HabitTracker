using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = @"Data Source=habit-Tracker.db";
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

GetUserInput();
void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close App.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record");
        Console.WriteLine("_____________________________________________\n");

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
                Console.WriteLine("Please enter a number 0-4.");
                break;


        }
    }
}
void GetAllRecords()
{
    Console.Clear();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT * FROM drinking_water";

        List<DrinkingWater> tableData = new();
        SqliteDataReader reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
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

        Console.WriteLine("---------------------------------------------------\n");
        foreach (var drinkingWater in tableData) 
        {
            Console.WriteLine($"{drinkingWater.Id} - {drinkingWater.Date.ToString("dd-MMM-yyyy")} - Quantity: {drinkingWater.Quantity}");
        }
        Console.WriteLine("---------------------------------------------------\n");
    }

}

void Insert()
{
    string date = GetDateInput();
    int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO drinking_water(date, Quantity) VALUES('{date}',{quantity})";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}

string GetDateInput()
{
    Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu.");
    string dateInput = Console.ReadLine();
    if (dateInput == "0") GetUserInput();

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
    {
        Console.WriteLine("You entered a wrong date format!");
        dateInput = Console.ReadLine();
    }
    return dateInput;
}

int GetNumberInput(string message)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();
    if (numberInput == "0") GetUserInput();

    while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
    {
        Console.WriteLine("\n\nInvalid number. Try again. \n\n");
        numberInput = Console.ReadLine();
    }

    int finalInput = Convert.ToInt32(numberInput);
    return finalInput;
}

void Delete()
{
    Console.Clear();
    GetAllRecords();
    var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to Main Menu");
    
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = {recordId}";
        int rowCount = tableCmd.ExecuteNonQuery();
        
        if(rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
            Delete();
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
        GetUserInput();
        connection.Close();
    }
}

void Update()
{
    Console.Clear();
    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to return to Main Menu.\n\n");

    using(var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\nPress any key to continue...\n\n");
            connection.Close();
            Console.ReadKey();
            Update();
        }

        string date = GetDateInput();
        int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}