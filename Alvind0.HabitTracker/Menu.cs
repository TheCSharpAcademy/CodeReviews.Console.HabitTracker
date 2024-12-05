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
                    "Delete Record",
                    "View Records",
                    "Update Record",
                    "Quit")
                    );

            switch (usersChoice)
            {
                case "Add Record":
                    Database.AddRecord();
                    break;
                case "Delete Record":
                    Database.DeleteRecord();
                    break;
                case "View Records":
                    Database.GetRecords();
                    break;
                case "Update Record":
                    Database.UpdateRecord();
                    break;
                case "Quit":
                    Console.WriteLine("Goodbye");
                    isMenuRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please choose one of the above");
                    break;
            }
        }
    }
}
