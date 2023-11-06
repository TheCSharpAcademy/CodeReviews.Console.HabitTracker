using Microsoft.Data.Sqlite;
using System.Globalization;

List<string> habitList;

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
        Console.WriteLine("Type 5 to Add new habit.");
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
            case "5":
                AddHabit();
                break;
            default:
                Console.WriteLine("Please enter a number 0-5.");
                break;
        }
    }
}
void GetAllRecords()
{
    Console.Clear();
    string habitName = ChooseHabit();
    string columnName = GetColumnName(habitName);
    List<Habit> tableData = new();


    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT * FROM {habitName}";

        using (var reader = tableCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", CultureInfo.InvariantCulture),
                            Quantity = reader.IsDBNull(2) ? 0 : reader.GetInt32(2)
                        });
                }
            }
            else
            {
                Console.WriteLine("No records found. Press any key to return to the Main menu.");
                Console.ReadKey();
                GetUserInput();
            }
        }

    }

    Console.WriteLine("---------------------------------------------------\n");
    Console.WriteLine($"Records in {habitName} table:\n\n");
    
    foreach (var habit in tableData) 
    {
        Console.WriteLine($"{habit.Id} - {habit.Date.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)} - {columnName}: {habit.Quantity}");
    }
    Console.WriteLine("---------------------------------------------------\n");
}



void Insert()
{
    string habitName = ChooseHabit();
    string date = GetDateInput();
    string columnName = GetColumnName(habitName);
    int quantity = GetNumberInput($"\n\nPlease insert number of {columnName} for {habitName}. (no decimals allowed):\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO {habitName}(date, {columnName}) VALUES('{date}',{quantity})";
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

    using (var connection = new SqliteConnection(connectionString))
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
void AddHabit()
{
    Console.Clear();
        
    Console.WriteLine("\n\nEnter the new habit's name:");
    string habitName = Console.ReadLine();
    string habitNameEdited = habitName.Replace(' ', '_').ToLower();
        
    Console.WriteLine("\n\nEnter the unit of measurement (f.e. kilometres, liters, quantity,...):");
    string habitUnit = Console.ReadLine();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText =
            @$"CREATE TABLE IF NOT EXISTS {habitNameEdited} (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        {habitUnit} INTEGER
        )";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}

void GetHabitsList()
{
    habitList = new();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%'";

        using (var reader = tableCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var habitName = reader.GetString(0);
                habitList.Add(habitName);
            }
        }
    }

    if (habitList.Count > 0)
    {
        for (int i = 1; i <= habitList.Count; i++)
        {
            Console.WriteLine($"{i} - {habitList[i - 1]}");
        }
    }
    else
    {
        Console.WriteLine("There are no habits yet.");
        GetUserInput();
    }
}

string ChooseHabit()
{
    string result = "";
    Console.WriteLine("\n\nChoose a habit number from the list:\n");
    GetHabitsList();

    string userInput = Console.ReadLine();
    if(int.TryParse(userInput, out int userNumber)&& userNumber > 0)
    {
        result = habitList[userNumber-1];
    }
    else 
    {
        Console.WriteLine("You entered a wrong value.");
        ChooseHabit();
    }
    return result;
}

string GetColumnName(string habitName)
{
    string columnName = "";
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var columnCmd = connection.CreateCommand())
        {
            columnCmd.CommandText = $"SELECT name FROM (SELECT * FROM pragma_table_info('{habitName}') LIMIT 1 OFFSET 2);";
            columnName = (string)columnCmd.ExecuteScalar();
        }
    }
    return columnName;
}
public class Habit
{
    public Habit()
    {
        EntriesCount++;
    }
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public static int EntriesCount { get; private set; }
}
