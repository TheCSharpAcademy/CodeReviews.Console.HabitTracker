using HabitTracker.DataHelpers;
using Spectre.Console;

namespace Erix101.HabitTracker.Services
{
    internal class MenuService
    {
        static bool closeApp = false;
        private SQLite sql;

        //Menu Colour Palette
        Color headingColour = Color.White;
        Color menu = Color.Grey70;
        Color menuSelected = Color.SpringGreen2;
        public MenuService(SQLite sql)
        {
            this.sql = sql;
        }

        public void OpenMainMenu()
        {
            while (!closeApp)
            {
                Console.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[bold {headingColour}]MAIN MENU[/]")
                        .PageSize(6)
                        .AddChoices("Manage Habits", "Manage Habit Logs", "Exit")
                        .UseConverter(choice => $"[{menu}]{choice}[/]")
                        .HighlightStyle(new Style(foreground: menuSelected))
                        );
                string userChoice = choice;

                switch (userChoice)
                {
                    case "Exit":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        break;
                    case "Manage Habits":
                        OpenHabitsMenu();
                        break;
                    case "Manage Habit Logs":
                        OpenHabitLogMenu();
                        break;
                    default:
                        Console.WriteLine("\nInvalid command. Please type a number from 0 to 2.\n");
                        break;
                }
            }
        }
        private void OpenHabitsMenu()
        {

            bool closeMenu = false;
            while (!closeMenu)
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[bold {headingColour}]HABIT MENU[/]")
                    .PageSize(6)
                    .AddChoices("View all Habits", "Insert New Habit", "Delete Habit", "Update Habit", "Back To Main Menu")
                    .UseConverter(choice => $"[{menu}]{choice}[/]")
                    .UseConverter(choice => $"[{menu}]{choice}[/]")
                    .HighlightStyle(new Style(foreground: menuSelected))
                    );

                string userChoice = choice;
                switch (userChoice)
                {
                    case "Back To Main Menu":
                        closeMenu = true;
                        break;
                    case "View all Habits":
                        Console.Clear();
                        sql.GetAllHabits();
                        break;
                    case "Insert New Habit":
                        sql.InsertHabit(); ;
                        break;
                    case "Delete Habit":
                        sql.DeleteHabit();
                        break;
                    case "Update Habit":
                        sql.UpdateHabit();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }
        private void OpenHabitLogMenu()
        {
            sql.GetAllHabits();
            bool closeMenu = false;
            while (!closeMenu)
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[bold {headingColour}]HABIT MENU[/]")
                    .PageSize(6)
                    .AddChoices("View all Habit Logs", "Insert New Habit Log", "Delete Habit Log", "Update Habit Log", "Back To Main Menu")
                    .UseConverter(choice => $"[{menu}]{choice}[/]")
                    .HighlightStyle(new Style(foreground: menuSelected))
                    );

                string userChoice = choice;
                switch (userChoice)
                {
                    case "Back To Main Menu":

                        closeMenu = true;
                        break;
                    case "View all Habit Logs":
                        sql.GetAllHabitLogs("View");
                        break;
                    case "Insert New Habit Log":
                        sql.InsertHabitLog();
                        break;
                    case "Delete Habit Log":
                        sql.DeleteHabitLog();
                        break;
                    case "Update Habit Log":
                        sql.UpdateHabitLog();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }
        internal static void DisplayHeading()
        {
            AnsiConsole.Write(new Markup("[Lime]+--------------------------+[/]").Centered());
            AnsiConsole.Write(new Markup("[Lime]|      HABIT TRACKER       |[/]").Centered());
            AnsiConsole.Write(new Markup("[Lime]+--------------------------+[/]").Centered());
            AnsiConsole.Write(new Markup("\n\n\nWe are what we repeatedly do. Excellence, then, is not an act, but a habit. ~ Will Durant").Centered());

            AnsiConsole.Write(new Markup("\n\nPress Any Key to start your habit journey!").Centered());
            Console.ReadKey();
        }
    }
}
