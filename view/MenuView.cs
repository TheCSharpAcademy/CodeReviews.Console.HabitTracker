using Spectre.Console;

namespace HabitLogger.view;

public static class MenuView
{ 
    internal static string MainMenu(string[] mainMenuChoices)
    {
        Console.Clear();
        
        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?\n\nType '0' on prompt to return\nto this menu (system wide)")
                .AddChoices(mainMenuChoices)
        );
        
        return userChoice;
    }

    internal static string ReportsMenu(string[] reportsMenuChoices)
    {
        Console.Clear();

        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose from the following options:")
                .AddChoices(reportsMenuChoices)
        );

        return userChoice;
    }
}