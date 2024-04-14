using HabitTrackerConsole.Models;
using HabitTrackerConsole.Services;
using HabitTrackerConsole.Util;
using Spectre.Console;

namespace HabitTrackerConsole.Application;

/// <summary>
/// Manages habit-related operations and user interactions.
/// </summary>
public class HabitApplication
{
    private readonly HabitService _habitService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HabitApplication"/> class.
    /// </summary>
    /// <param name="habitService">The service for managing habit data.</param>
    public HabitApplication(HabitService habitService)
    {
        _habitService = habitService;
    }

    /// <summary>
    /// Starts the habit management menu loop.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();
            ApplicationHelper.AnsiWriteLine(new Markup("[underline green]Select an option[/]\n"));
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Manage Habits")
                .PageSize(10)
                .AddChoices(Enum.GetNames(typeof(HabitMenuOption)).Select(ApplicationHelper.SplitCamelCase)));

            switch (Enum.Parse<HabitMenuOption>(option.Replace(" ", "")))
            {
                case HabitMenuOption.AddNewHabit:
                    AddHabit();
                    break;
                case HabitMenuOption.UpdateHabit:
                    UpdateHabit();
                    break;
                case HabitMenuOption.ViewHabitInformation:
                    ViewHabits();
                    break;
                case HabitMenuOption.DeleteHabit:
                    DeleteHabit();
                    break;
                case HabitMenuOption.DeleteAllHabits:
                    DeleteAllHabits();
                    break;
                case HabitMenuOption.ReturnToMainMenu:
                    return;
            }
        }
    }


    /// <summary>
    /// Adds a new habit by prompting the user for habit details and ensuring the name is valid and unique before saving it to the database.
    /// </summary>
    private void AddHabit()
    {
        string habitName = GetHabitInputString("Enter the name of the new habit:");

        // InsertHabitIntoHabitsTable will return a boolean of true if rows were successfully inserted
        if (_habitService.InsertHabitIntoHabitsTable(habitName))
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[green]Habit added successfully![/]"));
        }
        else
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]Failed to add habit.[/]"));
        }

        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Updates an existing habit's details selected by the user.
    /// </summary>
    private void UpdateHabit()
    {
        List<HabitViewModel> habits = _habitService.GetAllHabitsOverviews();
        if (habits.Count == 0)
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]No habits available to update.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        HabitViewModel habitToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<HabitViewModel>()
                .Title("Which habit would you like to update?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more habits)[/]")
                .UseConverter(h => $"{h.HabitName} (ID: {h.HabitId})")
                .AddChoices(habits));

        string newName = GetHabitInputString("Enter the new name for the habit:");

        // UpdateHabit will return a boolean of true if records were successfully updated
        if (_habitService.UpdateHabit(habitToUpdate.HabitId, newName))
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[green]Habit updated successfully![/]"));
        }
        else
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]Failed to update habit.[/]"));
        }

        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Deletes a habit selected by the user, including all related log entries.
    /// </summary>
    private void DeleteHabit()
    {
        List<HabitViewModel> habits = _habitService.GetAllHabitsOverviews();
        if (habits.Count == 0)
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]No habits available to delete.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        HabitViewModel habitToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<HabitViewModel>()
                .Title("Which habit would you like to delete?")
                .PageSize(10)
                .UseConverter(h => $"{h.HabitName} (ID: {h.HabitId})")
                .AddChoices(habits));

        if (AnsiConsole.Confirm($"Are you sure you want to delete the habit '{habitToDelete.HabitName}' and all its log entries?"))
        {
            bool result = _habitService.DeleteHabit(habitToDelete.HabitId);
            if (result)
                ApplicationHelper.AnsiWriteLine(new Markup("[green]Habit and all related log entries successfully deleted![/]"));
            else
                ApplicationHelper.AnsiWriteLine(new Markup("[red]Failed to delete the habit and log entries.[/]"));
        }
        else
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[yellow]Operation cancelled.[/]"));
        }

        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Deletes all habits and their associated log entries after confirming with the user.
    /// </summary>
    private void DeleteAllHabits()
    {
        List<HabitViewModel> habits = _habitService.GetAllHabitsOverviews();
        if (habits.Count == 0)
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[red]No habits available to delete.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        if (!AnsiConsole.Confirm("Are you sure you want to delete ALL habits and associated log entries?"))
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[yellow]Operation cancelled.[/]"));
            ApplicationHelper.PauseForContinueInput();
            return;
        }

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Processing...", ctx =>
            {
                foreach (var habit in habits)
                {
                    bool result = _habitService.DeleteHabit(habit.HabitId);
                }
            });

        ApplicationHelper.AnsiWriteLine(new Markup("[green]All Habits with related log entries successfully deleted![/]"));
        ApplicationHelper.PauseForContinueInput();
    }

    /// <summary>
    /// Displays all existing habits in a formatted table.
    /// </summary>
    private void ViewHabits()
    {
        var habits = _habitService.GetAllHabitsOverviews();
        if (habits.Count == 0)
        {
            ApplicationHelper.AnsiWriteLine(new Markup("[yellow]No habits found![/]"));
            ApplicationHelper.PauseForContinueInput();
        }
        else
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(Color.Grey);
            table.Title("[yellow]Habit Overview[/]");

            // Adding columns with improved styling
            table.AddColumn(new TableColumn("[bold underline]ID[/]").LeftAligned());
            table.AddColumn(new TableColumn("[bold underline]Name[/]").Centered());
            table.AddColumn(new TableColumn("[bold underline]Date Created[/]").LeftAligned());
            table.AddColumn(new TableColumn("[bold underline]Last Log Entry Date[/]").LeftAligned());
            table.AddColumn(new TableColumn("[bold underline]Total Logs[/]").RightAligned());

            // Add rows with conditional formatting
            foreach (var habit in habits)
            {
                table.AddRow(
                    habit.HabitId.ToString(),
                    habit.HabitName!,
                    habit.DateCreated!,
                    habit.LastLogEntryDate ?? "N/A",
                    $"[bold {"green"}]{habit.TotalLogs}[/]");  // Apply color styling inline
            }

            AnsiConsole.Write(table);
        }

        ApplicationHelper.PauseForContinueInput();
    }


    /// <summary>
    /// Prompts the user for the name of the new habit and validates the input.
    /// </summary>
    public string GetHabitInputString(string promptMessage)
    {
        string habitName = string.Empty;
        do
        {
            habitName = AnsiConsole.Ask<string>(promptMessage).Trim();
            if (string.IsNullOrWhiteSpace(habitName))
            {
                ApplicationHelper.AnsiWriteLine(new Markup("[yellow]Habit name cannot be empty. Please enter a valid name.[/]"));
            }
            else if (_habitService.DoesHabitExist(habitName))
            {
                ApplicationHelper.AnsiWriteLine(new Markup("[yellow]A habit with this name already exists. Please enter a unique name.[/]"));
                habitName = string.Empty;  // Reset habitName to force re-entry.
            }
        } while (string.IsNullOrEmpty(habitName));

        return habitName;
    }
}
