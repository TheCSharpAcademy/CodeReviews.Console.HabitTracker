namespace DotNETConsole.HabitTracker.UI;
using Spectre.Console;
using Enums;
using DataModels;
using Controllers;

public class Menu
{
    public MainUI GetChoice()
    {
        MainUI choice = AnsiConsole.Prompt(new SelectionPrompt<MainUI>()
                .Title("Select [green]options from the menu.[/]?").AddChoices(new List<MainUI>((MainUI[])Enum.GetValues(typeof(MainUI)))));
        return choice;
    }

    public string GetNewHabit()
    {
        string habitTitle = AnsiConsole.Prompt(
            new TextPrompt<string>("Habit Title: ").Validate((title) => title.Length switch
            {
                < 3 => ValidationResult.Error("Too short(min 3 char) or blank."),
                >= 3 => ValidationResult.Success()
            }));
        return habitTitle;
    }

    public Habit SelectSingleHabit()
    {
        var habitController = new HabitController();
        List<Habit> habits = habitController.GetHabits();
        var habit = AnsiConsole.Prompt(
            new SelectionPrompt<Habit>()
                .Title("Select habit:")
                .PageSize(10)
                .AddChoices(habits));
        return habit;
    }
}
