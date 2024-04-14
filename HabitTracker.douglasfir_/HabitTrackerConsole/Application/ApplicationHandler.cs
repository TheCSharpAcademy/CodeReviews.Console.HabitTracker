using HabitTrackerConsole.Services;
using HabitTrackerConsole.Util;
using Spectre.Console;

namespace HabitTrackerConsole.Application;

/// <summary>
/// ApplicationHandler manages the main application loop and user interactions for the Habit Tracker console application.
/// It orchestrates the flow of the application based on user input and coordinates between different parts of the application.
/// </summary>
public class ApplicationHandler
{
    private readonly HabitService _habitService;
    private readonly LogEntryService _logEntryService;

    /// <summary>
    /// Initializes a new instance of the ApplicationHandler with the necessary services to manage habits and log entries.
    /// </summary>
    /// <param name="habitService">Service to manage habit-related operations.</param>
    /// <param name="logEntryService">Service to manage log entry-related operations.</param>
    public ApplicationHandler(HabitService habitService, LogEntryService logEntryService)
    {
        _habitService = habitService;
        _logEntryService = logEntryService;
    }

    /// <summary>
    /// Starts the main execution loop of the application. This method continuously displays the main menu and processes user input
    /// until the user decides to exit the application.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[underline green]Welcome to the Habit Tracker![/]\n");
            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .PageSize(10)
                .AddChoices(Enum.GetNames(typeof(MainMenuOption)).Select(name => ApplicationHelper.SplitCamelCase(name))));


            MainMenuOption selectedOption = ApplicationHelper.FromFriendlyString<MainMenuOption>(option);

            switch (selectedOption)
            {
                case MainMenuOption.ViewAndEditHabits:
                    var habitApp = new HabitApplication(_habitService);
                    habitApp.Run();
                    break;
                case MainMenuOption.ViewAndEditLogs:
                    var logApp = new LogApplication(_logEntryService, _habitService);
                    logApp.Run();
                    break;
                case MainMenuOption.SeedDatabase:
                    SeedDatabase();
                    break;
                case MainMenuOption.Exit:
                    Console.WriteLine();
                    ApplicationHelper.AnsiWriteLine(new Markup("[grey]Goodbye![/]"));
                    return;
            }
        }
    }

    /// <summary>
    /// Seeds the application's database with initial data for habits and log entries.
    /// This method is used prepopulate the database with test data.
    /// </summary>
    private void SeedDatabase()
    {
        AnsiConsole.WriteLine("Starting database seeding...");

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Processing...", ctx =>
            {
                DatabaseSeeder.SeedHabits(_habitService);
                DatabaseSeeder.SeedLogEntries(_logEntryService, _habitService, 2);
            });

        AnsiConsole.Write(new Markup("\n[green]Database seeded successfully![/]\n"));
        ApplicationHelper.PauseForContinueInput();
    }    
}

