using HabitTracker.TruthfulUK.Helpers;
using Spectre.Console;

namespace HabitTracker.TruthfulUK;
internal static class HabitLogManager
{
    internal static List<int> CurrentRowIDCollection = new List<int>();
    internal static string selectedMeasurement = string.Empty;
    internal static void AddNewHabitLog()
    {
        var selectedHabit = InputHelpers.AskForHabitSelection();

        var selectedMeasurement = DbHelpers.SelectMeasurement(selectedHabit);

        var loggedMeasurement = AnsiConsole.Prompt(
            new TextPrompt<double>($"Enter amount ({selectedMeasurement}):"));

        var loggedDate = AnsiConsole.Prompt(
            new TextPrompt<DateOnly>("Enter the date (YYYY-MM-DD) or leave blank for today:").AllowEmpty());

        DbHelpers.AddHabitLog(selectedHabit, loggedMeasurement, loggedDate);

        Console.WriteLine();

        AnsiConsole.MarkupLine($"Habit log for [blue]{selectedHabit} {loggedMeasurement} {selectedMeasurement}[/] was added successfully!");

        Console.WriteLine();
    }

    internal static void ViewHabitLogs()
    {
        var selectedHabit = InputHelpers.AskForHabitSelection();

        List<(int, string, double)> habitLogs = DbHelpers.ViewHabitLogs(selectedHabit);
        selectedMeasurement = DbHelpers.SelectMeasurement(selectedHabit);

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
                $"{InputHelpers.FormatDouble(row.amount)} {selectedMeasurement}",
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

        while (rowIdToDelete < 0)
        {
            AnsiConsole.MarkupLine("[white on blue]Enter [underline]0[/] to Exit without Deleting a Log[/]");

            rowIdToDelete = AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter the [white on blue] ID # [/] of the entry you'd like to delete:"));

            if (rowIdToDelete == 0 || CurrentRowIDCollection.Contains(rowIdToDelete))
            {
                if (rowIdToDelete == 0) break;

                DbHelpers.DeleteHabitLog(rowIdToDelete);

                AnsiConsole.MarkupLine($"Habit log with ID [blue]{rowIdToDelete}[/] was deleted successfully!");

                break;
            }
            else
            {
                InputHelpers.InvalidIDError(rowIdToDelete);
                rowIdToDelete = -1;
            }
        }
    }

    internal static void UpdateHabitLog()
    {
        ViewHabitLogs();

        var rowIdToUpdate = -1;

        while (rowIdToUpdate < 0)
        {
            AnsiConsole.MarkupLine("[white on blue]Enter [underline]0[/] to Exit without Updating a Log[/]");

            rowIdToUpdate = AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter the [white on blue] ID # [/] of the entry you'd like to update:"));

            if (rowIdToUpdate == 0 || CurrentRowIDCollection.Contains(rowIdToUpdate))
            {
                if (rowIdToUpdate == 0) break;

                var newMeasurement = AnsiConsole.Prompt(
                    new TextPrompt<double>($"Enter amount ({selectedMeasurement}):"));

                var newDate = AnsiConsole.Prompt(
                    new TextPrompt<DateOnly>("Enter the date (YYYY-MM-DD) or leave blank for today:").AllowEmpty());

                DbHelpers.UpdateHabitLog(rowIdToUpdate, newMeasurement, newDate);

                AnsiConsole.MarkupLine($"Habit log with ID [blue]{rowIdToUpdate}[/] was updated successfully!");

                break;
            }
            else
            {
                InputHelpers.InvalidIDError(rowIdToUpdate);
                rowIdToUpdate = -1;
            }
        }
    }
}
