using Spectre.Console;
using mrgee1978.HabitLogger.Controllers;
using mrgee1978.HabitLogger.Models.Habits;
using mrgee1978.HabitLogger.Models.Records;

namespace mrgee1978.HabitLogger.Views;

// This class is responsible for presenting the habits 
// and records contained in the database to the user
public class ViewData
{
    private HabitController _habitController = new HabitController();
    private RecordController _recordController = new RecordController();

    /// <summary>
    /// Using Spectre Console displays all habits as a table of information
    /// </summary>
    public void ViewHabits()
    {
        List<Habit> habits = _habitController.GetHabits();
        if (habits.Count == 0)
        {
            AnsiConsole.Markup("[red]No habits added yet.\nNothing to view.[/]\n");
            return;
        }

        var table = new Table();
        table.AddColumn("[green]Id[/]");
        table.AddColumn("[green]Habit Name[/]");
        table.AddColumn("[green]Description[/]");

        foreach (var habit in habits)
        {
            table.AddRow(habit.Id.ToString(), habit.Name, habit.Description);
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Using Spectre Console displays all records as a table of information
    /// </summary>
    public void ViewRecords()
    {
        List<Record> records = _recordController.GetRecords();
        if (records.Count == 0)
        {
            AnsiConsole.Markup("[red]No records added yet.\nNothing to view.[/]\n");
            return;
        }

        var table = new Table();
        table.AddColumn("[green]Id[/]");
        table.AddColumn("[green]Date[/]");
        table.AddColumn("[green]Quantity[/]");
        table.AddColumn("[green]Habit Id[/]");

        foreach (var record in records)
        {
            table.AddRow(record.Id.ToString(), record.Date.ToString(), record.Quantity.ToString(),
                record.HabitId.ToString());
        }

        AnsiConsole.Write(table);
    }
}
