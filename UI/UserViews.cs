namespace DotNETConsole.HabitTracker.UI;
using DataModels;
using Spectre.Console;

public class UserViews
{
    public void ViewHabits(List<Habit> habits)
    {
        var table = new Table();
        table.AddColumns(new[] { "Id", "Title", "Unit" });

        foreach (Habit habit in habits)
        {
            table.AddRow($"{habit.Id}", $"{habit.Title}", $"{habit.Unit}");
        }

        AnsiConsole.Write(table);
    }
    public void ViewHabitLogs(List<HabitLogView> habitLogs)
    {
        var table = new Table();
        table.AddColumns(new[] { "Habit", "Date", "Quantity" });

        foreach (HabitLogView habit in habitLogs)
        {
            table.AddRow($"{habit.HabitTitle}", $"{habit.EntryDate}", $"{habit.Quantity} {habit.Unit}");
        }

        AnsiConsole.Write(table);
    }

    public void HabitLogSummary(HabitLogView habitLog, string actionType = "Editing")
    {
        AnsiConsole.Write(new Align(new Markup($"[bold blue]{actionType}[/]"), HorizontalAlignment.Center, VerticalAlignment.Top));
        AnsiConsole.Write(new Align(new Markup($"{habitLog.HabitTitle}[yellow]({DateOnly.FromDateTime(habitLog.EntryDate)})[/] - [blue bold]Q:-{habitLog.Quantity} {habitLog.Unit}[/]"), HorizontalAlignment.Center, VerticalAlignment.Top));
        Console.WriteLine();
    }

    public void HabitSummary(Habit habit, string actionType = "Editing")
    {
        AnsiConsole.Write(new Align(new Markup($"[bold blue]{actionType}[/]"), HorizontalAlignment.Center, VerticalAlignment.Top));
        AnsiConsole.Write(new Align(new Markup($"[yellow]{habit.Title}[/] Unit: {habit.Unit}"), HorizontalAlignment.Center, VerticalAlignment.Top));
        Console.WriteLine();
    }
}
