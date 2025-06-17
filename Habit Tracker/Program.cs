/*Changing Your Working Directory
This way .NET will build your project in your main folder. By default it builds your project
in a bin folder and just to keep things simple we want to avoid that.
That will create a Properties folder with a launchsettings.json file containing your configuration
information. This is an important step only for applications that use Sqlite because you want the database
to be created in the same folder of the application to avoid confusion.
*/

using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=habit-Tracker.db";

using (var connection = new SqliteConnection(connectionString)) //creating an instance of SqliteConnection.
{
    connection.Open();
    var tableCMD = connection.CreateCommand();

    tableCMD.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        Quantity INTEGER 
        )";  //@ verbatim strings aren't necessary but are a standard because they allow us to create multiline strings which are quite common in SQL
        //notation for integer is different from sql server and there's no date type in sql lite
    tableCMD.ExecuteNonQuery(); //we don't want database to return any values

    connection.Close();

    GetUserInput();
}

void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.WriteLine("\n\n MAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\n Type 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to insert record.");
        Console.WriteLine("Type 3 to delete record.");
        Console.WriteLine("Type 4 to update record");
        Console.WriteLine("----------------------------------------------\n");

        string commandInput = Console.ReadLine();
        int command = 0;
        int.TryParse(commandInput, out command);
        switch (command)
        {
            case 0:
                Console.WriteLine("\nGoodbye");
                closeApp = true;
                Environment.Exit(0);
                break;
            case 1:
                GetAllRecords();
                break;
            case 2:
                Insert();
                break;
            case 3:
                Delete();
                break;
            case 4:
                Update();
                break;
            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}

void Insert()
{
    string date = GetDateInput();

    int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n"); ;

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES ('{date}', {quantity})";

        tableCmd.ExecuteNonQuery();

        connection.Close();
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

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(new DrinkingWater
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

        Console.WriteLine("-------------------------------------------------\n");
        foreach (var dw in tableData)
        {
            Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyy")} - Quantity: {dw.Quantity}");
        }

        Console.WriteLine("--------------------------------------\n");
    }
}

void Delete()
{
    Console.Clear();
    GetAllRecords();

    int recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to" +
        "go back to Main Menu\n\n");

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
    }

    Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

    GetUserInput();

}

void Update()
{
    Console.Clear();
    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to return to main menu.\n\n");

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
            return;
        }
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed) \n\n");

        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}

string GetDateInput()
{
    Console.WriteLine("\n\n Please insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu");

    string dateInput = Console.ReadLine();

    if (dateInput == "0") GetUserInput();

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again:\n\n");
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
        Console.WriteLine("\n\nInvalid number. Try again.\n\n");
        numberInput = Console.ReadLine();
    }

    int finalInput = Convert.ToInt32(numberInput);

    return finalInput;
}

internal class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
