using HabitTrackerConsole.Models;
using HabitTrackerConsole.Services;
using HabitTrackerConsole.Util;
using Spectre.Console;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTrackerConsole.Application;

/// <summary>
/// LogApplication manages the user interface for log entry operations within the Habit Tracker console application.
/// It allows users to view, add, update, and delete log entries, as well as delete all log entries, interfacing with LogEntryService and HabitService.
/// </summary>
public class LogApplication
{
    private readonly LogEntryService _logEntryService;
    private readonly HabitService _habitService;


    /// <summary>
    /// Initializes a new instance of the LogApplication with services for managing log entries and habits.
    /// </summary>
    /// <param name="logEntryService">Service to manage log entry-related operations.</param>
    /// <param name="habitService">Service to manage habit-related operations.</param>
    public LogApplication(LogEntryService logEntryService, HabitService habitService)
    {
        _logEntryService = logEntryService;
        _habitService = habitService;
    }

    /// <summary>
    /// Starts the main loop for managing log entries. Provides menu options to view, add, update, and delete log entries.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();
            ApplicationHelper.AnsiWriteLine(new Markup("[underline green]Select an option[/]\n"));
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Manage Logs")
                .PageSize(10)
                .AddChoices(Enum.GetNames(typeof(LogMenuOption)).Select(ApplicationHelper.SplitCamelCase)));

            switch (Enum.Parse<LogMenuOption>(option.Replace(" ", "")))
            {
                case LogMenuOption.ViewLogEntries:
                    ViewLogEntries();
                    break;
                case LogMenuOption.AddLogEntry:
                    AddLogEntry();
                    break;
                case LogMenuOption.UpdateLogEntry:
                    UpdateLogEntry();
                    break;
                case LogMenuOption.DeleteLogEntry:
                    DeleteLogEntry();
                    break;
                case LogMenuOption.DeleteAllLogEntries:
                    DeleteAllLogEntries();
                    break;
                case LogMenuOption.ReturnToMainMenu:
                    return;
            }
        }
    }

    /// <summary>
    /// Provides a user interface for adding a new log entry.
    /// Validates user input for date and quantity before adding the entry.
    /// </summary>
    private void AddLogEntry()
    {
        List<HabitViewModel> habits = _habitService.GetAllHabitsOverviews();
        if (!habits.Any())
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]No habits available to add a log entry. Please add a habit first.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        var habitOptions = habits.Select(h => $"[deepskyblue3]{h.HabitName}[/] (ID: {h.HabitId})").ToList();

        string selectedHabit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a habit:")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more habits)[/]")
                .HighlightStyle(new Style(Color.Black, Color.White, Decoration.Bold))
                .AddChoices(habitOptions));

        int habitId = int.Parse(Regex.Match(selectedHabit, @"ID: (\d+)").Groups[1].Value);

        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<String>("Enter the date for the log entry (yyyy-MM-dd):")
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (DateTime.TryParseExact(input.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        if (parsedDate <= DateTime.Now && parsedDate > DateTime.MinValue)
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Please enter a valid date that is not in the future.[/]");
                        }
                    }
                    else
                    {
                        return ValidationResult.Error("[red]Invalid date format. Please use the format yyyy-MM-dd.[/]");
                    }
                }));

        DateTime date = DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        var quantity = PromptForPositiveInteger("Enter the quantity:");

        if (_logEntryService.InsertLogEntryIntoHabitLog(date.ToString("yyyy-MM-dd"), habitId, quantity))
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[green]Log entry added successfully![/]"));
        }
        else
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]Failed to add log entry.[/]"));
        }

        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Provides a user interface for updating an existing log entry.
    /// Validates the new quantity before updating the entry.
    /// </summary>
    private void UpdateLogEntry()
    {
        List<LogEntryViewModel> entries = _logEntryService.GetAllLogEntriesFromHabitsLogView();
        if (!entries.Any())
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]No log entries available to update.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        LogEntryViewModel entryToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<LogEntryViewModel>()
                .Title("[yellow]Which log entry would you like to delete?[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more log entries)[/]")
                .UseConverter(entry =>
                    $"[bold yellow]ID:[/] {entry.RecordId}, " +
                    $"[bold cyan]Date:[/] {entry.Date:yyyy-MM-dd}, " +
                    $"[bold green]Habit:[/] {entry.HabitName}, " +
                    $"[bold magenta]Quantity:[/] {entry.Quantity}")
                .AddChoices(entries));

        int newQuantity = PromptForPositiveInteger($"Enter the new quantity for log entry ID {entryToUpdate.RecordId}:");

        if (AnsiConsole.Confirm($"Are you sure you want to update the quantity for this log entry (ID: {entryToUpdate.RecordId}) to {newQuantity}?"))
        {
            bool result = _logEntryService.UpdateLogEntryInHabitsLog(entryToUpdate.RecordId, newQuantity);
            if (result)
                ApplicationHelper.AnsiWriteLine(new Markup("[green]Log entry updated successfully![/]"));
            else
                ApplicationHelper.AnsiWriteLine(new Markup("[red]Failed to update log entry.[/]"));
        }
        else
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[yellow]Update operation cancelled.[/]"));
        }

        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Provides a user interface for deleting a specific log entry.
    /// Confirms with the user before deletion.
    /// </summary>
    private void DeleteLogEntry()
    {
        var entries = _logEntryService.GetAllLogEntriesFromHabitsLogView();
        if (!entries.Any())
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]No log entries available to delete.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        var entryToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<LogEntryViewModel>()
                .Title("[yellow]Which log entry would you like to delete?[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more log entries)[/]")
                .UseConverter(entry =>
                    $"[bold yellow]ID:[/] {entry.RecordId}, " +
                    $"[bold cyan]Date:[/] {entry.Date:yyyy-MM-dd}, " +
                    $"[bold green]Habit:[/] {entry.HabitName}, " +
                    $"[bold magenta]Quantity:[/] {entry.Quantity}")
                .AddChoices(entries));


        if (AnsiConsole.Confirm($"Are you sure you want to delete this log entry (ID: {entryToDelete.RecordId})?"))
        {
            bool result = _logEntryService.DeleteLogEntryFromHabitsLog(entryToDelete.RecordId);
            if (result)
                ApplicationHelper.AnsiWriteLine(new Markup("[green]Log entry successfully deleted![/]"));
            else
                ApplicationHelper.AnsiWriteLine(new Markup("[red]Failed to delete log entry. It may no longer exist or the database could be locked.[/]"));
        }
        else
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[yellow]Operation cancelled.[/]"));
        }

        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Displays all log entries in a formatted table.
    /// </summary>
    private void ViewLogEntries()
    {
        var entries = _logEntryService.GetAllLogEntriesFromHabitsLogView();
        if (!entries.Any())
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[yellow]No log entries found![/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.Grey);
        table.Title("[yellow]Log Entries[/]");

        table.AddColumn(new TableColumn("[bold underline]Record ID[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold underline]Date[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold underline]Habit Name[/]").Centered());
        table.AddColumn(new TableColumn("[bold underline]Quantity[/]").RightAligned());

        foreach (var entry in entries)
        {
            table.AddRow(
                entry.RecordId.ToString(),
                entry.Date.ToString("yyyy-MM-dd"),
                entry.HabitName!,
                $"[bold {"green"}]{entry.Quantity}[/]");
        }

        AnsiConsole.Write(table);
        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Provides an interface for deleting all log entries in the database.
    /// Confirms with the user before performing the bulk deletion.
    /// </summary>
    private void DeleteAllLogEntries()
    {
        if (!AnsiConsole.Confirm("Are you sure you want to delete ALL log entries?"))
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[yellow]Operation cancelled.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        if (_logEntryService.DeleteAllLogEntries())
        {
            AnsiConsole.Write(new Markup("[green]All log entries have been successfully deleted![/]\n"));
        }
        else
        {
            AnsiConsole.Write(new Markup("[red]Failed to delete log entries.[/]\n"));
        }

        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Prompts the user for a positive integer input with custom messaging and validates the input.
    /// </summary>
    /// <param name="promptMessage">The message displayed to the user when asking for input.</param>
    /// <returns>The validated positive integer input from the user. If the input is not a valid integer or is not positive, 
    /// the function continues to prompt the user until a valid positive integer is entered.</returns>
    public int PromptForPositiveInteger(string promptMessage)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(promptMessage)
                .Validate(input =>
                {
                    // Attempt to parse the input as an integer
                    if (!int.TryParse(input.ToString().Trim(), out int parsedQuantity))
                    {
                        return ValidationResult.Error("[red]Please enter a valid integer number.[/]");
                    }

                    // Check if the parsed integer is positive
                    if (parsedQuantity <= 0)
                    {
                        return ValidationResult.Error("[red]Please enter a positive number.[/]");
                    }

                    return ValidationResult.Success();
                }));
    }
}
