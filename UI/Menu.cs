using Spectre.Console;
using Habit_Logger.Services;

namespace Habit_Logger.UI
{
    internal class Menu
    {
        internal static void MainMenu()
        {
            var isMenuRunning = true;

            while (isMenuRunning)
            {
                var usersChoice = AnsiConsole.Prompt(
                       new SelectionPrompt<string>()
                        .Title("Welcome! Please select from the following options:")
                        .AddChoices(
                           "Add Habit",
                           "Delete Habit",
                           "Update Habit",
                           "Add Progress",
                           "Delete Progress",
                           "View All Progress",
                           "Update Progress",
                           "Quit")
                        );

                switch (usersChoice)
                {
                    case "Add Habit":
                        HabitServices.InsertHabit();
                        break;
                    case "Delete Habit":
                        HabitServices.DeleteHabit();
                        break;
                    case "Update Habit":
                        HabitServices.UpdateHabit();
                        break;
                    case "Add Progress":
                        HabitServices.InsertProgress();
                        break;
                    case "Delete Progress":
                        HabitServices.DeleteProgress();
                        break;
                    case "View All Progress":
                        HabitServices.GetProgress();
                        break;
                    case "Update Progress":
                        HabitServices.UpdateProgress();
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
}
