using iamryanmacdonald.Console.HabitTracker.Models;
using Spectre.Console;

namespace iamryanmacdonald.Console.HabitTracker.Controllers;

public class HabitEntryController : IBaseController
{
    private readonly Database _database;

    internal HabitEntryController(Database database)
    {
        _database = database;
    }

    public void DeleteItem()
    {
        var habits = _database.GetHabits();

        if (habits.Count == 0)
        {
            AnsiConsole.MarkupLine("No habits found.");
        }
        else
        {
            var habitToDeleteEntryFor = AnsiConsole.Prompt(new SelectionPrompt<Habit>()
                .Title("Select a habit to delete an entry for:")
                .UseConverter(h => $"{h.Name} ({h.Unit})").AddChoices(habits));

            var deletableHabitEntries = _database.GetHabitEntries(habitToDeleteEntryFor.Id);

            if (deletableHabitEntries.Count == 0)
            {
                AnsiConsole.MarkupLine($"No entries for {habitToDeleteEntryFor.Name} found.");
            }
            else
            {
                var habitEntryToDelete =
                    AnsiConsole.Prompt(new SelectionPrompt<HabitEntry>().Title("Select an entry to delete:")
                        .UseConverter(he => $"{he.Date:MMMM d, yyyy} - {he.Quantity} {habitToDeleteEntryFor.Unit}")
                        .AddChoices(deletableHabitEntries));

                _database.DeleteHabitEntry(habitEntryToDelete.Id);

                AnsiConsole.MarkupLine(
                    $"[green]Entry for {habitToDeleteEntryFor.Name} on {habitEntryToDelete.Date:MMMM d, yyyy} deleted successfully![/]");
            }
        }
    }

    public void InsertItem()
    {
        var habits = _database.GetHabits();

        if (habits.Count == 0)
        {
            AnsiConsole.MarkupLine("No habits found.");
        }
        else
        {
            var habitToAddTo = AnsiConsole.Prompt(new SelectionPrompt<Habit>()
                .Title("Select a habit to add an entry for:")
                .UseConverter(h => $"{h.Name} ({h.Unit})").AddChoices(habits));

            AnsiConsole.MarkupLine($"Entering entry for {habitToAddTo.Name}");

            var date = AnsiConsole.Ask<DateOnly>("Please enter the date: (Format: yyyy-mm-dd)");
            var quantity =
                AnsiConsole.Ask<int>(
                    $"Please enter the quantity (in {habitToAddTo.Unit})");

            var existingHabitEntry = _database.GetHabitEntry(date, habitToAddTo.Id);
            while (existingHabitEntry != null)
            {
                date = AnsiConsole.Ask<DateOnly>(
                    "An entry already exists for that date. Please enter a new date: (Format: yyyy-mm-dd)");
                quantity =
                    AnsiConsole.Ask(
                        $"Please enter the quantity (in {habitToAddTo.Unit})", quantity);

                existingHabitEntry = _database.GetHabitEntry(date, habitToAddTo.Id);
            }

            _database.InsertHabitEntry(date, habitToAddTo.Id, quantity);

            AnsiConsole.MarkupLine(
                $"Successfully inserted an entry for {habitToAddTo.Name} on {date:MMMM d, yyyy} with a value of {quantity} {habitToAddTo.Unit}.");
        }
    }

    public void UpdateItem()
    {
        var habits = _database.GetHabits();

        if (habits.Count == 0)
        {
            AnsiConsole.MarkupLine("No habits found.");
        }
        else
        {
            var habitToUpdateEntryFor = AnsiConsole.Prompt(new SelectionPrompt<Habit>()
                .Title("Select a habit to update an entry for:")
                .UseConverter(h => $"{h.Name} ({h.Unit})").AddChoices(habits));

            var updateableHabitEntries = _database.GetHabitEntries(habitToUpdateEntryFor.Id);

            if (updateableHabitEntries.Count == 0)
            {
                AnsiConsole.MarkupLine($"No entries for {habitToUpdateEntryFor.Name} found.");
            }
            else
            {
                AnsiConsole.MarkupLine($"Updating entry for {habitToUpdateEntryFor.Name}");

                var habitEntryToUpdate =
                    AnsiConsole.Prompt(new SelectionPrompt<HabitEntry>().Title("Select an entry to update:")
                        .UseConverter(he => $"{he.Date:MMMM d, yyyy} - {he.Quantity} {habitToUpdateEntryFor.Unit}")
                        .AddChoices(updateableHabitEntries));
                var updatedDate =
                    AnsiConsole.Ask("Please enter the date: (Format: yyyy-mm-dd)", habitEntryToUpdate.Date);
                var updatedQuantity =
                    AnsiConsole.Ask(
                        $"Please enter the quantity (in {habitToUpdateEntryFor.Unit})", habitEntryToUpdate.Quantity);

                var existingHabitEntry = _database.GetHabitEntry(updatedDate, habitToUpdateEntryFor.Id);
                while (existingHabitEntry != null && habitEntryToUpdate.Date != updatedDate)
                {
                    updatedDate =
                        AnsiConsole.Ask(
                            "An entry for that date already exists. Please enter the date: (Format: yyyy-mm-dd)",
                            habitEntryToUpdate.Date);
                    updatedQuantity =
                        AnsiConsole.Ask(
                            $"Please enter the quantity (in {habitToUpdateEntryFor.Unit})",
                            updatedQuantity);

                    existingHabitEntry = _database.GetHabitEntry(updatedDate, habitToUpdateEntryFor.Id);
                }

                _database.UpdateHabitEntry(habitEntryToUpdate.Id, updatedDate, updatedQuantity);

                AnsiConsole.MarkupLine(
                    $"[green]Entry for {habitToUpdateEntryFor.Name} on {updatedDate} updated successfully![/]");
            }
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
            var now = DateTime.Now;
            var startOfWeek = DateOnly.FromDateTime(now.AddDays(-(int)now.DayOfWeek + 1));
            var endOfWeek = DateOnly.FromDateTime(now.AddDays(-(int)now.DayOfWeek + 7));

            var habitStatisticsCountMap = new Dictionary<int, Dictionary<string, int>>();
            var habitStatisticsQuantityMap = new Dictionary<int, Dictionary<string, int>>();

            foreach (var habit in habits)
            {
                var habitEntryStatisticsCount = new Dictionary<string, int>
                {
                    { "week", 0 },
                    { "month", 0 },
                    { "year", 0 }
                };
                var habitEntryStatisticsQuantity = new Dictionary<string, int>
                {
                    { "week", 0 },
                    { "month", 0 },
                    { "year", 0 }
                };

                var habitEntries = _database.GetHabitEntries(habit.Id);

                foreach (var habitEntry in habitEntries)
                {
                    if (habitEntry.Date.Year == now.Year && habitEntry.Date.DayOfYear >= startOfWeek.DayOfYear &&
                        habitEntry.Date.DayOfYear <= endOfWeek.DayOfYear)
                    {
                        habitEntryStatisticsCount["week"] += 1;
                        habitEntryStatisticsQuantity["week"] += habitEntry.Quantity;
                    }

                    if (habitEntry.Date.Year == now.Year && habitEntry.Date.Month == now.Month)
                    {
                        habitEntryStatisticsCount["month"] += 1;
                        habitEntryStatisticsQuantity["month"] += habitEntry.Quantity;
                    }

                    if (habitEntry.Date.Year == now.Year)
                    {
                        habitEntryStatisticsCount["year"] += 1;
                        habitEntryStatisticsQuantity["year"] += habitEntry.Quantity;
                    }
                }

                habitStatisticsCountMap.Add(habit.Id, habitEntryStatisticsCount);
                habitStatisticsQuantityMap.Add(habit.Id, habitEntryStatisticsQuantity);
            }

            AnsiConsole.MarkupLine("Habit Statistics:");

            var table = new Table();
            table.Border(TableBorder.Rounded);

            table.AddColumn("Name");
            table.AddColumn("Week (Count)");
            table.AddColumn("Week (Total)");
            table.AddColumn("Month (Count)");
            table.AddColumn("Month (Total)");
            table.AddColumn("Year (Count)");
            table.AddColumn("Year (Total)");

            foreach (var habit in habits)
            {
                var habitStatisticsCount =
                    habitStatisticsCountMap.GetValueOrDefault(habit.Id, new Dictionary<string, int>());
                var habitStatisticsQuantity =
                    habitStatisticsQuantityMap.GetValueOrDefault(habit.Id, new Dictionary<string, int>());

                var weekCount = habitStatisticsCount.GetValueOrDefault("week", 0);
                var weekQuantity = habitStatisticsQuantity.GetValueOrDefault("week", 0);
                var monthCount = habitStatisticsCount.GetValueOrDefault("month", 0);
                var monthQuantity = habitStatisticsQuantity.GetValueOrDefault("month", 0);
                var yearCount = habitStatisticsCount.GetValueOrDefault("year", 0);
                var yearQuantity = habitStatisticsQuantity.GetValueOrDefault("year", 0);

                table.AddRow(habit.Name, weekCount.ToString("N0"), $"{weekQuantity:N0} {habit.Unit}",
                    monthCount.ToString("N0"), $"{monthQuantity:N0} {habit.Unit}", yearCount.ToString("N0"),
                    $"{yearQuantity:N0} {habit.Unit}");
            }

            AnsiConsole.Write(table);
        }
    }
}