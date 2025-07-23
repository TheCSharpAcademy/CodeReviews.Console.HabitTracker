using HabitTracker.TruthfulUK.Helpers;
using Spectre.Console;

namespace HabitTracker.TruthfulUK;
internal static class HabitsManager
{
    internal static List<int> CurrentRowIDCollection = new List<int>();
    internal static void AddNewHabitLog() 
    {
        var selectedHabit = UIHelpers.AskForHabitSelection();

        var selectedMeasurement = DB_Helpers.SelectMeasurement(selectedHabit);

        var loggedMeasurement = AnsiConsole.Prompt(
            new TextPrompt<double>($"Enter amount ({selectedMeasurement}):"));

        var loggedDate = AnsiConsole.Prompt(
            new TextPrompt<DateOnly>("Enter the date (YYYY-MM-DD) or leave blank for today:").AllowEmpty());

        DB_Helpers.AddHabitLog(selectedHabit, loggedMeasurement, loggedDate);

        Console.WriteLine();

        AnsiConsole.MarkupLine($"Habit log for [blue]{selectedHabit} {loggedMeasurement} {selectedMeasurement}[/] was added successfully!");

        Console.WriteLine();
    }

    internal static void ViewHabitLogs() 
    {
        var selectedHabit = UIHelpers.AskForHabitSelection();

        DB_Helpers.ViewHabitLogs(selectedHabit);

        List<(int, string, double)> habitLogs = DB_Helpers.ViewHabitLogs(selectedHabit);
        var selectedMeasurement = DB_Helpers.SelectMeasurement(selectedHabit);

        var habitLogsTable = new Table();
        habitLogsTable
            .AddColumn("[white on blue] ID # [/]")
            .AddColumn("[white on blue] Habit [/]")
            .AddColumn("[white on blue] Amount [/]")
            .AddColumn("[white on blue] Date [/]");

        CurrentRowIDCollection.Clear();

        foreach ((int id, string date, double amount) row in habitLogs)
        {
            CurrentRowIDCollection.Add(row.id);
            habitLogsTable.AddRow(
                $"{row.id}",
                selectedHabit,
                $"{UIHelpers.FormatDouble(row.amount)} {selectedMeasurement}",
                row.date
            );
        }

        habitLogsTable
            .ShowRowSeparators()
            .Border(TableBorder.Horizontal)
            .Expand();

        AnsiConsole.Write(habitLogsTable);
    }

    internal static void DeleteHabitLog() 
    {
        ViewHabitLogs();

        var rowIdToDelete = -1;
        var paddedErrorText = 
            new Text("Invalid ID #. Please try again.", 
            new Style(Color.Red));
        var paddedError = new Padder(paddedErrorText).PadTop(2).PadBottom(2).PadLeft(0);

        while (rowIdToDelete < 0)
        {
            AnsiConsole.MarkupLine("[white on blue]Enter [underline]0[/] to Exit without Deleting a Log[/]");
            
            rowIdToDelete = AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter the [white on blue] ID # [/] of the entry you'd like to delete:"));

            if (rowIdToDelete == 0 || CurrentRowIDCollection.Contains(rowIdToDelete))
            {
                if (rowIdToDelete == 0) return;

                DB_Helpers.DeleteHabitLog(rowIdToDelete);

                AnsiConsole.MarkupLine($"Habit log with ID [blue]{rowIdToDelete}[/] was deleted successfully!");

                return;
            }
            else
            {
                AnsiConsole.Write(paddedError);
                rowIdToDelete = -1;
            }
        }


    }
}
