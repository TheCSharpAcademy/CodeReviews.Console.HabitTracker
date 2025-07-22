using HabitTracker.TruthfulUK.Helpers;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
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

    public static void ContinuePrompt()
    {
        AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
        Console.ReadKey();
    }
    public static void DisplayManageHabitsMenu()
    {
        var options = GetMenuOptions<ManageHabitLogs>();

        var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
                    .AddChoices(options.Keys));

        var selectedEnum = options[selectedOption];
        var selectedHabit = string.Empty;
        var selectedMeasurement = string.Empty;

        switch (selectedEnum)
        {
            case ManageHabitLogs.AddNewHabitLog:

                selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
                    .AddChoices(DB_Helpers.SelectHabits()));

                selectedMeasurement = DB_Helpers.SelectMeasurement(selectedHabit);

                var loggedMeasurement = AnsiConsole.Prompt(
                    new TextPrompt<int>($"Enter amount ({selectedMeasurement}):"));

                var loggedDate = AnsiConsole.Prompt(
                    new TextPrompt<DateOnly>("Enter the date (YYYY-MM-DD) or leave blank for today:").AllowEmpty());

                DB_Helpers.AddHabitLog(selectedHabit, loggedMeasurement, loggedDate);

                Console.WriteLine();
                AnsiConsole.MarkupLine($"[green]Habit log for {selectedHabit} added successfully![/]");
                Console.WriteLine();

                ContinuePrompt();
                break;
            case ManageHabitLogs.ViewHabitLogs:

                selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an [blue]option[/]:")
                    .AddChoices(DB_Helpers.SelectHabits()));

                DB_Helpers.ViewHabitLogs(selectedHabit);

                List<(string, string)> habitLogs = DB_Helpers.ViewHabitLogs(selectedHabit);
                selectedMeasurement = DB_Helpers.SelectMeasurement(selectedHabit);

                var habitLogsTable = new Table();
                habitLogsTable
                    .AddColumn("[white on blue] Habit [/]")
                    .AddColumn("[white on blue] Amount [/]")
                    .AddColumn("[white on blue] Date [/]");

                foreach ((string, string) log in habitLogs)
                {
                    habitLogsTable.AddRow(selectedHabit, $"{log.Item2} {selectedMeasurement}", log.Item1);
                    
                }

                habitLogsTable
                    .Border(TableBorder.Minimal)
                    .Expand();

                AnsiConsole.Write(habitLogsTable);

                ContinuePrompt();

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
