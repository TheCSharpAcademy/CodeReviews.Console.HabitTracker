using HabitTracker.tristenCodes.Services;
using HabitTracker.tristenCodes.Helpers;
using Microsoft.Data.Sqlite;

DBService dbService = new DBService("Data source=local.db");

string menuSelection;

while (true)
{
    Console.Clear();
    MenuService.DisplayMainMenu();

    menuSelection = Console.ReadLine() ?? "";

    switch (menuSelection)
    {
        case "1":
            Console.Clear();
            MenuService.AddHabit(dbService);
            break;
        case "2":
            Console.Clear();
            MenuService.UpdateHabit(dbService);
            break;
        case "3":
            Console.Clear();
            MenuService.RemoveHabit(dbService);
            break;
        case "4":
            Console.Clear();
            ReaderUtils.DisplayRows(dbService);
            break;
        case "5":
            Console.WriteLine("Exiting...");
            return;
        default:
            Console.Clear();
            Console.WriteLine("Invalid Entry.");
            break;
    }
}

