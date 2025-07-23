using HabitTracker.TruthfulUK.Helpers;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using static HabitTracker.TruthfulUK.Enums;

namespace HabitTracker.TruthfulUK;
internal static class UserInterface
{
    private static bool exitRequested = false;

    public static void DisplayMainMenu()
    {
        var options = UIHelpers.GetMenuOptions<MainMenu>();

        while (!exitRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("Habit Tracker")
                    .LeftJustified()
                    .Color(Color.Blue));

            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
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
        var options = UIHelpers.GetMenuOptions<ManageHabitLogs>();

        var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
                    .AddChoices(options.Keys));

        var selectedEnum = options[selectedOption];

        switch (selectedEnum)
        {
            case ManageHabitLogs.AddNewHabitLog:
                HabitsManager.AddNewHabitLog();
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.ViewHabitLogs:
                HabitsManager.ViewHabitLogs();
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.DeleteHabitLog:
                HabitsManager.DeleteHabitLog();
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.UpdateHabitLog:
                AnsiConsole.Write("Nothing here yet, sorry!");
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.BackToMainMenu:
                break;
        }
    }
}
