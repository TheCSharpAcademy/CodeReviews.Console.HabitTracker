using Spectre.Console;

namespace HabitLogger.view;

public static class MenuView
{ 
    internal static string MainMenu(string[] mainMenuChoices)
    {
        AnsiConsole.Clear();
        
        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices(mainMenuChoices)
        );
        
        return userChoice;
    }

    internal static string ReportsMenu(string[] reportsMenuChoices)
    {
        AnsiConsole.Clear();

        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose from the following options:")
                .AddChoices(reportsMenuChoices)
        );

        return userChoice;
    }

    internal static void ShowHelp()
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("Habit Logger\n");
        AnsiConsole.WriteLine("This application allows you to track your habits and view reports based on your habits.\n");
        AnsiConsole.WriteLine("To get to main menu, type '0'(zero) on prompt. System wide.");
        AnsiConsole.WriteLine("Date format is always 'yyyy-MM-dd'.");
        AnsiConsole.WriteLine($"You can put any date from today to -1 year ({DateTime.Today.Date.AddYears(-1):yyyy-MM-dd}).");
        AnsiConsole.WriteLine("Only positive numbers are allowed.\n");
    }
}