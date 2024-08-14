namespace HabitTracker;

internal class CardioTracker
{
    readonly HabitDbHelper dbHelper;

    public CardioTracker()
    {
        dbHelper = new HabitDbHelper();
    }

    /// <summary>
    /// Entry point for running the Habit Tracker from the CLI.
    /// </summary>
    public void StartCli()
    {
        InitializeDatabase();
        Console.WriteLine("Welcome to the Habit Tracker!\n\n");

        while (true)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("\n\tMAIN MENU\n");
            Console.WriteLine("\tWhat would you like to do?\n");
            Console.WriteLine("\tType 0 to Close Application.");
            Console.WriteLine("\tType 1 to View All Records.");
            Console.WriteLine("\tType 2 to Insert Record.");
            Console.WriteLine("\tType 3 to Delete Record.");
            Console.WriteLine("\tType 4 to Update Record.");
            Console.WriteLine("\tType 5 to View Total Number of Days Exercised.");
            Console.WriteLine("\tType 6 to View Total Heart Points.");
            Console.WriteLine("\tType 7 to View Total Heart Points by Year.\n");
            Console.WriteLine("-----------------------------------------------\n");

            string? userInput = Console.ReadLine() ?? "";
            Console.WriteLine();

            // Exit on 0.
            if (userInput.Equals("0"))
            {
                Console.WriteLine("Thank you for using the Habit Tracker.\n");
                return;
            }

            // Handle user selection.
            switch (userInput)
            {
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
                    ViewTotalDays();
                    break;
                case "6":
                    ViewTotalHeartPoints();
                    break;
                case "7":
                    ViewTotalHeartPointsByYear();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }
    private void InitializeDatabase()
    {
        Console.WriteLine("Initializing database...");
        dbHelper.InitializeDB();
        if (dbHelper.IsDbEmpty())
        {
            Console.WriteLine("Populating dummy data...");
            dbHelper.PopulateDB();
        }
        Console.WriteLine("Done.\n\n");
    }

    /// <summary>
    /// Prints all the database entries to the console.
    /// </summary>
    public void ViewAllRecords()
    {
        var heartPoints = dbHelper.GetAllRecords();

        Console.WriteLine("Your cardio activity so far:");

        foreach (var row in heartPoints)
        {
            Console.WriteLine(row.Display());
        }
    }

    /// <summary>
    /// CLI method to insert new cardio entry into the database.
    /// </summary>
    private void InsertRecord()
    {
        Console.WriteLine("Please insert the date (Format: dd-mm-yy):");
        string date = GetDateInput();
        Console.WriteLine("\nPlease insert the number of heart points for the day:");
        int quantity = GetNumberInput();

        dbHelper.Insert(date, quantity);
    }

    /// <summary>
    /// Gets a Date from the user, formatted as dd-MM-yy.
    /// </summary>
    /// <returns>A string representation of the date.</returns>
    internal static string GetDateInput()
    {
        while (true)
        {
            string dateInput = Console.ReadLine() ?? string.Empty;

            if (!dateInput.Equals("")) //TODO validate date input
            {
                return dateInput;
            }

            Console.WriteLine("Invalid date. Please try again.");
        }
    }

    /// <summary>
    /// Gets an integer from the user.
    /// </summary>
    /// <returns>A user-selected integer.</returns>
    internal static int GetNumberInput()
    {
        while (true)
        {
            string numInput = Console.ReadLine() ?? string.Empty;
            if (int.TryParse(numInput, out int number))
            {
                return number;
            }
            Console.WriteLine("Invalid input. Please enter an integer value.");
        }
    }

    /// <summary>
    /// Asks the user for the ID number of a record they wish to delete.
    /// </summary>
    internal void DeleteRecord()
    {
        Console.WriteLine("Enter the ID of the record you would like to delete, or 0 to return to the main menu.");
        int userChoice = GetNumberInput();

        if (userChoice == 0)
        {
            return;
        }

        dbHelper.Delete(userChoice);
    }

    /// <summary>
    /// Allows the user to modify an existing record.
    /// </summary>
    internal void UpdateRecord()
    {
        Console.WriteLine("Enter the ID of the record you would like to update, or 0 to return to the main menu.");
        int userChoice = GetNumberInput();

        if (userChoice == 0)
        {
            return;
        }

        if (dbHelper.TryGetById(userChoice, out var entry))
        {
            Console.WriteLine($"Current data: << {entry!.Display()} >>");
            Console.WriteLine("Please enter the new date (Format: dd-mm-yy):\n");
            string newDate = GetDateInput();
            Console.WriteLine("Please enter the new number of heart points:");
            int newQuantity = GetNumberInput();

            dbHelper.Update(userChoice, newDate, newQuantity);
        }
        else
        {
            Console.WriteLine("Invalid ID. Please try again.");
        }

        return;

    }

    internal void ViewTotalDays()
    {
        Console.WriteLine($"\nTotal days of exercise: {dbHelper.GetTotalDays()}\n");
    }

    internal void ViewTotalHeartPoints()
    {
        Console.WriteLine($"\nTotal heart points: {dbHelper.GetTotalPoints()}\n");
    }

    internal void ViewTotalHeartPointsByYear()
    {
        Console.WriteLine("View total heart points for which year? (Format: yy):");

        int year = GetNumberInput();
        if (year < 0)
        {
            Console.WriteLine("Invalid year. Please use a positive two digit number.");
            year = GetNumberInput();
        }
        Console.WriteLine($"\nTotal heart points for year {year:00}: {dbHelper.GetTotalPoints(year)}\n");
    }
}
