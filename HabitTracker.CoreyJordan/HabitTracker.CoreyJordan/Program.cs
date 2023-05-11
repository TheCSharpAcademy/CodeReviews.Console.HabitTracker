using HabitTrackerLibrary;
using System.Globalization;

string divider = "----------------------------------\n";
SqlCommands.InitializeUnitTable(DataConnection.ConnString, "unitsOfMeasure");

bool closeApp = false;
while (!closeApp)
{
    MainMenu();
}

void MainMenu()
{
    List<string> habits = SqlCommands.GetTables();

    Console.WriteLine("What would you like to do?");
    Console.WriteLine("\t0: Exit");
    Console.WriteLine("\t1: Open Habit");
    Console.WriteLine("\t2: New Habit");

    string choice = Console.ReadLine()!;
    switch (choice)
    {
        case "0":
            Console.WriteLine($"{divider}Goodbye!\n{divider}");
            closeApp = true;
            Environment.Exit(0);
            break;
        case "1":
            OpenHabit(habits);
            break;
        case "2":
            CreateHabit();
            break;
        default:
            Console.WriteLine("\nInvalid Command. Please try again.\nHit Enter");
            Console.ReadLine();
            Console.Clear();
            break;
    }
}

void DisplayHabits(List<string> habits)
{
    for (int i = 0; i < habits.Count; i++)
    {
        Console.WriteLine($"{i}: {habits[i]}");
    }
}

void EditEntry(string habit)
{
    Console.Clear();
    DisplayEntries(GetEntries(habit));

    int entryId = GetNumberInput("Select the entry you wish to delete or \"X\" to return to Main Menu: ");
    if (entryId == -999)
    {
        Console.Clear();
        return;
    }
    

    try
    {
        if (SqlCommands.RecordExists(entryId, habit))
        {
            string date = GetDateInput();
            if (date == "-999")
            {
                Console.Clear();
                return;
            }

            int quantity = GetNumberInput("\nEnter ounces (integer) or \"X\" to return to Main Menu: ");
            if (quantity == -999)
            {
                Console.Clear();
                return;
            }

            Habit editedHabit = new()
            {
                HabitName = habit,
                Id = entryId,
                Date = DateTime.ParseExact(date, "MM-dd-yy", new CultureInfo("en-US")),
                Quantity = quantity
            };

            SqlCommands.UpdateRecord(editedHabit);

            Console.WriteLine($"\nEntry {entryId} updated successfully.\nHit Enter...\n");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"\nEntry {entryId} does not exist.\nHit Enter...\n");
            Console.ReadLine();
            EditEntry(habit);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nAn unexpected error has occured\n{ex.Message}\nHit Enter...\n");
        Console.ReadLine();
    }
}

void DeleteEntry(string habit)
{
    Console.Clear();
    DisplayEntries(GetEntries(habit));
    int entryId = GetNumberInput("Select the entry you wish to delete or \"X\" to return to Main Menu: ");
    if (entryId == -999)
    {
        Console.Clear();
        return;
    }

    try
    {
        int rowsDeleted = SqlCommands.DeleteRecord(entryId, habit);
        if (rowsDeleted == 0)
        {
            Console.WriteLine("\nEntry does not exist\nHit Enter...\n");
            Console.ReadLine();
            DeleteEntry(habit);
        }
        else
        {
            Console.WriteLine($"\nEntry {entryId} deleted successfully.\nHit Enter...\n ");
            Console.ReadLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"There was an error attempting to delete the entry.\n{ex}\nHit Enter...\n");
        Console.ReadLine();
    }
}

void DisplayEntries(List<Habit> entries)
{
    Console.Clear();

    if (entries.Count == 0)
    {
        Console.WriteLine("\nNo entries found.\nHit Enter...\n");
        Console.ReadLine();
    }
    else
    {
        Console.WriteLine(divider);
        foreach (var entry in entries)
        {
            Console.WriteLine($"{entry.Id}: {entry.Date:MMM dd, yyyy} - Qty: {entry.Quantity} {entry.Unit}");
        }
        Console.WriteLine();
        Console.WriteLine(divider);
    }
}

void AddNewEntry(string habitName)
{
    Console.Clear();

    string date = GetDateInput();
    if (date == "-999")
    {
        Console.Clear();
        return;
    }

    int quantity = GetNumberInput("\nEnter amount (integer) or \"X\" to return to Main Menu: ");
    if (quantity == -999)
    {
        Console.Clear();
        return;
    }

    try
    {
        SqlCommands.InsertRecord(date, quantity, habitName);
        Console.WriteLine("\nEntry added\nHit Enter...\n");
        Console.ReadLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nEntry failed to insert\n{ex}\nHit Enter...\n");
        Console.ReadLine();
    }
}

int GetNumberInput(string prompt)
{
    Console.Write(prompt);
    string numberInput = Console.ReadLine()!;
    int output;


    while (!int.TryParse(numberInput, out output) || output < 0)
    {
        if (numberInput.ToLower() == "x")
        {
            return -999;
        }
        Console.Write("Invalid number. Try again: ");
        numberInput = Console.ReadLine()!;
    }
    return output;
}

string GetDateInput()
{
    Console.Write("Enter the date (mm-dd-yy) or \"X\" to return to Main Menu: ");
    string dateInput = Console.ReadLine()!;


    while (!DateTime.TryParseExact(dateInput, "MM-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        if (dateInput.ToLower() == "x")
        {
            return "-999";
        }
        Console.Write("Invalid format. Format: mm-dd-yy: ");
        dateInput = Console.ReadLine()!;
    }
    return dateInput;
}

void OpenHabit(List<string> habits)
{
    if (habits.Count == 0)
    {
        Console.WriteLine("No habits found\nHit Enter...");
        Console.ReadLine();
        Console.Clear();
        return;
    }

    Console.Clear();
    Console.WriteLine(divider);
    DisplayHabits(habits);
    Console.WriteLine();
    Console.WriteLine(divider);

    int habitChoice = GetNumberInput("Select a habit (or enter x to return to main menu): ");
    if (habitChoice == -999)
    {
        Console.Clear();
        return;
    }

    while (habitChoice < 0 || habitChoice > habits.Count - 1)
    {
        Console.WriteLine("Invalid selection, try again.");
        habitChoice = GetNumberInput("Select a habit (or enter x to return): ");
    }

    string habit = habits[habitChoice];
    Console.Clear();

    bool returnToMain = false;
    while (!returnToMain)
    {
        Console.WriteLine();
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("\t0: Return");
        Console.WriteLine("\t1: View entries");
        Console.WriteLine("\t2: Add new entry");
        Console.WriteLine("\t3: Delete entry");
        Console.WriteLine("\t4: Edit entry");
        Console.WriteLine(divider);

        string choice = Console.ReadLine()!;
        switch (choice.ToLower())
        {
            case "0":
                returnToMain = true;
                Console.Clear();
                break;
            case "1":
                DisplayEntries(GetEntries(habit));
                break;
            case "2":
                AddNewEntry(habit);
                break;
            case "3":
                DeleteEntry(habit);
                break;
            case "4":
                EditEntry(habit);
                break;
            default:
                Console.WriteLine("\nInvalid Command. Please try again.\nHit Enter");
                Console.ReadLine();
                Console.Clear();
                break;
        }
    }
}

void CreateHabit()
{
    Console.Clear();

    Console.Write("Enter a habit name without spaces (or x to return to main menu): ");
    string habitName = GetStringInput();
    if (habitName.ToLower() == "x")
    {
        return;
    }

    Console.Write("Enter a unit of measurement for this habit (or x to return to main): ");
    string unitOfMeasure = GetStringInput();
    if (unitOfMeasure.ToLower() == "x")
    {
        return;
    }

    try
    {
        SqlCommands.InitializeDB(DataConnection.ConnString, habitName);
        SqlCommands.InsertRecord(habitName, unitOfMeasure);
    }
    catch (Exception)
    {
        Console.WriteLine("Habit could not be created\nHit Enter...");
        Console.ReadLine();
    }
}

string GetStringInput()
{
    string input = Console.ReadLine()!;
    while (input == string.Empty || input.Contains(' '))
    {
        Console.Write("Invalid input, try again: ");
        input = Console.ReadLine()!;
    }
    return input;
}

List<Habit> GetEntries(string habitName)
{
    List<Habit> entries = new();
    try
    {
        entries = SqlCommands.GetAllRecords(habitName);
    }
    catch (Exception ex)
    {
        Console.WriteLine("\nError retrieving records\nHit Enter...\n");
        Console.WriteLine(ex.Message);
        Console.ReadLine();
    }
    return entries;
}