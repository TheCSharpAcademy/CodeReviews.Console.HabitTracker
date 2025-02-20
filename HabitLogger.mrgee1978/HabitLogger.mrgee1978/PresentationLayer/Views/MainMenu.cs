using Spectre.Console;
using HabitLogger.mrgee1978.DataAccessLayer;
using HabitLogger.mrgee1978.PresentationLayer.Enumerations;

namespace HabitLogger.mrgee1978.PresentationLayer.Views;

public static class MainMenu
{
    public static void DisplayMenu()
    {
        DatabaseCreation.CreateDatabase();
        RecordView recordView = new RecordView();
        HabitView habitView = new HabitView();

        bool isMenuRunning = true;

        while (isMenuRunning)
        {
            var menuChoices = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("Please choose one of the following options: ")
                .AddChoices(
                    MenuOptions.AddHabit,
                    MenuOptions.UpdateHabit,
                    MenuOptions.DeleteHabit,
                    MenuOptions.ViewHabits,
                    MenuOptions.AddRecord,
                    MenuOptions.UpdateRecord,
                    MenuOptions.DeleteRecord,
                    MenuOptions.ViewRecords,
                    MenuOptions.Quit));

            switch (menuChoices)
            {
                case MenuOptions.AddHabit:
                    habitView.AddHabit();
                    break;
                case MenuOptions.UpdateHabit:
                    habitView.UpdateHabit();
                    break;
                case MenuOptions.DeleteHabit:
                    habitView.DeleteHabit();
                    break;
                case MenuOptions.ViewHabits:
                    habitView.ViewHabits();
                    AnsiConsole.Markup("[blue]\n\nPress any key to continue[/]");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case MenuOptions.AddRecord:
                    recordView.AddRecord();
                    break;
                case MenuOptions.UpdateRecord:
                    recordView.UpdateRecord();
                    break;
                case MenuOptions.DeleteRecord:
                    recordView.DeleteRecord();
                    break;
                case MenuOptions.ViewRecords:
                    recordView.ViewRecords();
                    AnsiConsole.Markup("[blue]\n\nPress any key to continue[/]");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case MenuOptions.Quit:
                    AnsiConsole.Markup("[bold green]Thank you for using the habit logger!\nPress any key to exit\n[/]");
                    Console.ReadKey();
                    isMenuRunning = false;
                    break;
                default:
                    AnsiConsole.Markup("[bold red]Invalid option! Please enter one of the above options!\nPress any key to return to the main menu[/]");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }
}
