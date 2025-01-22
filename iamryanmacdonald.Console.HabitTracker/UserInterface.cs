using System.Text.RegularExpressions;
using iamryanmacdonald.Console.HabitTracker.Controllers;
using Spectre.Console;
using static iamryanmacdonald.Console.HabitTracker.Enums;

namespace iamryanmacdonald.Console.HabitTracker;

internal class UserInterface
{
    private readonly Database _database;
    private readonly HabitEntryController _habitEntriesController;
    private readonly HabitController _habitsController;

    internal UserInterface(Database database)
    {
        _database = database;
        _habitEntriesController = new HabitEntryController(database);
        _habitsController = new HabitController(database);
    }

    internal void MainMenu()
    {
        var closeApp = false;
        while (!closeApp)
        {
            System.Console.Clear();

            var actionChoice =
                AnsiConsole.Prompt(new SelectionPrompt<MenuAction>()
                    .Title("What would you like to do?")
                    .UseConverter(a => Regex.Replace(a.ToString(), "(\\B[A-Z])", " $1"))
                    .AddChoices(Enum.GetValues<MenuAction>()));

            switch (actionChoice)
            {
                case MenuAction.ViewHabits:
                    _habitsController.ViewItems();
                    break;
                case MenuAction.NewHabit:
                    _habitsController.InsertItem();
                    break;
                case MenuAction.UpdateHabit:
                    _habitsController.UpdateItem();
                    break;
                case MenuAction.DeleteHabit:
                    _habitsController.DeleteItem();
                    break;
                case MenuAction.NewHabitEntry:
                    _habitEntriesController.InsertItem();
                    break;
                case MenuAction.UpdateHabitEntry:
                    _habitEntriesController.UpdateItem();
                    break;
                case MenuAction.DeleteHabitEntry:
                    _habitEntriesController.DeleteItem();
                    break;
                case MenuAction.ViewStatistics:
                    _habitEntriesController.ViewItems();
                    break;
                case MenuAction.CloseApplication:
                    AnsiConsole.MarkupLine("Goodbye!");
                    closeApp = true;
                    break;
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }
}