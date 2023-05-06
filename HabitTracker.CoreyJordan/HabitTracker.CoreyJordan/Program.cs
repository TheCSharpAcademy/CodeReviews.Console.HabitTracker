using HabitTrackerLibrary;
using System.Globalization;

string divider = "----------------------------------\n";
SqlCommands.InitializeDB(DataConnection.ConnString);
MainMenu();



void EditEntry()
{
    //Console.Clear();
    //DisplayEntries();
    //var recordId = GetNumberInput("Enter the record ID you wish to edit or \"X\" to return to Main Menu");
    
    //if (recordId == -999)
    //{
    //    return;
    //}

    //Console.WriteLine("Choose field to edit:");
    //Console.WriteLine("\t1: Date");
    //Console.WriteLine("\t2: Quantity");
    //Console.WriteLine("\t3: Both");
    //Console.WriteLine("\tX: Return");
    //string editChoice = Console.ReadLine()!;

    //switch (editChoice.ToLower())
    //{
    //    case "x":
    //        return;
    //    case "1":
    //        break;
    //    case "2":
    //        break;
    //    case "3":
    //        break;
    //    default:
    //        Console.WriteLine("Invalid Choice.\n Hit Enter.\n");
    //        Console.ReadLine();
    //        EditEntry();
    //        return;
    //}

    //try
    //{
    //    SqlCommands.UpdateRecord(recordId);
    //    Console.WriteLine("Entry update\nHit Enter\n");
    //    Console.ReadLine();
    //}
    //catch (Exception)
    //{
    //    Console.WriteLine("Edit failed\nHit Enter\n");
    //    Console.ReadLine();
    //}
}

void DeleteEntry()
{
    //Console.Clear();
    //DisplayEntries();
}

void DisplayEntries(List<DrinkingWater> drinks)
{
    Console.Clear();
    if (drinks.Count == 0)
    {
        Console.WriteLine("\nNo records found.\nHit Enter...\n");
        Console.ReadLine();
    }
    else
    {
        Console.WriteLine(divider);
        foreach (var entry in drinks) 
        {
            Console.WriteLine($"{entry.Id}: {entry.Date:MMM dd, yyyy} - Qty: {entry.Quantity}");
        }
        Console.WriteLine(divider);

    }
}

void AddNewEntry()
{
    Console.Clear();
    string date = GetDateInput();    
    int quantity = GetNumberInput("\nEnter ounces (integer) or \"X\" to return to Main Menu:");
        
    try
    {
        SqlCommands.InsertRecord(date, quantity);
        Console.WriteLine("\nEntry added\nHit Enter...\n");
        Console.ReadLine();
    }
    catch (Exception)
    {
        Console.WriteLine("\nEntry failed to insert\nHit Enter...\n");
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