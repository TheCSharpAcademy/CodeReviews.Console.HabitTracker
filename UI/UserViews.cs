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
    public void ViewHabitLogs(List<HabitLog> habitLogs)
    {
        var table = new Table();
        table.AddColumns(new[] { "Id", "Date", "Quentity", "Habit ID" });

        foreach (HabitLog habit in habitLogs)
        {
            table.AddRow($"{habit.Id}", $"{habit.LogDate}", $"{habit.Quantity}", $"{habit.HabitId}");
        }

        AnsiConsole.Write(table);
    }
}
