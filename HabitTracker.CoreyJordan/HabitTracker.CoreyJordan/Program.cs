using HabitTrackerLibrary;
using System.Globalization;

string divider = "----------------------------------\n";
SqlCommands.InitializeDB(DataConnection.ConnString);

bool closeApp = false;
while (!closeApp)
{
    DisplayMainMenu();
    string choice = Console.ReadLine()!;

    switch (choice.ToLower())
    {
        case "q":
            Console.WriteLine($"{divider}Goodbye!\n{divider}");
            closeApp = true;
            break;
        case "1":
            DisplayEntries();
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

void EditEntry()
{
    Console.Clear();
    DisplayEntries();
    var recordId = GetNumberInput("Enter the record ID you wish to edit or \"X\" to return to Main Menu");
    
    if (recordId == -999)
    {
        return;
    }

    try
    {
        SqlCommands.UpdateRecord(recordId);
        Console.WriteLine("Entry update\nHit Enter\n");
        Console.ReadLine();
    }
    catch (Exception)
    {

        Console.WriteLine("Edit failed\nHit Enter\n");
        Console.ReadLine();
    }
}

void DeleteEntry()
{
    Console.Clear();
    DisplayEntries();
}

void DisplayEntries()
{
    Console.Clear();
    try
    {
        List<DrinkingWater> table = SqlCommands.GetAllRecords();
        Console.WriteLine(divider);
        foreach (var entry in table)
        {
            Console.WriteLine($"{entry.Id}: {entry.Date} - Qty: {entry.Quantity}");
        }
        Console.WriteLine(divider);

    }
    catch (Exception)
    {

        Console.WriteLine("No entries found\nHit Enter\n");
        Console.ReadLine();
    }
}

void AddNewEntry()
{
    Console.Clear();
    string date = GetDateInput();
    if (date == "x")
    {
        return;
    }
    
    int quantity = GetNumberInput("\nEnter ounces (integer) or \"X\" to return to Main Menu:");
    if (quantity == -999)
    {
        return;
    }
        
    try
    {
        SqlCommands.InsertRecord(date, quantity);
        Console.WriteLine("Entry added\nHit Enter\n");
        Console.ReadLine();
    }
    catch (Exception)
    {
        Console.WriteLine("Entry failed to insert\nHit Enter\n");
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
        return -999;
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
        return "x";
    }

    while (!DateTime.TryParseExact(dateInput, "MM-dd-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.Write("Invalid format. Format: mm-dd-yy: ");
        dateInput = Console.ReadLine()!;
    }
    return dateInput;
}

void DisplayMainMenu()
{
    Console.Clear();
    Console.WriteLine($"{divider}MAIN MENU\n{divider}");
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("\tQ: Exit");
    Console.WriteLine("\t1: View entries");
    Console.WriteLine("\t2: Add new entry");
    Console.WriteLine("\t3: Delete entry");
    Console.WriteLine("\t4: Edit entry");
    Console.WriteLine(divider);
}