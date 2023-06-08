class Menu
{
    public static void ShowGreeting()
    {
        Console.WriteLine("Hello there User!\nHow can i be of assisstance?\n");
        Console.WriteLine("Press any key to access the Main menu.\n");
        Console.ReadLine();
        Console.Clear();
    }

    public static void ShowMainMenu()
    {
        string choice = "";
        bool rightChoice = false;

        Console.WriteLine(@"Press A to view habits Menu.
Press B to view system units Menu.
Press C to View entries Menu.
Press X to Exit.");

        while (rightChoice == false)
        {
            choice = Helpers.ValidateStringInput();
            switch (choice.ToUpper())
            {
                case "A":
                    ShowHabitMenu();
                    rightChoice = true;
                    break;
                case "B":
                    ShowUnitMenu();
                    rightChoice = true;
                    break;
                case "C":
                    ShowEntryMenu();
                    rightChoice = true;
                    break;
                case "X":
                    rightChoice = true;
                    break;
                default:
                    Console.WriteLine(@"Wrong choice. Try again please.");
                    break;
            }
        }
    }

    public static void ShowHabitMenu()
    {
        string choice = "";
        bool rightChoice = false;
        int habitId;
        while (rightChoice == false)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Habits Menu.");
            Console.WriteLine("---------------------------");
            Console.WriteLine(@"Press 1 To View Tracked Habits
Press 2 to add a habit.
Press 3 to update a habit.
Press 4 to delete a habit.
Press X to go back to the main menu.");
            choice = Helpers.ValidateStringInput();
            switch (choice.ToUpper())
            {
                case "1":
                    Console.Clear();
                    Habit.DisplayHabits();
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Enter the name of your Habit:");
                    string habitName = Helpers.ValidateStringInput();
                    Console.WriteLine("Choose the ID of the unit to use:");
                    Unit.DisplayUnits();
                    int habitUnit = Helpers.ValidateIntegerInput();
                    while (!Unit.UnitExist(habitUnit))
                    {
                        Console.WriteLine("No Unit with the chosen ID. Choose again.");
                        habitUnit = Convert.ToInt32(Console.ReadLine());
                    }
                    if (Habit.AddHabit(habitName, habitUnit))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Habit inserted successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong while inserting new Habit.");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "3":
                    Console.Clear();
                    string newName;
                    int newUnitId;
                    Console.WriteLine("Which Habit do you want to update. choose an ID");
                    Habit.DisplayHabits();
                    habitId = Helpers.ValidateIntegerInput();
                    while (!Habit.HabitExist(habitId))
                    {
                        Console.WriteLine("No Habit with the chosen ID. Choose again.");
                        habitId = Helpers.ValidateIntegerInput();
                    }
                    Console.WriteLine("Choose a new Habit Name");
                    newName = Helpers.ValidateStringInput();
                    Console.WriteLine("choose a new Unit (type the ID of the unit you want to choose.)");
                    Unit.DisplayUnits();
                    newUnitId = Helpers.ValidateIntegerInput();
                    while (!Unit.UnitExist(newUnitId))
                    {
                        Console.WriteLine("No Unit with the chosen ID. Choose again.");
                        newUnitId = Helpers.ValidateIntegerInput();
                    }
                    if (Habit.UpdateHabit(habitId, newName, newUnitId))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Habit updated successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong while updating the Habit.");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "4":
                    Console.Clear();
                    Habit.DisplayHabits();
                    Console.WriteLine("Which habit do you want to delete. choose an ID (you cannot delete a habit that has entries.)");
                    habitId = Helpers.ValidateIntegerInput();
                    while (!Habit.HabitExist(habitId))
                    {
                        Console.WriteLine("No Habit with the chosen ID. Choose again.");
                        habitId = Helpers.ValidateIntegerInput();
                    }
                    if (Habit.IsHabitTracked(habitId))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Habit cannot be deleted because there are some entries linked to it.");
                        Console.ResetColor();
                    }
                    else
                    {
                        if (Habit.DeleteHabit(habitId))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Habit Deleted successfully.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Something went wrong while deleting the habit.");
                            Console.ResetColor();
                        }
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "X":
                    rightChoice = true;
                    break;
                default:
                    Console.WriteLine("Wrong choice.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadLine();
                    break;
            }
        }
        Console.Clear();
        Menu.ShowMainMenu();
    }

    public static void ShowUnitMenu()
    {
        int unitId;

        string choice = "";
        bool rightChoice = false;
        while (rightChoice == false)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Units Menu.");
            Console.WriteLine("---------------------------");
            Console.WriteLine(@"Press 1 To View System Units
Press 2 to add a unit.
Press 3 to update a unit.
Press 4 to delete a unit.
Press X to go back to the main menu.");
            choice = Helpers.ValidateStringInput();
            switch (choice.ToUpper())
            {
                case "1":
                    Console.Clear();
                    Unit.DisplayUnits();
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Enter the name of your Unit:");
                    string UnitName = Helpers.ValidateStringInput();
                    Console.WriteLine("Choose the symbol of your unit:");
                    string UnitSymbol = Helpers.ValidateStringInput();
                    if (Unit.AddUnit(UnitName, UnitSymbol))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Unit inserted successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong while inserting new Unit.");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "3":
                    Console.Clear();
                    string newName, newSym;
                    Console.WriteLine("Which Unit do you want to update. choose an ID");
                    Unit.DisplayUnits();
                    unitId = Helpers.ValidateIntegerInput();
                    while (!Unit.UnitExist(unitId))
                    {
                        Console.WriteLine("No Unit with the chosen ID. Choose again.");
                        unitId = Helpers.ValidateIntegerInput();
                    }
                    Console.WriteLine("Choose a new Unit Name");
                    newName = Helpers.ValidateStringInput();
                    Console.WriteLine("choose a new Unit Symbol");
                    newSym = Console.ReadLine();
                    if (Unit.UpdateUnit(unitId, newName, newSym))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Unit updated successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong while updating the Unit.");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "4":
                    Console.Clear();
                    Unit.DisplayUnits();
                    Console.WriteLine("Which unit do you want to delete. choose an ID (please you cannot delete a used unit.)");
                    unitId = Helpers.ValidateIntegerInput();
                    while (!Unit.UnitExist(unitId))
                    {
                        Console.WriteLine("No Unit with the chosen ID. Choose again.");
                        unitId = Helpers.ValidateIntegerInput();
                    }
                    if (Unit.IsUnitUsed(unitId))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Unit cannot be deleted because it is used.");
                        Console.ResetColor();
                    }
                    else
                    {
                        if (Unit.DeleteUnit(unitId))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Unit Deleted successfully.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Something went wrong while deleting the unit.");
                            Console.ResetColor();
                        }
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "X":
                    rightChoice = true;
                    break;
                default:
                    Console.WriteLine("Wrong choice.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadLine();
                    break;
            }
        }
        Console.Clear();
        Menu.ShowMainMenu();
    }

    public static void ShowEntryMenu()
    {
        string choice = "";
        bool rightChoice = false;

        int habitId, entryId;
        while (rightChoice == false)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Entries Menu.");
            Console.WriteLine("---------------------------");
            Console.WriteLine(@"Press 1 To View Tracked Habits entries
Press 2 to view a summary of Tracked habits
Press 3 to add an entry.
Press 4 to update an entry.
Press 5 to delete an entry.
Press X to go back to the main menu.");
            choice = Helpers.ValidateStringInput();
            switch (choice.ToUpper())
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Which Habit do you want to View?");
                    Habit.DisplayHabits();
                    habitId = Helpers.ValidateIntegerInput();
                    while (!Habit.HabitExist(habitId))
                    {
                        Console.WriteLine("No Habit with the chosen ID. Choose again.");
                        habitId = Helpers.ValidateIntegerInput();
                    }
                    Habit.DisplaySingleHabit(habitId);
                    Console.WriteLine("----------------------------------");
                    Entry.DisplayEntries(habitId);
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Entries Per Month");
                    Entry.DisplayReportPerMonth();
                    Console.WriteLine("Entries Per Year");
                    Entry.DisplayReportPerYear();
                    Console.WriteLine("Entries Per Habit (toatal)");
                    Entry.DisplayEntriesSummary();
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "3":
                    int quantity;
                    Console.Clear();
                    Console.WriteLine("Which Habit do you want to log?");
                    Habit.DisplayHabits();
                    habitId = Helpers.ValidateIntegerInput();
                    while (!Habit.HabitExist(habitId))
                    {
                        Console.WriteLine("No Habit with the chosen ID. Choose again.");
                        habitId = Helpers.ValidateIntegerInput();
                    }
                    Console.WriteLine("Enter Quantity:");
                    quantity = Helpers.ValidateIntegerInput();
                    if (Entry.AddEntry(habitId, quantity))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Log inserted successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong while inserting data");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Which Habit do you want to Update?");
                    Habit.DisplayHabits();
                    habitId = Helpers.ValidateIntegerInput();
                    while (!Habit.HabitExist(habitId))
                    {
                        Console.WriteLine("No Habit with the chosen ID. Choose again.");
                        habitId = Helpers.ValidateIntegerInput();
                    }
                    Console.WriteLine("choose the entry you want to update (choose entry ID)");
                    Entry.DisplayEntriesDetailed(habitId);
                    entryId = Helpers.ValidateIntegerInput();
                    while (!Entry.EntryExist(entryId))
                    {
                        Console.WriteLine("No Habit with the chosen ID. Choose again.");
                        entryId = Helpers.ValidateIntegerInput();
                    }
                    Console.WriteLine("Enter Quantity:");
                    quantity = Helpers.ValidateIntegerInput();
                    if (Entry.UpdateEntry(entryId, quantity))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Log updated successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong while updating data");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Which Habit do you want to delete entries from?");
                    Habit.DisplayHabits();
                    habitId = Helpers.ValidateIntegerInput();
                    while (!Habit.HabitExist(habitId))
                    {
                        Console.WriteLine("No Enty with the chosen ID. Choose again.");
                        habitId = Helpers.ValidateIntegerInput();
                    }
                    Console.WriteLine("choose the entry you want to delete (choose entry ID)");
                    Entry.DisplayEntriesDetailed(habitId);
                    entryId = Helpers.ValidateIntegerInput();
                    while (!Entry.EntryExist(entryId))
                    {
                        Console.WriteLine("No Enty with the chosen ID. Choose again.");
                        entryId = Helpers.ValidateIntegerInput();
                    }
                    if (Entry.DeleteEntry(entryId))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Log deleted successfully.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong while deleting data");
                        Console.ResetColor();
                    }
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadLine();
                    break;
                case "X":
                    rightChoice = true;
                    break;
                default:
                    Console.WriteLine("Wrong choice.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadLine();
                    break;
            }
        }
        Console.Clear();
        Menu.ShowMainMenu();
    }
}