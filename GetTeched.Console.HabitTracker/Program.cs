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
 * Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
 * Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. 
   This is specially helpful during development so you don't have to reinsert data every time you create the database.
 * Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.
 */
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace habit_tracker;
class Program
{
    static string connectionString = @"Data Source=Habit-Tracker.db";
    static SqlCommands sqlCommands = new();
    static string tableName = "drinking_water";
    static void Main(string[] args)
    {
        sqlCommands.SqlInitialize();

        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool endApplication = false;
        int errorCount = 0;
        while (!endApplication)
        {
            Console.WriteLine("\n\tMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?\n");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("Type 5 to View Available Habit Trackers");
            Console.WriteLine("Type 9 to Create New Habit Tracker");
            Console.WriteLine("-------------------------------------------");


            string userInput = Console.ReadLine();
            switch (userInput)
            {
                case "0":
                    Console.WriteLine("\nGood Bye!\n");
                    endApplication = true;
                    break;
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    ViewAllTables();
                    break;
                case "9":
                    CreateNewHabit();
                    break;
                default:
                    Console.WriteLine("Invalid Command. Please Type a number between 0 and 4.\n");
                    errorCount++;
                    if (errorCount == 3)
                    {
                        errorCount = 0;
                        Console.Clear();
                        Console.WriteLine("Invalid Command. Please Type a number between 0 and 4.\n");

                    }
                    userInput = Console.ReadLine();
                    break;
            }
            if (endApplication) break;
        }
    }

    private static void ViewAllTables()
    {
        List<string> tables = sqlCommands.SqlGetTables();

        Console.WriteLine("Current Available Tables:\n");
        foreach (string table in tables)
        {
            Console.WriteLine(table); ;
        }
        Console.WriteLine("\nPress any key to return to the Main Menu.");
        Console.ReadLine();
    }

    private static void UpdateRecord()
    {
        Console.Clear();
        ViewAllRecords();

        bool zeroRow = false;

        var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to update or type 0 to return to Main Menu.\n\n");

        zeroRow = sqlCommands.SqlUpdateActionCheck(tableName, recordId);

        if (zeroRow)
        {
            UpdateRecord();
        }
        else
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
            sqlCommands.SqlUpdateAction(tableName, recordId, date, quantity);
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was updated. \n\nPress any key to return to Main Menu.");
        Console.ReadLine();
        GetUserInput();
    }

    private static void DeleteRecord()
    {
        Console.Clear();
        ViewAllRecords();
        bool zeroRow = false;

        var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to return to Main Menu.\n\n");

        zeroRow = sqlCommands.SqlDeleteAction(tableName, recordId);

        if (zeroRow)
        {
            DeleteRecord();
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n Press any key to return to Main Menu.");
        Console.ReadLine();
        GetUserInput();
    }

    private static void InsertRecord()
    {
        string date = GetDateInput();
        int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
        

        sqlCommands.SqlInsertAction(tableName,date,quantity);
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

    private static void ViewAllRecords()
    {
        Console.Clear();
        sqlCommands.SqlViewAction(tableName);
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
        while(string.IsNullOrEmpty(userInput))
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
        GetUserInput();

    }

}
