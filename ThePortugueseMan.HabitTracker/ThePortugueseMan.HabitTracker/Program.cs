using DataBaseLibrary;
using HabitsLibrary;
using System.Globalization;


internal class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    static string habitsTableName = "HabitsTable";
    static DataBaseCommands dbCommands = new();
    static HabitsTable habitsTable = new(habitsTableName, connectionString);
    private static void Main(string[] args)
    {
        //DataBaseCommands dbCommands = new();

        dbCommands.Initialization(habitsTableName);
        
        MainMenu();
        GetUserInput();
    }

    static void MainMenu()
    {
        Console.Clear();
        bool closeApp = false;
        bool invalidCommand = false;
        while (!closeApp)
        {
            Console.Clear();
            Console.WriteLine("HABIT TRACKER");
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View all Habits.");
            Console.WriteLine("Type 2 to Insert a new Habit.");
            Console.WriteLine("Type 3 do Delete a Habit.");
            Console.WriteLine("Type 4 to Update a Habit.");
            Console.WriteLine("----------------------------------------");
            if (invalidCommand)
            {
                Console.Write("Invalid Command. Please choose one of the commands above");
            }
            Console.Write("\n");
            string? commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0": closeApp = true; break;
                case "1": habitsTable.ViewAll(); break;
                case "2": habitsTable.Insert(); break;
                case "3": DeleteScreen(); break;
                case "4": UpdateScreen(); break;
                default:
                    invalidCommand = true;
                    break;
            }
        }
        Console.Clear();
        Console.WriteLine("\nGoodbye!");
        Environment.Exit(0);
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        bool invalidCommand = false;
        while (!closeApp)
        {
            Console.Clear();
            Console.WriteLine("HABIT TRACKER");
            Console.WriteLine("\nHABIT X");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Return to the Main Menu.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 do Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("----------------------------------------");
            if(invalidCommand)
            {
                Console.Write("Invalid Command. Please choose one of the commands above");
            }
            Console.Write("\n");
            string? commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0": closeApp = true; break;
                case "1": ViewAllHabits(""); break;
                case "2": Insert(); break;
                case "3": DeleteScreen(); break;
                case "4": UpdateScreen(); break;
                default:
                    invalidCommand = true;
                    break;
            }
        }
        Console.Clear();
        Console.WriteLine("\nGoodbye!");
        Environment.Exit(0);
    }

    private static void UpdateScreen()
    {
        bool started = false;
        bool success = false;
        int index = 0;

        do
        {
            Console.Clear();
            Console.WriteLine("\nUPDATE");
            dbCommands.ViewAll();

            if (started && !success) Console.WriteLine("\nIndex not valid");
            else if (success) Console.WriteLine($"\nUpdated record {index}");
            else if (!started) Console.WriteLine("\n");

            success = false;
            string? index_str = GetNumberInput(
                "\n\nPlease select write the index of the number you want to update" +
                "\nOr press 0 to return to the menu");
            if (!int.TryParse(index_str, out index)) continue;
            if (index == 0) return;
            else if (!dbCommands.CheckIndex(index)) continue;

            string date = GetDateInput();
            if (date == null) continue;

            string? quantity_str = GetNumberInput(
                "\nPlease insert the number of glasses. Integers only" +
                "\nOr press 0 to return to the menu");

            if (!int.TryParse(quantity_str, out int quantity)) continue;
            if (quantity == 0) return;

            success = dbCommands.UpdateByIndex(index, date, quantity);

            started = true;
        }
        while (true);
    }

    private static void DeleteScreen()
    {
        bool started = false;
        bool success = false;
        int index = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("\nDELETE");
            dbCommands.ViewAll();

            if (started && !success) Console.WriteLine("\nIndex not valid");
            else if (success) Console.WriteLine($"\nDeleted record {index}");
            else if (!started) Console.WriteLine("\n");

            success = false;
            string? index_str = GetNumberInput(
                "\n\nPlease select write the index of the number you want to delete" +
                "\nOr press 0 to return to the menu");
            if (!int.TryParse(index_str, out index)) continue;
            if (index == 0) return;
            else if (!dbCommands.CheckIndex(index)) continue;

            success = dbCommands.DeleteByIndex(index);

            started = true;
        }
        while (true);
    }


    private static void Insert()
    {
        string date = GetDateInput();
        if (date == null) return;

        string? quantity = GetNumberInput("\n\nPlease insert the number of glasses. Integers only");
        if (!int.TryParse(quantity, out int quantity_number)) return;

        dbCommands.Insert(date, quantity_number);
    }

    private static bool GetInsertData(ref string date, ref int quantity)
    {
        date = GetDateInput();
        if (date == null) return false;

        string? quantity_str = GetNumberInput("\n\nPlease insert the number of glasses. " +
            "Integers only");
        if (!int.TryParse(quantity_str, out quantity)) return false;
        
        return true;
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). " +
            "Type 0 to return to main menu");

        string? dateInput = Console.ReadLine();

        while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", 
            new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. Please follow the format dd-mm-yy");
            dateInput = Console.ReadLine();
            if (dateInput == "0") return null;
        }

        return dateInput;
    }

    private static string? GetNumberInput(string message)
    {
        Console.WriteLine(message);
        Console.Write("\n");
        int number;
        string? input;
        input = Console.ReadLine();

        while (!Int32.TryParse(input, out number) || Convert.ToInt32(input) < 0)
        {
            Console.WriteLine("\n\nInvalid input. Please write a positive integer above 0 " +
                "Or 0 to return to the menu");
            input = Console.ReadLine();
            if (input == "0") return null;
        }

        return input;
        
    }

    private static void ViewAllHabits(string db_name)
    {
        /*Console.Clear();
        Console.WriteLine("\nVIEW");
        dbCommands.ViewAll(db_name);
        Console.WriteLine("\n\n\n\nPress any key and ENTER to return to the menu");
        Console.ReadLine();*/
        habitsTable.ViewAll();
        Console.WriteLine("\n\n\n\nPress any key and ENTER to return to the menu");
        Console.ReadLine();
    }
}