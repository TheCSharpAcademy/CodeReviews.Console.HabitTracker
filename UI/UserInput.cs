namespace DotNETConsole.HabitTracker.UI;
using Spectre.Console;
using DataModels;

public class UserInput
{
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

    public HabitLog LogHabit()
    {
        var menu = new Menu();
        var habit = menu.SelectSingleHabit();
        AnsiConsole.WriteLine($"Selected Habit: {habit.Title}");
        int quentity = AnsiConsole.Ask<int>("Enter [green]quantity[/]:");
        DateTime date = AnsiConsole.Ask<DateTime>("Date: ");
        var newLog = new HabitLog();
        newLog.LogDate = date;
        newLog.Quantity = quentity;
        newLog.HabitId = habit.Id;
        return newLog;
    }

    public bool ContinueInput()
    {
        AnsiConsole.MarkupLine("[green]Press ESC to continue...[/]");
        while (true)
        {
            var keyPressed = AnsiConsole.Console.Input.ReadKey(true);
            if (keyPressed is { Key: ConsoleKey.Escape })
            {
                return true;
            }
        }
    }

    public ConsoleKeyInfo? HabitLogModifyOptionPrompt()
    {
        AnsiConsole.MarkupLine("[green]Press ESC to return to main menu\n Press E to edit\n Press D to delete...[/]");
        while (true)
        {
            var keyPressed = AnsiConsole.Console.Input.ReadKey(true);
            if (keyPressed is { Key: ConsoleKey.Escape } || keyPressed is { Key: ConsoleKey.D } || keyPressed is { Key: ConsoleKey.E })
            {
                return keyPressed;
            }
        }
    }
}
