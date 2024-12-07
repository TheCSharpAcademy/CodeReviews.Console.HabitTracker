using Spectre.Console;

namespace Alvind0.HabitTracker;

internal class Menu
{
    internal static void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(
                    "Add Record",
                    "Add Habit",
                    "View Records",
                    "View Statistics",
                    "Update Record",
                    "Delete Record",
                    "Delete Habit",
                    "Quit"
                    ));

            
            switch (usersChoice)
            {
                case "Add Record":
                    Database.AddRecord();
                    Console.Clear();
                    break;
                case "Add Habit":
                    Database.AddHabit();
                    Console.Clear();
                    break;
                case "View Records":
                    Database.GetRecords(true);
                    break;
                case "View Statistics":
                    Database.GetStatistics();
                    break;
                case "Update Record":
                    Database.UpdateRecord();
                    Console.Clear();
                    break;
                case "Delete Record":
                    Database.DeleteRecord();
                    break;
                case "Delete Habit":
                    Database.DeleteHabit();
                    break;
                case "Quit":
                    Console.Clear();
                    Console.WriteLine("Goodbye");
                    isMenuRunning = false;
                    break;
            }
        }
    }

    internal static string StatisticsMenu()
    {
        Console.Clear();
        var usersChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("What statistics would you like to check?")
                            .AddChoices(
                            "All Time",
                            "This Year",
                            "This Month"
                            ));

        return usersChoice;
    }
}
