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
                    "Add Habit",
                    "Delete Habit",
                    "Quit")
                    );

            
            switch (usersChoice)
            {
                case "Add Record":
                    Database.AddRecord();
                    Console.Clear();
                    break;
                case "Delete Record":
                    Database.DeleteRecord();
                    //Console.Clear();
                    break;
                case "View Records":
                    Database.GetRecords(true);
                    break;
                case "Update Record":
                    Database.UpdateRecord();
                    Console.Clear();
                    break;
                case "Add Habit":
                    Database.AddHabit();
                    Console.Clear();
                    break;
                case "Delete Habit":
                    Database.DeleteHabit();
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
