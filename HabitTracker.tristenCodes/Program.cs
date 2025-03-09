using Services;
using Helpers;

DBService dBService = new DBService("Data source=local.db");

string menuSelection;

// TODO: Update an entry - HAVE THIS DISPLAY ALL OCCURENCES FIRST
MenuService.DisplayMainMenu();

menuSelection = Console.ReadLine() ?? "";


switch (menuSelection)
{
    case "1":
        MenuService.AddHabit(dBService);
        break;

    case "2":
        MenuService.UpdateHabit(dBService);
        break;
    case "3":
        // Remove a habit 
        MenuService.RemoveHabit(dBService);
        break;
    case "4":
        // Show all habits
        break;
    default:
        Console.WriteLine("Invalid Entry");
        break;
}

