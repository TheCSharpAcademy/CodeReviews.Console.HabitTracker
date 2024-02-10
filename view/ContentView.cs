using HabitLogger.logic.utils;
using HabitLogger.data_and_access;
using Spectre.Console;

namespace HabitLogger.view;

/// <summary>
/// This class provides methods to display various views related to habits and records.
/// </summary>
public static class ContentView
{
    /// <summary>
    /// Displays a table of habits.
    /// </summary>
    /// <param name="habits">A list of Habit objects representing the habits to be displayed.</param>
    internal static void ViewHabits(List<Habit> habits)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Measurement Unit");

        foreach (var habit in habits)
        {
            table.AddRow(habit.Id.ToString(), habit.Name, habit.Unit);
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Retrieves the records from the database and displays them in a table format.
    /// </summary>
    /// <param name="records">A list of RecordWithHabit objects containing the records to be displayed.</param>
    internal static void ViewRecords(List<RecordWithHabit> records)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Amount");
        table.AddColumn("Habit");

        foreach (var record in records)
        {
            
            table.AddRow(
                record.Id.ToString() ?? "No ID", 
                record.Date.Date.ToString("D"), 
                $"{record.Quantity} {record.Unit}",
                record.HabitName ?? "No Habit Name"
            );
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Displays the habit report for a specific habit.
    /// </summary>
    /// <param name="records">The list of records with habit information.</param>
    /// <param name="habitName">The name of the habit.</param>
    /// <param name="measurementUnit">The measurement unit of the habit.</param>
    internal static void ViewHabitReport(List<RecordWithHabit> records, string habitName, string measurementUnit)
    {
        Console.Clear();
        
        var table = new Table()
            .Title($"Habit Report for {habitName} activity")
            .AddColumn("Date")
            .AddColumn($"{measurementUnit}")
            .Width(50);

        foreach (var record in records)
        {
            table.AddRow(record.Date.ToString("D"), $"{record.Quantity}");
        }

        table.AddRow(new Rule(), new Rule());
        table.AddRow($"Total entries: {records.Count}", $"Total: {records.Sum(r => r.Quantity)} {measurementUnit}");
        
        AnsiConsole.Write(table);

        bool saveToFile = AnsiConsole.Confirm("Would you like to save this report?");
        if (saveToFile)
        {
            Utilities.SaveReportToFile(table);
        }
    }
}