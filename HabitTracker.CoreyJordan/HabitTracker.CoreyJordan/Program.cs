using HabitTrackerLibrary;
using System.Globalization;

string divider = "----------------------------------\n";
SqlCommands.InitializeDB(DataConnection.ConnString);
MainMenu();



void EditEntry()
{
    Console.Clear();
    DisplayEntries(GetEntries());

    int entryId = GetNumberInput("Select the entry you wish to delete or \"X\" to return to Main Menu: ");
    
    try
    {
        if (SqlCommands.RecordExists(entryId))
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\nEnter ounces (integer) or \"X\" to return to Main Menu: ");
            DrinkingWater drink = new()
            {
                Id = entryId,
                Date = DateTime.ParseExact(date, "MM-dd-yy", new CultureInfo("en-US")),
                Quantity = quantity
            };

            SqlCommands.UpdateRecord(drink);

            Console.WriteLine($"\nEntry {entryId} updated successfully.\nHit Enter...\n");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"\nEntry {entryId} does not exist.\nHit Enter...\n");
            Console.ReadLine();
            EditEntry();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nAn unexpected error has occured\n{ex}\nHit Enter...\n");
        Console.ReadLine();
    } 
}

void DeleteEntry()
{
    Console.Clear();
    DisplayEntries(GetEntries());
    int entryId = GetNumberInput("Select the entry you wish to delete or \"X\" to return to Main Menu: ");

    try
    {
        int rowsDeleted = SqlCommands.DeleteRecord(entryId);
        if (rowsDeleted == 0)
        {
            Console.WriteLine("\nEntry does not exist\nHit Enter...\n");
            Console.ReadLine();
            DeleteEntry();
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

void DisplayEntries(List<DrinkingWater> drinks)
{
    Console.Clear();
    if (drinks.Count == 0)
    {
        Console.WriteLine("\nNo entries found.\nHit Enter...\n");
        Console.ReadLine();
    }
    else
    {
        Console.WriteLine(divider);
        foreach (var entry in drinks) 
        {
            Console.WriteLine($"{entry.Id}: {entry.Date:MMM dd, yyyy} - Qty: {entry.Quantity}");
        }
        Console.WriteLine();
        Console.WriteLine(divider);

    }
}

void AddNewEntry()
{
    Console.Clear();
    string date = GetDateInput();    
    int quantity = GetNumberInput("\nEnter ounces (integer) or \"X\" to return to Main Menu: ");
        
    try
    {
        SqlCommands.InsertRecord(date, quantity);
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
   
    if (numberInput.ToLower() == "x")
    {
        Console.Clear();
        MainMenu();
    }
    
    while (!int.TryParse(numberInput, out output) || output < 0)
    {
        Console.Write("Invalid number. Try again: ");
        numberInput = Console.ReadLine()!;
    }
    return output;
}

string GetDateInput()
{
    Console.Write("Enter the date (mm-dd-yy) or \"X\" to return to Main Menu: ");
    string dateInput = Console.ReadLine()!;

    if (dateInput.ToLower() == "x")
    {
        Console.Clear();
        MainMenu();
    }

    while (!DateTime.TryParseExact(dateInput, "MM-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.Write("Invalid format. Format: mm-dd-yy: ");
        dateInput = Console.ReadLine()!;
    }
    return dateInput;
}

void MainMenu()
{
    bool closeApp = false;
    while (!closeApp)
    {

        //Console.Clear();
        Console.WriteLine($"{divider}MAIN MENU\n{divider}");
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("\t0: Exit");
        Console.WriteLine("\t1: View entries");
        Console.WriteLine("\t2: Add new entry");
        Console.WriteLine("\t3: Delete entry");
        Console.WriteLine("\t4: Edit entry");
        Console.WriteLine(divider);

        string choice = Console.ReadLine()!;
        switch (choice)
        {
            case "0":
                Console.WriteLine($"{divider}Goodbye!\n{divider}");
                closeApp = true;
                Environment.Exit(0);
                break;
            case "1":
                DisplayEntries(GetEntries());
                break;
            case "2":
                AddNewEntry();
                break;
            case "3":
                DeleteEntry();
                break;
            case "4":
                EditEntry();
                break;
            default:
                Console.WriteLine("\nInvalid Command. Please try again.\nHit Enter");
                Console.ReadLine();
                break;
        }
    }
}

List<DrinkingWater> GetEntries()
{
    List<DrinkingWater> drinks = new();
    try
    {
       drinks = SqlCommands.GetAllRecords();
    }
    catch (Exception)
    {
        Console.WriteLine("\nError retrieving records\nHit Enter...\n");
        Console.ReadLine();
    }
    return drinks;
}