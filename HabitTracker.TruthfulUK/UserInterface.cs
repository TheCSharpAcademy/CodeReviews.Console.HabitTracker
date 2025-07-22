using Microsoft.Data.Sqlite;
using Spectre.Console;
using static HabitTracker.TruthfulUK.Enums;
using static HabitTracker.TruthfulUK.Helpers.UIHelpers;

namespace HabitTracker.TruthfulUK;
internal static class UserInterface
{
    private static bool exitRequested = false;

    public static void DisplayMainMenu()
    {
        var options = GetMenuOptions<MainMenu>();

        while (!exitRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("Welcome to the [bold green]Habit Tracker[/]!");

            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an option:")
                    .AddChoices(options.Keys));

            var selectedEnum = options[selectedOption];

            switch (selectedEnum)
            {
                case MainMenu.ManageHabitLogs:
                    DisplayManageHabitsMenu();
                    break;
                case MainMenu.ManageHabitCategories:
                    AnsiConsole.Write("Nothing here yet, sorry!");
                    Console.ReadKey();
                    break;
                case MainMenu.GenerateReports:
                    AnsiConsole.Write("Nothing here yet, sorry!");
                    Console.ReadKey();
                    break;
                case MainMenu.ExitApplication:
                    SqliteConnection.ClearAllPools();
                    exitRequested = true;
                    break;
            }
        }
    }

    public static void DisplayManageHabitsMenu()
    {
        var options = GetMenuOptions<ManageHabitLogs>();

        var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an option:")
                    .AddChoices(options.Keys));

        var selectedEnum = options[selectedOption];
        
        switch (selectedEnum)
        {
            case ManageHabitLogs.AddNewHabitLog:
                AnsiConsole.Write("Nothing here yet, sorry!");
                Console.ReadKey();
                break;
            case ManageHabitLogs.ViewHabitLogs: 
                AnsiConsole.Write("Nothing here yet, sorry!");
                Console.ReadKey();
                break;
            case ManageHabitLogs.DeleteHabitLog:
                AnsiConsole.Write("Nothing here yet, sorry!");
                Console.ReadKey();
                break;
            case ManageHabitLogs.UpdateHabitLog:
                AnsiConsole.Write("Nothing here yet, sorry!");
                Console.ReadKey();
                break;
            case ManageHabitLogs.BackToMainMenu:
                break;
        }
    }
}
