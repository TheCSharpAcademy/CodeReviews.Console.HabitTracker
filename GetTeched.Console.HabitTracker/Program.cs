using System.Globalization;

namespace habit_tracker;
class Program
{
    static SqlCommands sqlCommands = new();
    static RandomData randomData = new();
    static void Main(string[] args)
    {
        bool testingMode = true;

        if (testingMode)
        {
            randomData.GenerateRandomData();
        }
        else
        {
            sqlCommands.SqlInitialize("drinking_water", "Glasses");
        }

        GetUserInput();
    }

    static void GetUserInput(string tableName = "", bool showTables = true)
    {
        Console.Clear();
        bool endApplication = false;
        bool inputError = false;
        if (showTables)
        {
            Console.WriteLine("Welcome to your Habit Tracker. Please select a tracker you would like to manage.");
            tableName = SelectTable();
        }
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
            Console.WriteLine("Type 9 to Delete Current Selected Table");
            Console.WriteLine("Type drop to Reset To Factory Defaults");
            Console.WriteLine("-------------------------------------------");

            if (inputError)
            {
                Console.WriteLine("Invalid Command. Please Type a number between 0 and 8.");
                Console.WriteLine("-------------------------------------------\n");
                inputError = false;
            }

            string userInput = Console.ReadLine().ToLower().Trim();
            switch (userInput)
            {
                case "0":
                    Console.WriteLine("\nGood Bye!\n");
                    endApplication = true;
                    Environment.Exit(0);
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
                case "9":
                    DropTable(tableName);
                    break;
                case "drop":
                    DropAllTables();
                    break;
                default:
                    inputError = true;
                    Console.Clear();
                    break;
            }
            if (endApplication) break;
        }
    }

    private static void GenerateReports(string tableName)
    {
        List<int> records = sqlCommands.SqlReporting(tableName);
        int sumRecords = records.Sum();
        double averageRecords = records.Average();
        string averageFormat = string.Format("{0:N2}", averageRecords);
        int daysLogged = records.Count();
        int minimum = records.Min();
        int maximum = records.Max();
        string unitName = sqlCommands.SqlGetUnitName(tableName);

        Console.Clear();
        Console.WriteLine("--------------------------------------------------------------------");
        Console.WriteLine($"\t\tReport for {tableName.Replace("_", " ").ToUpper()}");
        Console.WriteLine("--------------------------------------------------------------------");

        Console.WriteLine($"\nTotal Days Logged: {daysLogged}\n");
        Console.WriteLine($"Total {unitName}: {sumRecords}\n");
        Console.WriteLine($"Average {unitName}: {averageFormat}\n");
        Console.WriteLine($"Minimum {unitName}: {minimum}\n");
        Console.WriteLine($"Maximum {unitName}: {maximum}\n");
        Console.WriteLine("\n\nPress any key to return to the Main Menu.");
        Console.ReadLine();
        Console.Clear();

    }

    private static string SelectTable()
    {
        ViewAllTables();

        List<string> tables = sqlCommands.SqlGetTables();
        string tableName = "";
        string userInput;
        bool inputError = false;

        if (tables.Count <= 0)
        {
            Console.Clear();
            Console.WriteLine("There are no habit trackers to select.");
            Console.WriteLine("Press any key to create a new tracker or type 0 to exit application.");
            userInput = Console.ReadLine();
            if (userInput == "0") Environment.Exit(0);
            CreateNewHabit();
        }

        Console.WriteLine("Please type the number of the table you would like to view/modify.\n");
        userInput = Console.ReadLine();
        while (!int.TryParse(userInput, out _))
        {
            inputError = true;
            if (inputError)
            {
                userInput = (userInput == "") ? "blank" : userInput;
                Console.Clear();
                ViewAllTables();
                Console.WriteLine($"\n{userInput} is not a valid request, value must be a number. Please try again.");
                inputError = false;
            }
            userInput = Console.ReadLine();
        }

        int tableNumber = Convert.ToInt32(userInput);
        while (tableNumber > tables.Count || tableNumber <= 0)
        {
            inputError = true;
            if (inputError)
            {
                Console.Clear();
                ViewAllTables();
                Console.WriteLine("\nYou have entered a number outside the list provided. Please try again.");
                inputError = false;
            }
            userInput = Console.ReadLine();
            tableNumber = Convert.ToInt32(userInput);
        }
        Console.Clear();
        return tableName = tables[tableNumber - 1];
    }

    private static void ViewAllTables(bool returnToMenu = false)
    {
        List<string> tables = sqlCommands.SqlGetTables();
        int listNumber = 1;

        Console.Clear();

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

    private static void UpdateRecord(string tableName, bool zeroRow = false)
    {
        Console.Clear();
        ViewAllRecords(tableName, false);

        if (zeroRow)
        {
            Console.WriteLine("Invalid Id entered, please try again.");
        }

        string unitName = sqlCommands.SqlGetUnitName(tableName);
        var recordId = GetNumberInput("\nPlease type the Id of the record you want to update or type 0 to return to Main Menu.\n", tableName);

        zeroRow = sqlCommands.SqlUpdateActionCheck(tableName, recordId);

        if (zeroRow)
        {
            UpdateRecord(tableName, true);
        }
        else
        {
            string date = GetDateInput(tableName);
            int quantity = GetNumberInput($"\nPlease insert number of {unitName} or other measure of your choice (no decimals allowed)\n", tableName);
            sqlCommands.SqlUpdateAction(tableName, recordId, date, quantity);
            Console.WriteLine($"The following record Id {recordId} [{date}|{quantity} {unitName}] for {tableName.Replace("_", " ").ToUpper()} has been updated");
            Console.WriteLine("\nPress any key to return to the Main Menu or press 1 to add another entry.");
            string userInput = Console.ReadLine();
            Console.Clear();
            if (userInput == "1") UpdateRecord(tableName);
        }
    }

    private static void DeleteRecord(string tableName)
    {
        Console.Clear();
        ViewAllRecords(tableName, false);

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete or type 0 to return to Main Menu.\n", tableName);
        bool zeroRow = sqlCommands.SqlDeleteAction(tableName, recordId);

        if (zeroRow)
        {
            DeleteRecord(tableName);
        }

        Console.WriteLine($"\nRecord with Id {recordId} was deleted. \n\nPress any key to return to Main Menu.");
        Console.ReadLine();
    }

    private static void InsertRecord(string tableName)
    {
        Console.Clear();

        string date = GetDateInput(tableName);
        string unitName = sqlCommands.SqlGetUnitName(tableName);
        int quantity = GetNumberInput($"\n\nPlease insert number of {unitName} or other measure of your choice (no decimals allowed)\n\n", tableName);

        sqlCommands.SqlInsertAction(tableName, date, quantity);

        Console.WriteLine($"The following [{date}|{quantity} {unitName}] record for {tableName.Replace("_", " ").ToUpper()} has been added");
        Console.WriteLine("\nPress any key to return to the Main Menu or press 1 to add another entry.");
        string userInput = Console.ReadLine();

        Console.Clear();
        if (userInput == "1") InsertRecord(tableName);
    }

    private static int GetNumberInput(string message, string tableName)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserInput(tableName, false);

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvaild number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }

    private static string GetDateInput(string tableName)
    {
        int errorCount = 1;
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu.\n\n");
        string dateInput = Console.ReadLine();
        if (dateInput == "0") GetUserInput(tableName, false);

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            if (errorCount == 3)
            {
                Console.Clear();
                errorCount = 0;
            }
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to Main Menu or try again\n\n");
            errorCount++;
            dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput(tableName, false);
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

        sqlCommands.SqlCreateTable(tableName, unitMeasurement);
        randomData.GenerateRandomData(tableName, false);
        GetUserInput();

    }

    private static void DropTable(string tableName)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete {tableName.Replace("_", " ").ToUpper()}.");
        Console.WriteLine("Type yes to proceed with deleting all data and tables or any other key to return to the Main Menu");
        string userInput = Console.ReadLine().ToLower().Trim();

        if (userInput == "yes")
        {
            Console.Clear();
            Console.WriteLine("You are about to delete all data, this is irrecoverable process.");
            Console.WriteLine("If you are sure Type DROP or any other key to return to the Main Menu.");
            userInput = Console.ReadLine().Trim();
            if (userInput == "DROP")
            {
                sqlCommands.SqlDropTables(tableName);

                Console.WriteLine($"{tableName.Replace("_", " ").ToUpper()} has been removed. Press any key to return to the Main Menu.");
                Console.ReadLine();
                GetUserInput("", true);
            }
        }
    }

    private static void DropAllTables()
    {
        List<string> tables = sqlCommands.SqlGetTables();
        Console.Clear();
        Console.WriteLine($"Would you like to delete all tables and data?");
        Console.WriteLine("Type yes to proceed with deleting all data and tables or any other key to return to the Main Menu.");
        string userInput = Console.ReadLine().ToLower().Trim();

        if (userInput == "yes")
        {
            Console.Clear();
            Console.WriteLine("You are about to delete all data, this is irrecoverable process.");
            Console.WriteLine("If you are sure Type DROP or any other key to return to the Main Menu.");
            userInput = Console.ReadLine().Trim();

            if (userInput == "DROP")
            {
                foreach (string table in tables)
                {
                    sqlCommands.SqlDropTables(table);
                }
                Console.WriteLine("All Tables Dropped. Press any key to return to the Main Menu");
                Console.ReadLine();
                GetUserInput("",true);
            }
        }
    }

}
