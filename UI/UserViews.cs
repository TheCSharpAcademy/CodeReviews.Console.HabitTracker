namespace DotNETConsole.HabitTracker.UI;
using DataModels;
using Spectre.Console;

public class UserViews
{
    public void ViewHabits(List<Habit> habits)
    {
        var table = new Table();
        table.AddColumns(new[] { "Id", "Title" });

        foreach (Habit habit in habits)
        {
            table.AddRow($"{habit.Id}", $"{habit.Title}");
        }

        AnsiConsole.Write(table);
    }
    public void ViewHabitLogs(List<HabitLogView> habitLogs)
    {
        var table = new Table();
        table.AddColumns(new[] { "Habit", "Date", "Quantity" });

        foreach (HabitLogView habit in habitLogs)
        {
            table.AddRow($"{habit.HabitTitle}", $"{habit.EntryDate}", $"{habit.Quantity}");
        }

        AnsiConsole.Write(table);
    }

    public void HabitLogSummary(HabitLogView habitLog)
    {
        AnsiConsole.WriteLine($"Editing == Log_id-{habitLog.LogId}::Habit-{habitLog.HabitTitle}::Quantity::{habitLog.Quantity}::Date-{DateOnly.FromDateTime(habitLog.EntryDate)}");
    }
}
