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
        var options = InputHelpers.GetMenuOptions<MainMenu>();

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
                    InputHelpers.PressKeyToContinue();
                    break;
                case MainMenu.HabitReports:
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
        var options = InputHelpers.GetMenuOptions<ManageHabitLogs>();

        var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
                    .AddChoices(options.Keys));

        var selectedEnum = options[selectedOption];

        switch (selectedEnum)
        {
            case ManageHabitLogs.AddNewHabitLog:
                HabitLogManager.AddNewHabitLog();
                InputHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.ViewHabitLogs:
                HabitLogManager.ViewHabitLogs();
                InputHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.DeleteHabitLog:
                HabitLogManager.DeleteHabitLog();
                InputHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.UpdateHabitLog:
                HabitLogManager.UpdateHabitLog();
                InputHelpers.PressKeyToContinue();
                break;
            case ManageHabitLogs.BackToMainMenu:
                break;
        }
    }

    public static void DisplayReportMenu()
    {
        var options = InputHelpers.GetMenuOptions<ReportOptions>();

        var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
                    .AddChoices(options.Keys));

        var selectedEnum = options[selectedOption];

        switch (selectedEnum)
        {
            case ReportOptions.DayReport:
                HabitReportManager.GenerateDayReport();
                InputHelpers.PressKeyToContinue();
                break;
            case ReportOptions.TotalLoggedByHabit:
                HabitReportManager.GenerateTotalLogged();
                InputHelpers.PressKeyToContinue();
                break;
            case ReportOptions.BackToMainMenu:
                break;
        }
    }
}
