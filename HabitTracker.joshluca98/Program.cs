// SQLite database preparation
using HabitTracker.joshluca98;
using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = @"Data Source=HabitTracker.db";
var db = new Database(connectionString);

void UserMenu()
{
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.Clear();
        Console.WriteLine("\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("------------------------------------------\n");

        string commandInput = Console.ReadLine();

        switch (commandInput)
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
                Console.Clear();
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}

// Insert new record into the drinking_water database
void Insert()
{
    Console.Clear();
    string date = Helper.GetDateInput();
    int quantity = Helper.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}

// Retrieves all records from the drinking_water database
void GetAllRecords()
{
    //Console.Clear();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT * FROM drinking_water";
        tableCmd.ExecuteNonQuery();

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
        Console.Clear();
        Console.WriteLine("------------------------------------------\n");
        foreach (var entry in tableData)
        {
            Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MMM-yyyy")} - Quantity: {entry.Quantity}");
        }
        Console.WriteLine("------------------------------------------\n");
        Console.WriteLine("Press ENTER to return to menu..");
        Console.ReadLine();
    }
}

// Deletes a specified record from the table by Id
void Delete()
{
    Console.Clear();
    GetAllRecords();

    int recordId = Helper.GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");
    if (recordId == 0) return;

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";
        int rowCount = tableCmd.ExecuteNonQuery();
        if (rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
            Delete();
        }
        connection.Close();
    }
    Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
}

// Updates a specified record from the table by Id
void Update()
{
    Console.Clear();
    GetAllRecords();

    int recordId = Helper.GetNumberInput("\n\nPlease type the Id of the record you want to update or type 0 to go back to Main Menu\n\n");
    if (recordId == 0) return;

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
            connection.Close();
            Update();
        }

        string date = Helper.GetDateInput();
        if (date == "0") { return; }
        int quantity = Helper.GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id =  {recordId}";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
    Console.WriteLine($"\n\nRecord with Id {recordId} was updated. \n\n");
}


db.CreateDatabaseTable(connectionString);
UserMenu();