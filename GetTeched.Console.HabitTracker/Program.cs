/*
 * This is an application where you’ll register one habit.
 * This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
 * The application should store and retrieve data from a real database
 * When the application starts, it should create a sqlite database, if one isn’t present.
 * It should also create a table in the database, where the habit will be logged.
 * The app should show the user a menu of options.
 * The users should be able to insert, delete, update and view their logged habit.
 * You should handle all possible errors so that the application never crashes.
 * The application should only be terminated when the user inserts 0.
 * You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.
 * Your project needs to contain a Read Me file where you'll explain how your app works. Here's a nice example:
 * Example Read Me file: https://github.com/thags/ConsoleTimeLogger
 * -------------------------------------CHALLENGES---------------------------------------------------------------
 * Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit. !!!!COMPLETED!!!!
 * Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. 
   This is specially helpful during development so you don't have to reinsert data every time you create the database.
 * Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.
 */
using System.Globalization;

namespace habit_tracker;
class Program
{
    static SqlCommands sqlCommands = new();
    static RandomData randomData = new();
    static void Main(string[] args)
    {

        randomData.GenerateRandomData();
        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool endApplication = false;
        int errorCount = 0;
        Console.WriteLine("Welcome to your Habit Tracker. Please select a tracker you would like to manage.");
        string tableName = SelectTable();
        Console.Clear();
        while (!endApplication)
        {
            Console.WriteLine("\n\tMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine($"\nYou are currently viewing your {tableName.Replace("_", " ").ToUpper()} habit tracker");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("Type 5 to View Available Habit Trackers.");
            Console.WriteLine("Type 6 to Change Habit Tracker Table.");
            Console.WriteLine("Type 7 to Create New Habit Tracker");
            Console.WriteLine("Type 8 to Generate Reports For Tracker");
            Console.WriteLine("-------------------------------------------");

            string userInput = Console.ReadLine();
            switch (userInput)
            {
                case "0":
                    Console.WriteLine("\nGood Bye!\n");
                    endApplication = true;
                    break;
                case "1":
                    ViewAllRecords(tableName);
                    break;
                case "2":
                    InsertRecord(tableName);
                    break;
                case "3":
                    DeleteRecord(tableName);
                    break;
                case "4":
                    UpdateRecord(tableName);
                    break;
                case "5":
                    ViewAllTables(true);
                    break;
                case "6":
                    tableName = SelectTable();
                    break;
                case "7":
                    CreateNewHabit();
                    break;
                case "8":
                    GenerateReports(tableName);
                    break;
                default:
                    Console.WriteLine("Invalid Command. Please Type a number between 0 and 8.\n");
                    errorCount++;
                    if (errorCount == 3)
                    {
                        errorCount = 0;
                        Console.Clear();
                        Console.WriteLine("Invalid Command. Please Type a number between 0 and 8.\n");
                        userInput = Console.ReadLine();

                    }
                    userInput = Console.ReadLine();
                    break;
            }
            if (endApplication) break;
        }
    }

    private static void GenerateReports(string tableName)
    {
        List<int> records = sqlCommands.sqlNumberOfDays(tableName);
        int sum = records.Sum();
        double ave = records.Average();
        int days = records.Count();
        int min = records.Min();
        int max = records.Max();
        string unitName = sqlCommands.SqlGetUnitName(tableName);

        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------");
        Console.WriteLine($"\t\tReport for {tableName.Replace("_", " ").ToUpper()}");
        Console.WriteLine("--------------------------------------------------------------------");

        Console.WriteLine($"\nTotal {unitName}:\t {sum}\n");
        Console.WriteLine($"Average {unitName}:\t {ave}\n");
        Console.WriteLine($"Total Days Logged:\t {days}\n");
        Console.WriteLine($"Minimum {unitName}:\t {min}\n");
        Console.WriteLine($"Maximum {unitName}:\t {max}\n");
        Console.WriteLine("\n\nPress any key to return to the Main Menu.");
        Console.ReadLine();
        Console.Clear();

    }

    private static string SelectTable()
    {
        ViewAllTables();
        List<string> tables = sqlCommands.SqlGetTables();
        string tableName = "";

        Console.WriteLine("Please type the number of the table you would like to view/modify.\n");
        string userInput = Console.ReadLine();
        while (!int.TryParse(userInput, out _))
        {
            Console.WriteLine($"{userInput} is not a valid request, value must be a number. Please try again.");
            userInput = Console.ReadLine();
        }
        int tableNumber = Convert.ToInt32(userInput);
        while (tableNumber > tables.Count || tableNumber <= 0)
        {
            Console.WriteLine("You have entered a number outside the list provided. Please try again.");
            SelectTable();
        }
        Console.Clear();
        return tableName = tables[tableNumber - 1];
    }

    private static void ViewAllTables(bool returnToMenu = false)
    {
        List<string> tables = sqlCommands.SqlGetTables();
        int listNumber = 1;
        Console.WriteLine("Current Available Tables:\n");
        foreach (string table in tables)
        {
            Console.WriteLine($"{listNumber} - {table}");
            listNumber++;
        }
        if (returnToMenu)
        {
            Console.WriteLine("\nPress any key to return to the Main Menu.");
            Console.ReadLine();
            Console.Clear();
        }

    }

    private static void UpdateRecord(string tableName)
    {
        Console.Clear();
        ViewAllRecords(tableName, false);

        bool zeroRow = false;
        string unitName = sqlCommands.SqlGetUnitName(tableName);
        var recordId = GetNumberInput("\nPlease type the Id of the record you want to update or type 0 to return to Main Menu.\n");

        zeroRow = sqlCommands.SqlUpdateActionCheck(tableName, recordId);

        if (zeroRow)
        {
            UpdateRecord(tableName);
        }
        else
        {
            string date = GetDateInput();
            int quantity = GetNumberInput($"\nPlease insert number of {unitName} or other measure of your choice (no decimals allowed)\n");
            sqlCommands.SqlUpdateAction(tableName, recordId, date, quantity);
            Console.WriteLine($"The following record Id {recordId} [{date}|{quantity} {unitName}] for {tableName.Replace("_", " ").ToUpper()} has been updated");
            Console.WriteLine("\nPress any key to return to the Main Menu or press 1 to add another entry.");
            string userInput = Console.ReadLine();
            Console.Clear();
            if (userInput == "1") UpdateRecord(tableName);
        }


        //GetUserInput();
    }

    private static void DeleteRecord(string tableName)
    {
        Console.Clear();
        ViewAllRecords(tableName, false);

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete or type 0 to return to Main Menu.\n");

        bool zeroRow = sqlCommands.SqlDeleteAction(tableName, recordId);

        if (zeroRow)
        {
            DeleteRecord(tableName);
        }

        Console.WriteLine($"\nRecord with Id {recordId} was deleted. \n\nPress any key to return to Main Menu.");
        Console.ReadLine();
        //GetUserInput();
    }

    private static void InsertRecord(string tableName)
    {
        string date = GetDateInput();
        string unitName = sqlCommands.SqlGetUnitName(tableName);
        int quantity = GetNumberInput($"\n\nPlease insert number of {unitName} or other measure of your choice (no decimals allowed)\n\n");

        sqlCommands.SqlInsertAction(tableName, date, quantity);

        Console.WriteLine($"The following [{date}|{quantity} {unitName}] record for {tableName.Replace("_", " ").ToUpper()} has been added");
        Console.WriteLine("\nPress any key to return to the Main Menu or press 1 to add another entry.");
        string userInput = Console.ReadLine();
        Console.Clear();
        if (userInput == "1") InsertRecord(tableName);
    }

    private static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string numberInput = Console.ReadLine();
        if (numberInput == "0") GetUserInput();
        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvaild number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu.\n\n");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") GetUserInput();
        while (!DateTime.TryParseExact(dateInput, "dd-mm-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to Main Menu or try again\n\n");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }

    private static void ViewAllRecords(string tableName, bool returnToMenu = true)
    {
        Console.Clear();
        sqlCommands.SqlViewAction(tableName);
        if (returnToMenu)
        {
            Console.WriteLine("Press any key to return to the Main Menu.");
            Console.ReadLine();
            Console.Clear();
        }

    }

    private static void CreateNewHabit()
    {
        Console.Clear();
        string tableName;
        string unitMeasurement;

        Console.WriteLine("Existing Tables:");
        foreach (string table in sqlCommands.SqlGetTables())
        {
            Console.WriteLine(table);
        }

        Console.WriteLine("\nPlease enter a name of a new habit you want to keep track of:");
        string userInput = Console.ReadLine();
        while (string.IsNullOrEmpty(userInput))
        {
            Console.WriteLine("Invalid table name or no table name entered. Please Try again.");
            userInput = Console.ReadLine();
        }

        tableName = userInput.ToLower().Trim().Replace(" ", "_");
        string checkTableName = sqlCommands.SqlGetTables().Where(table => table.Contains(userInput)).FirstOrDefault();

        while (checkTableName != null)
        {
            Console.WriteLine($"Table: {tableName} Already exists press any key to try again");
            Console.ReadLine();
            CreateNewHabit();
        }
        Console.WriteLine($"Table: {tableName} has been created.");

        Console.WriteLine($"What is the unit of measurement for {tableName} table? eg. 1 Cup");
        Console.WriteLine("Please enter a unit of measurement.");
        userInput = Console.ReadLine().ToLower().Trim();
        unitMeasurement = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userInput);

        sqlCommands.sqlCreateTable(tableName, unitMeasurement);
        randomData.GenerateRandomData(tableName,false);
        GetUserInput();

    }

    private static void DropTables()
    {
        Console.WriteLine($"Would you like to delete all tables and data? Type yes to proceed with deleting all data and tables or any other key to return to the Main Menu.");
        string userInput = Console.ReadLine().ToLower().Trim();
        if (userInput == "yes")
        {
            Console.WriteLine("You are about to delete all data, this is irrecoverable process. If you are sure Type DROP or any other key to return to the Main Menu.");
            userInput = Console.ReadLine().Trim();
            if (userInput == "DROP")
            {
                //TODO
            }
            
        }

    }

}
