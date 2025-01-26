
using System.Globalization;
using Microsoft.Data.Sqlite;

/*
*
*
    ******Requirements******
    This is an application where you’ll log occurrences of a habit.

    This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)

    Users need to be able to input the date of the occurrence of the habit

    The application should store and retrieve data from a real database

    When the application starts, it should create a sqlite database, if one isn’t present.

    It should also create a table in the database, where the habit will be logged.

    The users should be able to insert, delete, update and view their logged habit.

    You should handle all possible errors so that the application never crashes.

    You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.

    Follow the DRY Principle, and avoid code repetition.

    Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:
*
*
*/

// Creates database if it does not exist. also looks for and creates drinking_water table if it does not exist in the DB
List<DrinkingWater> myRecordList = new();


string connectionString = @"Data Source = habitTracker.db";
using (SqliteConnection connection = new SqliteConnection(connectionString))
{
    connection.Open();
    SqliteCommand tableCmd = connection.CreateCommand();
    tableCmd.CommandText = 
        @"CREATE TABLE IF NOT EXISTS drinking_water
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
        )";
    tableCmd.ExecuteNonQuery();
    connection.Close();
    
}

MainLoop();

void MainLoop ()
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        
        Console.WriteLine("\n***** Main Menu *****\n");
        Console.WriteLine("Choose an option to execute\n");
        Console.WriteLine("Type 0 to Exit the applications");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.\n");
        Console.Write("\nSelect Command to run: ");
        int userChoice = -1;
        string? readResult = Console.ReadLine();
        if (readResult != null)
            int.TryParse(readResult,  out userChoice);  
        
        switch (userChoice)
        {
            case 0:
                Console.Clear();
                Console.WriteLine("The application will now close.");
                closeApp = true;
                Environment.Exit(0);
                break;
            case 1:
                Console.Clear();
                ViewRecords();
                Console.WriteLine("Press any key to return to the Main Menu.");
                Console.ReadLine();
                Console.Clear();
                break;
            case 2:
                Console.Clear();
                InsertRecord();
                break;
            case 3:
                Console.Clear();
                DeleteRecord();
                Console.WriteLine("Press any key to return to the Main Menu.");
                Console.ReadLine();
                Console.Clear();
                break;
            case 4:
                Console.Clear();
                UpdateRecord();
                ViewRecords();
                Console.WriteLine("Press any key to return to the Main Menu.");
                Console.ReadLine();
                Console.Clear();
                break;
        }
    }
}

void InsertRecord()
{  
    string myDate = GetDate();
    if (myDate == "0") // if user selected 0 to go back to the main menu, exit this method now
        return;

    int myQuantity =GetNumber("Enter quantity of tracked habit, or enter 0 to return to the Main Menu.");
    if (myQuantity == 0) // if user selected 0 to go back to the main menu, exit this method now
        return;

    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        SqliteCommand tableCmd = connection.CreateCommand();
        tableCmd.CommandText = 
        @$"INSERT INTO drinking_water (Date,  Quantity) VALUES ('{myDate}', {myQuantity})";

        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    MainLoop();

}

List<DrinkingWater> ViewRecords()
{
    List<DrinkingWater> tableData = new();
    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        SqliteCommand tableCmd = connection.CreateCommand();
        tableCmd.CommandText = 
        $"SELECT * FROM drinking_water";

        
        SqliteDataReader reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(new DrinkingWater
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yy", new CultureInfo("en-US")),
                    Quantity = reader.GetInt32(2)
                });
            }
        }
        else
            Console.WriteLine("No rows found");

        connection.Close();

        Console.WriteLine("----------------------------------\n");
        foreach (var record in tableData)
        {
            Console.WriteLine($"{Convert.ToString(record.Id).PadRight(3)}  -   {record.Date.ToString("MMM-dd-yyyy")}   -   Quantity: {record.Quantity}");
        }
        Console.WriteLine("\n----------------------------------\n");

    }
    return tableData;
}

void DeleteRecord()
{
    ViewRecords();
    int myRecord = 0; 
    Console.WriteLine("Enter the Record Id of the entry you want to delete, or enter 0 to return to the Main Menu.");
    string? readResult = Console.ReadLine();
    if (readResult != null)
        {
            while (!int.TryParse(readResult, out myRecord)) //if its NOT a number...
            {
                Console.Clear();
                Console.WriteLine("You entered an invalid Record Id. Enter a numeric value. Try again, or enter 0 to return to the Main Menu.");
                readResult = Console.ReadLine();

            }
        }
    if (myRecord == 0)
        return;
    
    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        SqliteCommand tableCmd = connection.CreateCommand();
        tableCmd.CommandText = 
        $"DELETE FROM drinking_water WHERE Id = {myRecord}";
        int rowCount = tableCmd.ExecuteNonQuery();

        Console.Clear();
        
        if (rowCount == 0)
            Console.WriteLine($"\nRecord Id: {myRecord} not found.\n");
        else
            Console.WriteLine($"\nRecord Id: {myRecord} deleted.\n");

        connection.Close();
    }

}

void UpdateRecord() //TODO select which field to update?
{
    ViewRecords();

    int myRecordNumber = GetNumber("Enter Id of  record you would like to update, , or enter 0 to return to the Main Menu.");
    if (myRecordNumber == 0) // if user selected 0 to go back to the main menu, exit this method now
        return;

    // verify record with ID entered exists in DB, if not exit method and return to Main menu
    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        SqliteCommand checkCmd = connection.CreateCommand();
        checkCmd.CommandText = @$"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {myRecordNumber})";
        Int64? checkQuery = (Int64)checkCmd.ExecuteScalar();
        if (checkQuery == 0)
        {
            Console.WriteLine($"\nRecord with Id {myRecordNumber} doesn't exist.\n");
            connection.Close();
            return;
        }
        List<DrinkingWater> myRecordData = ViewRecords();
        string recordBeingUpdated = "";
        foreach (var record in myRecordData)
        {
            if (record.Id == myRecordNumber)
                {
                    recordBeingUpdated = ($"Updating the following record:\n\n{Convert.ToString(record.Id).PadRight(3)}  -   {record.Date.ToString("MMM-dd-yyyy")}   -   Quantity: {record.Quantity}\n");
                }
        }
       
        // TODO show record row. use ViewRecords and add Id as parameter. ViewRecords can return the list and use that to select the row by index or ID #
        Console.Clear();
        Console.WriteLine(recordBeingUpdated);
        string myNewDate = GetDate();
        if (myNewDate == "0") // if user selected 0 to go back to the main menu, exit this method now
            return;
        
        Console.Clear();
        Console.WriteLine(recordBeingUpdated);
        int myNewQuantity = GetNumber("Enter quantity of cups of water consumed, or enter 0 to return to the Main Menu.");
        if (myNewQuantity == 0) // if user selected 0 to go back to the main menu, exit this method now
            return;

        SqliteCommand tableCmd = connection.CreateCommand();
        tableCmd.CommandText = 
            @$"UPDATE drinking_water 
            SET Date ='{myNewDate}', Quantity = {myNewQuantity}
            WHERE Id = {myRecordNumber}";
        
        tableCmd.ExecuteNonQuery();
        connection.Close();

    }

}

string GetDate()
{
    Console.WriteLine("Enter date (format 'MM-dd-yy), or enter 0 to return to the Main Menu.");
    string? readResult = Console.ReadLine();
    if (readResult != null)
    {   
        if (readResult == "0")
            {
                Console.Clear();
                return "0";
            }
        //while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        while (!DateTime.TryParseExact(readResult, "MM-dd-yy", new CultureInfo("en-US"),DateTimeStyles.None, out _))
        {
            Console.Clear();
            Console.WriteLine("You entered an invalid date. Enter date in the format 'MM-dd-yy'. Try again, or enter 0 to return to the Main Menu.");
            readResult = Console.ReadLine();
            if (readResult == "0")
                return "0";
        }     
    }
    Console.Clear();
    return readResult;
}

int GetNumber(string message)
{
    int myNumber = 0;
    Console.WriteLine(message);

    string? readResult = Console.ReadLine();
    
    if (readResult != null)
    {
        while (!int.TryParse(readResult, out myNumber)) //if its NOT a number...
        {
            Console.Clear();
            Console.WriteLine("You entered an invalid number. Enter a numeric value. Try again, or enter 0 to return to the Main Menu.");
            readResult = Console.ReadLine();
            if (readResult == "0")
                return 0;
        }
    }
    Console.Clear();
    return myNumber;
}

class DrinkingWater
{
    public int Id {get; set;}
    public DateTime Date {get; set;}
    public int Quantity {get; set;}
}