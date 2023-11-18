using Microsoft.Data.Sqlite;
using System.Globalization;

List<string> habitList;
string habitName = "";

string connectionString = @"Data Source=habit-Tracker.db";
using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText = 
        @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            liters INTEGER
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
        Console.Clear();
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close App.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record");
        Console.WriteLine("Type 5 to Add new habit.");
        Console.WriteLine("Type 6 to Ask specific questions.");
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
                Console.WriteLine("\nPress any key to return to Main menu...");
                Console.ReadKey();
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
            case "6":
                QuestionMenu();
                break;
            default:
                Console.WriteLine("\nPlease enter a number 0-5. Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                break;
        }
    }
}
void GetAllRecords()
{
    Console.Clear();
    string habitName = ChooseHabit("\nChoose a habit number from the list:\nType 0 to return to Main menu.\n");
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
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", CultureInfo.InvariantCulture),
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
    habitName = ChooseHabit("\nChoose a habit number from the list:\nType 0 to return to Main menu.\n");
    string date = GetDateInput();
    string columnName = GetColumnName(habitName);
    int quantity = GetNumberInput($"\n\nPlease insert number of {columnName} for {habitName}. (no decimals allowed):\n\n",MenuTypes.MainMenu);

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"INSERT INTO {habitName}(date, {columnName}) VALUES('{date}',{quantity})";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }

    Console.WriteLine("Record added succesfully. Press any key to continue...");
    Console.ReadKey();
}

string GetDateInput()
{
    Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yyyy). Type 0 to return to Main Menu.");
    string dateInput = Console.ReadLine();
    if (dateInput == "0") GetUserInput();

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
    {
        Console.WriteLine("You entered a wrong date format!\nTry again:");
        dateInput = Console.ReadLine();
    }
    return dateInput;
}

int GetNumberInput(string message, MenuTypes menu)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();
    if (numberInput == "0")
    {
        if (menu == MenuTypes.MainMenu) GetUserInput();
        else if(menu == MenuTypes.QuestionMenu) QuestionMenu();
    }

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
    int rowCount;
    int recordId;

    GetAllRecords();

    do
    {
        recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to Main Menu\n",MenuTypes.MainMenu);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM {habitName} WHERE Id = {recordId}";
            rowCount = tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        
        if (rowCount == 0)
        {
            Console.WriteLine($"Record with Id {recordId} doesn't exist.");
        }
    } while (rowCount == 0);

    Console.WriteLine($"Record with Id {recordId} was deleted. Press any key to continue...");
    Console.ReadKey();
}

void Update()
{
    Console.Clear();
    int recordId;
    bool inputIsValid = false;
    GetAllRecords();
    string columnName = GetColumnName(habitName);

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        do
        {
            recordId = GetNumberInput("\nPlease type Id of the record you would like to update. Type 0 to return to Main Menu.\n", MenuTypes.MainMenu);

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habitName} WHERE Id = {recordId})";
            long checkQuery = (long)checkCmd.ExecuteScalar();

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\nPress any key to continue...");
                Console.ReadKey();
            }
            else inputIsValid = true;
        } while (!inputIsValid);
        

        string date = GetDateInput();
        int quantity = GetNumberInput($"\n\nPlease insert number of {columnName} for {habitName}. (no decimals allowed):\n\n", MenuTypes.MainMenu);

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE {habitName} SET date = '{date}', {columnName} = {quantity} WHERE Id = {recordId}";
        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
    Console.WriteLine($"Record with Id {recordId} was updated. Press any key to continue...");
    Console.ReadKey();
}
void AddHabit()
{
    Console.Clear();
        
    Console.WriteLine("\n\nEnter the new habit's name:");
    habitName = Console.ReadLine();
    string habitNameEdited = habitName.Replace(' ', '_').ToLower();
        
    Console.WriteLine("\n\nEnter the unit of measurement (f.e. kilometers, liters, quantity,...):");
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
    List<long>entriesCountList = new();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%'";

        using (var reader = tableCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                habitName = reader.GetString(0);
                habitList.Add(habitName);
            }
        }

        foreach (var habit in habitList)
        {
            var countCmd = connection.CreateCommand();
            countCmd.CommandText = $"SELECT COUNT (*) FROM [{habit}]";
            long count = (long)countCmd.ExecuteScalar();
            entriesCountList.Add(count);
        }
    }
    
    if (habitList.Count > 0)
    {
        for (int i = 1; i <= habitList.Count; i++)
        {
            Console.WriteLine($"{i} - {habitList[i - 1]} - records: {entriesCountList[i-1]}");
        }
    }
    else
    {
        Console.WriteLine("There are no habits yet.");
        GetUserInput();
    }
}

string ChooseHabit(string message)
{
    string result = "";
    bool inputIsValid = false;
    do
    {
        Console.Clear();
        Console.WriteLine(message);
        GetHabitsList();

        string userInput = Console.ReadLine();
        if (int.TryParse(userInput, out int userNumber) && userNumber > 0 && userNumber <= habitList.Count)
        {
            result = habitList[userNumber - 1];
            inputIsValid = true;
        }
        else if (userInput == "0")
        {
            GetUserInput();
        }
        else
        {
            Console.WriteLine("You entered a wrong value. Press any key to continue...");
            Console.ReadKey();
        }
    } while (!inputIsValid);
    
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
void QuestionMenu()
{
    bool questionMenuIsRunning = true;

    Console.Clear();

    habitName = ChooseHabit("\n\nQUESTION MENU\nChoose a habit number from the list: (Type 0 to return to Main menu.)\n");
    string columnName = GetColumnName(habitName);

    do
    {
        Console.Clear();
        Console.WriteLine($"\n\nQUESTION MENU for {habitName}");
        Console.WriteLine("\nWhat would you like to ask?");
        Console.WriteLine("\nType 0 to return to Main menu.");
        Console.WriteLine($"Type 1 to get Sum of all {columnName}");
        Console.WriteLine($"Type 2 to get Average of all {columnName}");
        Console.WriteLine($"Type 3 to get Maximum of all {columnName}");
        Console.WriteLine($"Type 4 to get Minimum of all {columnName}");
        Console.WriteLine("_____________________________________________\n");

        string command = Console.ReadLine();

        switch (command)
        {
            case "0":
                GetUserInput();
                break;
            case "1":
                GetQuantityOperation(habitName, columnName, DbOperTypes.SUM);
                break;
            case "2":
                GetQuantityOperation(habitName, columnName, DbOperTypes.AVG);
                break;
            case "3":
                GetQuantityOperation(habitName, columnName, DbOperTypes.MAX);
                break;
            case "4":
                GetQuantityOperation(habitName, columnName, DbOperTypes.MIN);
                break;
            default:
                Console.WriteLine("\nPlease enter a number 0-3. Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                break;
        }
    } while (questionMenuIsRunning);   
}

bool SpecificYearCheck(string operatorName)
{
    string userInput;
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"Do you want to see {operatorName} from a specific year? (y/n)");

        userInput = Console.ReadLine().ToLower();

        if (userInput == "y") return true;
        if (userInput == "n") return false;
        
        Console.WriteLine("Please write 'y' or 'n'");
    }
}
long PerformDbQuestionOperation(string habitName, string columnName, string operationText, bool specificYear, int year)
{
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var operCmd = connection.CreateCommand();

        if (specificYear)
        {
            operCmd.CommandText = $"SELECT {operationText} ({columnName}) FROM {habitName} WHERE Date LIKE '%{year}'";
        }
        else
            operCmd.CommandText = $"SELECT {operationText} ({columnName}) FROM {habitName}";

        var resultDb = operCmd.ExecuteScalar();
        return resultDb != DBNull.Value ? Convert.ToInt64(resultDb) : -1;
    }
}
void GetQuantityOperation(string habitName, string columnName, DbOperTypes oper)
{
    bool inputIsValid = false;
    string operatorName = "";
    string operationText = "";
    int year = 0;

    switch (oper)
    {
        case DbOperTypes.SUM:
            operationText = "SUM";
            operatorName = "sum";
            break;
        case DbOperTypes.AVG:
            operationText = "AVG";
            operatorName = "average";
            break;
        case DbOperTypes.MAX:
            operationText = "MAX";
            operatorName = "maximum";
            break;
        case DbOperTypes.MIN:
            operationText = "MIN";
            operatorName = "minimum";
            break;
    }

    bool specificYear = SpecificYearCheck(operatorName);

    do
    {
        if (specificYear)
            year = GetNumberInput("\nEnter the year you want to show:\nType 0 to return to Question menu.", MenuTypes.QuestionMenu);

        var result = PerformDbQuestionOperation(habitName, columnName, operationText, specificYear, year);

        if (result != -1)
        {
            if (specificYear)
                Console.WriteLine($"The {operatorName} of {columnName} in the year {year} is {result}.");
            else
                Console.WriteLine($"The {operatorName} of {columnName} is {result}.");

            Console.WriteLine("Press any key to return to Question menu...");
            Console.ReadKey();

            inputIsValid = true;
        }
        else
        {
            Console.WriteLine($"The {operatorName} cannot be calculated. Press any key to continue...");
            Console.ReadKey();
        }
    } while (!inputIsValid);

    QuestionMenu();
}

public class Habit
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}

public enum MenuTypes
{
    MainMenu,
    QuestionMenu
}
public enum DbOperTypes
{
    SUM,
    AVG,
    MAX,
    MIN
}