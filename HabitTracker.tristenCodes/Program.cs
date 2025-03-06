using Services;
using Helpers;

DBService dBService = new DBService("Data source=local.db");

string menuSelection;

// TODO: Update an entry - HAVE THIS DISPLAY ALL OCCURENCES FIRST
MenuHelper.DisplayMainMenu();

menuSelection = Console.ReadLine() ?? "";


switch (menuSelection)
{
    case "1":
        MenuHelper.AddHabit(dBService);
        break;

    case "2":
        MenuHelper.UpdateEntry(dBService);
        break;
    case "3":
        // Remove a habit 
        break;
    case "4":
        // Show a habits
        break;
    default:
        Console.WriteLine("Invalid Entry");
        break;
}

