using iamryanmacdonald.Console.HabitTracker.Models;
using Spectre.Console;

namespace iamryanmacdonald.Console.HabitTracker.Controllers;

internal class HabitController : IBaseController
{
    private readonly Database _database;

    internal HabitController(Database database)
    {
        _database = database;
    }

    public void DeleteItem()
    {
        var deletableHabits = _database.GetHabits();

        if (deletableHabits.Count == 0)
        {
            AnsiConsole.MarkupLine("No habits found.");
        }
        else
        {
            var habitToDelete =
                AnsiConsole.Prompt(new SelectionPrompt<Habit>().Title("Select a habit to delete:")
                    .UseConverter(h => $"{h.Name} ({h.Unit})").AddChoices(deletableHabits));

            _database.DeleteHabit(habitToDelete.Id);

            AnsiConsole.MarkupLine(
                $"[green]{habitToDelete.Name} ({habitToDelete.Unit}) deleted successfully![/]");
        }
    }

    public void InsertItem()
    {
        var name = AnsiConsole.Ask<string>("Please enter the name of the habit:");
        var unit = AnsiConsole.Ask<string>(
            "Please enter the unit of measurement:");

        var existingHabit = _database.GetHabit(name);
        while (existingHabit != null)
        {
            name = AnsiConsole.Ask<string>("That habit already exists. Please enter a new name for the habit:");
            unit = AnsiConsole.Ask(
                "Please enter the unit of measurement:", unit);

            existingHabit = _database.GetHabit(name);
        }

        _database.InsertHabit(name, unit);

        AnsiConsole.MarkupLine($"[green]{name} ({unit}) added successfully![/]");
    }

    public void UpdateItem()
    {
        var updateableHabits = _database.GetHabits();

        if (updateableHabits.Count == 0)
        {
            AnsiConsole.MarkupLine("No habits found.");
        }
        else
        {
            var habitToUpdate =
                AnsiConsole.Prompt(new SelectionPrompt<Habit>().Title("Select a habit to update:")
                    .UseConverter(h => $"{h.Name} ({h.Unit})").AddChoices(updateableHabits));
            var updatedName = AnsiConsole.Ask("Please enter the name of the habit:", habitToUpdate.Name);
            var updatedUnit = AnsiConsole.Ask(
                "Please enter the unit of measurement:", habitToUpdate.Unit);

            var existingHabit = _database.GetHabit(updatedName);
            while (existingHabit != null && habitToUpdate.Name != updatedName)
            {
                updatedName =
                    AnsiConsole.Ask<string>("That habit already exists. Please enter a new name for the habit:");
                updatedUnit = AnsiConsole.Ask(
                    "Please enter the unit of measurement:", updatedUnit);

                existingHabit = _database.GetHabit(updatedName);
            }

            _database.UpdateHabit(habitToUpdate.Id, updatedName, updatedUnit);

            AnsiConsole.MarkupLine($"[green]{updatedName} ({updatedUnit}) updated successfully![/]");
        }
    }

    public void ViewItems()
    {
        var habits = _database.GetHabits();

        if (habits.Count == 0)
        {
            AnsiConsole.MarkupLine("No habits found.");
        }
        else
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);

            table.AddColumn("Name");
            table.AddColumn("Unit");

            foreach (var habit in habits)
                table.AddRow(habit.Name, habit.Unit);

            AnsiConsole.Write(table);
        }
    }
}