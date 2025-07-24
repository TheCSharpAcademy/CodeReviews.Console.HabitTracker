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

            var rule = new Rule().RuleStyle("blue dim");
            AnsiConsole.Write(rule);

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
                case MainMenu.AddHabit:
                    HabitManager.AddNewHabit();
                    UIHelpers.PressKeyToContinue();
                    break;
                case MainMenu.HabitReport:
                    DisplayReportMenu();
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
                HabitLogManager.AddNewHabitLog();
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.ViewHabitLogs:
                HabitLogManager.ViewHabitLogs();
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.DeleteHabitLog:
                HabitLogManager.DeleteHabitLog();
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.UpdateHabitLog:
                HabitLogManager.UpdateHabitLog();
                UIHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.BackToMainMenu:
                break;
        }
    }

    public static void DisplayReportMenu()
    {
        var options = UIHelpers.GetMenuOptions<ReportOptions>();

        var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
                    .AddChoices(options.Keys));

        var selectedEnum = options[selectedOption];

        switch (selectedEnum)
        {
            case ReportOptions.DayReport:
                AnsiConsole.Write("Nothing here yet, sorry!");
                Console.ReadKey();
                break;
            case ReportOptions.DateRangeReport:
                AnsiConsole.Write("Nothing here yet, sorry!");
                Console.ReadKey();
                break;
            case ReportOptions.BackToMainMenu:
                break;
        }
    }
}
