using System.Globalization;
using Spectre.Console;
// Keep basic System usings
// Needed for LINQ methods like Any()

// Add Spectre.Console namespace

namespace HabitTracker;

internal static class Program
{
    private static readonly DatabaseManager DbManager = new DatabaseManager();
    private const string DateFormat = "yyyy-MM-dd";

    // Define Menu Option constants for clarity and DRY
    private const string OptViewLogs = "View All Log Records";
    private const string OptInsertLog = "Insert Log Record";
    private const string OptDeleteLog = "Delete Log Record";
    private const string OptUpdateLog = "Update Log Record";
    private const string OptManageHabits = "Manage Habits (View/Create)";
    private const string OptViewReports = "View Reports";
    private const string OptExit = "Close Application";

    // Habit Management Menu Options
    private const string OptViewHabits = "View Existing Habits";
    private const string OptCreateHabit = "Create New Habit";
    private const string OptBackToMain = "Back to Main Menu";

    // Report Menu Options
    private const string OptYearlyReport = "Yearly Habit Summary";

    private static void Main()
    {
        // Optional: Add a nice title
        AnsiConsole.Write(new FigletText("Habit Logger").Centered().Color(Color.Blue));
        AnsiConsole.Write(new Rule("[yellow]Enhanced Edition[/]").Centered());

        // Check for seeding messages from DatabaseManager (can't easily capture Console.WriteLine)
        // If you styled seeding messages in DatabaseManager with AnsiConsole, you wouldn't need this pause.
        // Since we didn't, this brief pause lets the user see seeding messages if they occurred.
        if (!File.Exists(DatabaseManager.DatabaseFileName + "-seeded")) // Use a simple flag file
        {
            // If the DatabaseManager printed seeding messages, pause briefly.
            // This is a workaround for not capturing Console.WriteLine from another class easily.
            // A better approach might involve logging or eventing.
            Thread.Sleep(1500); // Pause 1.5 seconds if potentially seeded
            try { File.Create(DatabaseManager.DatabaseFileName + "-seeded").Close(); } catch { /* Ignore */ }
        }


        var keepRunning = true;
        while (keepRunning)
        {
            AnsiConsole.Clear(); // Clear console before showing menu
            var choice = ShowMainMenuPrompt(); // Use Spectre prompt

            switch (choice)
            {
                case OptViewLogs:
                    ViewAllLogRecords();
                    break;
                case OptInsertLog:
                    InsertLogRecord();
                    break;
                case OptDeleteLog:
                    DeleteLogRecord();
                    break;
                case OptUpdateLog:
                    UpdateLogRecord();
                    break;
                case OptManageHabits:
                    ManageHabitsMenu();
                    break;
                case OptViewReports:
                    ReportsMenu();
                    break;
                case OptExit:
                    keepRunning = false;
                    AnsiConsole.MarkupLine("[green]Exiting Habit Logger. Goodbye![/]");
                    break;
                // No default needed as SelectionPrompt ensures a valid choice
            }

            if (keepRunning)
            {
                AnsiConsole.WriteLine(); // Add a line break
                AnsiConsole.MarkupLine("[grey]Press any key to return to the main menu...[/]");
                Console.ReadKey(true); // Wait for user key press without showing the key
            }
        }
    }

    // Use SelectionPrompt for the main menu
    static string ShowMainMenuPrompt()
    {
        AnsiConsole.Write(new Rule("[bold blue]MAIN MENU[/]").LeftJustified());
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .PageSize(10) // Show up to 10 options at once
                .AddChoices(new[] {
                    OptViewLogs, OptInsertLog, OptDeleteLog, OptUpdateLog,
                    OptManageHabits, OptViewReports,
                    OptExit
                }));
    }

    // --- Habit Management UI (Spectre) ---

    static void ManageHabitsMenu()
    {
        bool keepManaging = true;
        while (keepManaging)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold green]Manage Habits[/]").LeftJustified());
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        OptViewHabits, OptCreateHabit, OptBackToMain
                    }));

            switch (choice)
            {
                case OptViewHabits:
                    ViewAllHabits();
                    break;
                case OptCreateHabit:
                    CreateNewHabit();
                    break;
                case OptBackToMain:
                    keepManaging = false;
                    break;
            }
            if (keepManaging)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Press any key to return to the habit menu...[/]");
                Console.ReadKey(true);
            }
        }
    }

    // Display habits using Spectre Table
    static void ViewAllHabits()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold yellow]Existing Habits[/]").LeftJustified());
        var habits = DbManager.GetAllHabits();

        if (!habits.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No habits defined yet.[/]");
            return;
        }

        var table = new Table().Expand(); // Use Expand() for better width usage
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Unit of Measurement");

        foreach (var habit in habits)
        {
            table.AddRow(habit.Id.ToString(), habit.Name, habit.UnitOfMeasurement);
        }

        AnsiConsole.Write(table);
    }

    // Create habit using Spectre Prompts
    static void CreateNewHabit()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold cyan]Create New Habit[/]").LeftJustified());

        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the [green]name[/] for the new habit:")
                .PromptStyle("cyan") // Style the prompt text itself
                .Validate(n => !string.IsNullOrWhiteSpace(n), "[red]Habit name cannot be empty[/]"));

        var unit = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter the [green]unit of measurement[/] for '{name}' (e.g., km, glasses):")
                .PromptStyle("cyan")
                .Validate(u => !string.IsNullOrWhiteSpace(u), "[red]Unit cannot be empty[/]"));

        var success = DbManager.InsertHabit(name, unit);

        AnsiConsole.MarkupLine(success
            ? $"[green]Habit '{name}' ({unit}) created successfully.[/]"
            // DatabaseManager prints specific errors (like UNIQUE constraint) via Console.WriteLine
            // We rely on that here, or would need DatabaseManager to return error details.
            : "[red]Failed to create habit. See console output for details (if any).[/]");
    }


    // --- Log Record CRUD UI (Spectre) ---

    // Display logs using Spectre Table, grouped by habit
    private static void ViewAllLogRecords()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold yellow]All Log Records[/]").LeftJustified());
        var records = DbManager.GetAllRecords();

        if (records.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No log records found.[/]");
            return;
        }

        var groupedRecords = records.GroupBy(r => r.HabitName ?? "Unknown Habit") // Handle potential null
            .OrderBy(g => g.Key); // Order groups alphabetically by habit name

        foreach (var group in groupedRecords)
        {
            // Use habit name and unit in the rule title
            var unit = group.First().UnitOfMeasurement ?? "";
            AnsiConsole.Write(new Rule($"[bold orchid]Habit: {group.Key} ({unit})[/]").LeftJustified());

            var table = new Table().Expand();
            table.AddColumn("Log ID");
            table.AddColumn("Date");
            table.AddColumn("Quantity");
            // Unit is already in the title, no need for column unless preferred

            // Order records within the group by date descending
            foreach (var record in group.OrderByDescending(r => r.Date))
            {
                table.AddRow(
                    record.Id.ToString(),
                    record.Date.ToString(DateFormat), // Format date
                    record.Quantity.ToString()
                );
            }
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine(); // Add space between habit tables
        }
    }

    // Insert log using Spectre Prompts
    private static void InsertLogRecord()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold cyan]Insert Log Record[/]").LeftJustified());

        var selectedHabit = GetHabitSelectionPrompt("Select the habit you are logging:");
        if (selectedHabit == null)
        {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled or no habits available.[/]");
            return;
        }

        string dateInput = GetDateInputPrompt("Enter the date for the record");
        int quantityInput = GetQuantityInputPrompt($"Enter the quantity ([yellow]{selectedHabit.UnitOfMeasurement}[/])");

        bool success = DbManager.InsertRecord(selectedHabit.Id, dateInput, quantityInput);

        AnsiConsole.MarkupLine(success
            ? "[green]Log record inserted successfully.[/]"
            : "[red]Failed to insert log record. See console output for details (if any).[/]");
    }

    // Delete log using Spectre Prompts
    static void DeleteLogRecord()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold red]Delete Log Record[/]").LeftJustified());
        ViewAllLogRecords(); // Show records first

        var allRecords = DbManager.GetAllRecords(); // Get records again to validate ID
        if (!allRecords.Any()) return; // Already handled by ViewAllLogRecords, but good practice

        int idToDelete = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [green]Log ID[/] of the record to delete (or 0 to cancel):")
                .Validate(id =>
                {
                    if (id == 0) return ValidationResult.Success(); // Allow cancellation
                    if (id < 0) return ValidationResult.Error("[red]ID must be non-negative[/]");
                    // Check if the ID actually exists in the logs we fetched
                    return allRecords.Any(r => r.Id == id)
                        ? ValidationResult.Success()
                        : ValidationResult.Error($"[red]Log ID {id} not found.[/]");
                }));

        if (idToDelete == 0)
        {
            AnsiConsole.MarkupLine("[yellow]Deletion cancelled.[/]");
            return;
        }

        // No need to check exists again, prompt validation did it.
        bool success = DbManager.DeleteRecord(idToDelete);

        AnsiConsole.MarkupLine(success
            ? $"[green]Log record with ID {idToDelete} deleted successfully.[/]"
            // This case should be less likely now due to validation, but handle DB errors
            : $"[red]Failed to delete log record with ID {idToDelete}. Database error occurred.[/]");
    }

    // Update log using Spectre Prompts
    private static void UpdateLogRecord()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold blue]Update Log Record[/]").LeftJustified());
        ViewAllLogRecords(); // Show records first

        var allRecords = DbManager.GetAllRecords();
        if (!allRecords.Any()) return;

        int idToUpdate = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [green]Log ID[/] of the record to update (or 0 to cancel):")
                .Validate(id =>
                {
                    if (id == 0) return ValidationResult.Success();
                    if (id < 0) return ValidationResult.Error("[red]ID must be non-negative[/]");
                    return allRecords.Any(r => r.Id == id)
                        ? ValidationResult.Success()
                        : ValidationResult.Error($"[red]Log ID {id} not found.[/]");
                }));

        if (idToUpdate == 0)
        {
            AnsiConsole.MarkupLine("[yellow]Update cancelled.[/]");
            return;
        }

        // Get the specific record to show the unit in the prompt
        var currentRecord = allRecords.First(r => r.Id == idToUpdate);
        string unit = currentRecord.UnitOfMeasurement ?? "units";

        AnsiConsole.MarkupLine($"\nEnter new data for Log ID [yellow]{idToUpdate}[/]:");
        string newDateInput = GetDateInputPrompt("Enter the new date");
        int newQuantityInput = GetQuantityInputPrompt($"Enter the new quantity ([yellow]{unit}[/])");

        bool success = DbManager.UpdateRecord(idToUpdate, newDateInput, newQuantityInput);

        AnsiConsole.MarkupLine(success
            ? $"[green]Log record with ID {idToUpdate} updated successfully.[/]"
            : $"[red]Failed to update log record with ID {idToUpdate}. Database error occurred.[/]");
    }

    // --- Reporting UI (Spectre) ---

    static void ReportsMenu()
    {
        bool keepReporting = true;
        while(keepReporting)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold magenta]Reports[/]").LeftJustified());
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a report:")
                    .AddChoices(new[] { OptYearlyReport, OptBackToMain })
            );

            switch (choice)
            {
                case OptYearlyReport:
                    GenerateYearlySummaryReport();
                    break;
                case OptBackToMain:
                    keepReporting = false;
                    break;
            }
            if (keepReporting)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Press any key to return to the reports menu...[/]");
                Console.ReadKey(true);
            }
        }
    }

    static void GenerateYearlySummaryReport()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold yellow]Yearly Habit Summary Report[/]").LeftJustified());

        Habit? selectedHabit = GetHabitSelectionPrompt("Select the habit for the report:");
        if (selectedHabit == null)
        {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled or no habits available.[/]");
            return;
        }

        int year = GetYearInputPrompt("Enter the year for the report (e.g., 2024):");

        // Optional: Add a status indicator while querying
        (int TotalQuantity, int RecordCount)? summary = null;
        AnsiConsole.Status()
            .Start($"Fetching summary for '{selectedHabit.Name}'...", ctx =>
            {
                summary = DbManager.GetYearlyHabitSummary(selectedHabit.Id, year);
                // Simulate work if needed: Thread.Sleep(500);
                ctx.Status("Done.");
            });


        if (summary.HasValue)
        {
            var (totalQuantity, recordCount) = summary.Value;
            AnsiConsole.WriteLine(); // Line break after status

            var table = new Table()
                .Title($"Summary for '[yellow]{selectedHabit.Name}[/]' in [yellow]{year}[/]")
                .Border(TableBorder.Rounded)
                .AddColumn("Metric")
                .AddColumn("Value");

            table.AddRow("Total Occurrences Logged", $"[cyan]{recordCount}[/]");
            table.AddRow($"Total Quantity ([yellow]{selectedHabit.UnitOfMeasurement}[/])", $"[cyan]{totalQuantity}[/]");

            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.MarkupLine($"\n[yellow]No records found for '{selectedHabit.Name}' in the year {year}.[/]");
        }
    }


    // --- Input Helper Functions (Using Spectre Prompts) ---

    // Gets a valid habit selection using SelectionPrompt
    static Habit? GetHabitSelectionPrompt(string promptTitle)
    {
        var habits = DbManager.GetAllHabits();

        if (habits.Count == 0)
        {
            // Message handled by the caller function
            return null;
        }

        // Add a "Cancel" option represented by null
        new SelectionPrompt<Habit>() // Note the nullable Habit?
            .Title($"[dodgerblue1]{promptTitle}[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more habits)[/]")
            // Provide a converter to display habit name and unit, handling the null cancel option
            .UseConverter(habit => $"{habit.Name} ([grey]{habit.UnitOfMeasurement}[/])")
            .AddChoices(habits) // Add the actual habits
            .AddChoice(null!);

        return AnsiConsole.Prompt<Habit?>(null!); // Prompt returns the selected Habit or null
    }


    // Gets a valid date string using TextPrompt with validation
    private static string GetDateInputPrompt(string prompt)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"[dodgerblue1]{prompt} (Format: [yellow]{DateFormat}[/]):[/]")
                .PromptStyle("blue")
                .Validate(dateStr =>
                {
                    if (DateTime.TryParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        return ValidationResult.Success();
                    }
                    return ValidationResult.Error($"[red]Invalid date format. Please use {DateFormat}[/]");
                }));
    }

    // Gets a valid non-negative integer using TextPrompt
    static int GetQuantityInputPrompt(string prompt)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>($"[dodgerblue1]{prompt}:[/]")
                .PromptStyle("blue")
                .Validate(quantity => quantity >= 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Quantity must be a non-negative number[/]")));
    }

    // Gets a valid year using TextPrompt
    static int GetYearInputPrompt(string prompt)
    {
        int currentYear = DateTime.Now.Year;
        return AnsiConsole.Prompt(
            new TextPrompt<int>($"[dodgerblue1]{prompt}:[/]")
                .PromptStyle("blue")
                .Validate(year => (year > 1900 && year <= currentYear + 5) // Allow a bit into the future
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[red]Please enter a valid 4-digit year (e.g., 1990-{currentYear + 5})[/]")));
    }

    // Note: GetPositiveIntegerInput is effectively replaced by the validation logic
    // within the TextPrompts used for Delete/Update ID selection.
}