using Spectre.Console;

namespace HabitTracker;

internal class UserInterface(string connectionString, string userName)
{
    private readonly HabitController _habitController = new HabitController(connectionString, userName);
    internal void MainMenu()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                    .Title("What do you want to do next?")
                    .AddChoices(Enum.GetValues<MenuOption>()));

            switch (choice)
            {
                case MenuOption.InsertHabit:
                    _habitController.InsertHabit();
                    break;
                case MenuOption.SeeHabits:
                    _habitController.SeeHabits();
                    break;
                case MenuOption.UpdateHabit:
                    _habitController.UpdateHabit();
                    break;
                case MenuOption.RemoveHabit:
                    _habitController.RemoveHabit();
                    break;
                case MenuOption.ExitApplication:
                    AnsiConsole.MarkupLine("[green]Goodbye![/]");
                    return;
            }

        }
    }
}