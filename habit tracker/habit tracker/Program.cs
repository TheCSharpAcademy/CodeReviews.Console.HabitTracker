
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Net.Cache;
using Microsoft.Data.Sqlite;



// ----------------creating database 
string connectionString = @"Data Source=Habit-Tracker.db";
using (var connection = new SqliteConnection(connectionString)) 
{
    connection.Open();
    var tableCmd = connection.CreateCommand();
    tableCmd.CommandText = 
       @"CREATE TABLE IF NOT EXISTS habit_tracked 
(Id INTEGER PRIMARY KEY AUTOINCREMENT, Habit TEXT, Date TEXT, Quantity INTEGER, Unit TEXT)";
    tableCmd.ExecuteNonQuery();
    connection.Close();
}

// ------------- program
SeedingDatabase();
GetUserInput(); 

//----------- methods
static void GetUserInput() // method to get user input
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.WriteLine("\n\n MAIN MENU");
        Console.WriteLine("\n What would you like to do?");
        Console.WriteLine("\n type 0 to close app");
        Console.WriteLine("\n type 1 to view all records");
        Console.WriteLine("\n type 2 to insert record");
        Console.WriteLine("\n type 3 to delete record");
        Console.WriteLine("\n type 4 to update record");
        Console.WriteLine("-----------------------------\n");
        string comInput = Console.ReadLine();

        switch (comInput)
        {
            case "0":
                Console.WriteLine("Goodbye");
                closeApp = true;
                Environment.Exit(0);
                break;
            case "1":
                GetRecords();
                break;
            case "2":
                AddRecords();
                break;
            case "3":
                DeleteRecords();
                break;
            case "4":
                UpdateRecords();
                break;
            default:
                Console.WriteLine("Invalid input. Please type a valid choice");
                break;
        }
    }

}
static void GetRecords() // method to retrieve n display all records
{
    string connectionString = @"Data Source=Habit-Tracker.db";
    Console.Clear();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT * FROM habit_tracked";
        List <HabitRoutine> tableData = new();
        SqliteDataReader reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                new HabitRoutine
                {
                    Id = reader.GetInt32(0),
                    Habit = reader.GetString(1),
                    Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                    Quantity = reader.GetInt32(3),
                    Unit = reader.GetString(4),
                });
            }
        }
        else
        {
            Console.WriteLine("No rows found");
        }
        connection.Close();
        Console.WriteLine("===========================");
        foreach (var routine in tableData)
        {
            Console.WriteLine($"{routine.Id} - {routine.Habit} - {routine.Date.ToString("dd-MM-yy")} - {routine.Quantity} - {routine.Unit}");
        }
        Console.WriteLine("===========================");

    }
}
static void AddRecords() // method do add record
{
    string connectionString = @"Data Source=Habit-Tracker.db";
    string date = GetDateInput();
    string habit = GetStringInput("please enter the habit you'd like to track");
    int quantity = GetNumInput("Please insert the quantity, no decimals allowed");
    string unit = GetStringInput("please enter the unit you'd like to track (examples: glasses, pages, hours, assignments..)");
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO habit_tracked(Habit, Date, Quantity, Unit) VALUES('{habit}','{date}', {quantity}, '{unit}')";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}
static string GetStringInput(string message) // methods to retrieve data
{
    Console.WriteLine($"{message}");
    string input = Console.ReadLine();
    return input;
}
static int GetNumInput(string message)
{
    Console.WriteLine($"{message}");
    string numberInput = Console.ReadLine();
    if ( numberInput == "0" ) GetUserInput();
    int finalInput = Convert.ToInt32(numberInput);
    return finalInput;

}
static string GetDateInput()
{
    Console.WriteLine("Please insert the date (Format: dd-mm-yy) Type 0 to return to main menu");
    string  dateInput= Console.ReadLine();
    if (dateInput == "0") GetUserInput();

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("invalid date, use the format dd-MM-yy or press 0 to return to main menu");
        dateInput = Console.ReadLine();
    }
    return dateInput;
}
static void DeleteRecords() // methods to update data
{
    string connectionString = @"Data Source=Habit-Tracker.db";
    Console.Clear();
    GetRecords();
    var recordId = GetNumInput("\n\n please type the id of the record you want to delete or type 0 to turn back to main menu");
    
    using (var connection = new SqliteConnection(connectionString)) 
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText=$"DELETE from habit_tracked WHERE Id = {recordId}";
        int rowCount = tableCmd.ExecuteNonQuery();
        if (rowCount == 0)
        {
            Console.WriteLine($"Record with id  {recordId} doesn't exist");
            Console.WriteLine("");
            
    }
    else Console.WriteLine("The record has been deleted");
    Console.Clear();
}
}
static void UpdateRecords()
{
    string connectionString = @"Data Source=Habit-Tracker.db";
    Console.Clear();
    GetRecords();
    var recordId = GetNumInput("Please type the id of the record you'd like to update or type 0 to return to main menu");
     using (var connection = new SqliteConnection(connectionString)) 
    {
        connection.Open();
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_tracked WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
        if (checkQuery == 0)
        {
            Console.WriteLine($"record with id {recordId} doesn't exist");
            Console.WriteLine("");
            connection.Close();
            Console.Clear();
        }
        else {
        string habit = GetStringInput("please input the name of the habit");
        string date = GetDateInput();
        int quantity = GetNumInput("please insert quantity");
        string unit = GetStringInput("please input the unit (ex: glasses, pages, hours, assignments..)");
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText=$"UPDATE habit_tracked SET Habit = '{habit}', Date = '{date}', Quantity = {quantity}, Unit = '{unit}' WHERE Id = {recordId}";
        tableCmd.ExecuteNonQuery();
        connection.Close();  
        }
        
    }
}


//random seeding method
static void SeedingDatabase() // method to seed database
{
    Random dice = new Random();
    string connectionString = @"Data Source=Habit-Tracker.db";
    string  [] habits_array = { "writing" , "drinking water", "running", "reading", "studying", "bench presses"}; // array with random habits
    string [] units_array = {"words", "glasses", "km", "pages", "assignments", "repetitions"};
   for (int rep = 0; rep < 101; rep ++)
   {
    string date = RandomDate();
    int quantity = RandomQuantity();
    int index = dice.Next(0, habits_array.Length);
    string habit = habits_array[index];
    string unit = units_array[index];
     using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO habit_tracked(Habit, Date, Quantity, Unit) VALUES('{habit}','{date}', {quantity}, '{unit}')";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }


   }

}
// generate a random date

static string RandomDate()
{
    Random dice = new Random();
    int day; int month; 
    day = dice.Next(1, 32);
    month = dice.Next(1, 13);
    string dayString = day.ToString("D2");
    string monthString = month.ToString("D2");
    string date = $"{dayString}-{monthString}-24";
    while (!DateTime.TryParseExact(date, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        day = dice.Next(1, 32);
        month = dice.Next(1, 13);
        dayString = day.ToString("D2");
        monthString = month.ToString("D2");
        date = $"{dayString}-{monthString}-24";
    }
    return date;
}

 
// generate a random quantity
static int RandomQuantity()
{
    Random dice = new Random();
    int quantity = dice.Next(1, 100);
    return quantity;
}

public class HabitRoutine // 
{
    public int Id { get; set; }
    public string Habit { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
    
}
