namespace DotNETConsole.HabitTracker.Helper;
using Spectre.Console;
using DataModels;
using UI;

public class UserInput
{
    public (string, string) GetNewHabit()
    {
        string habitTitle = AnsiConsole.Prompt(
            new TextPrompt<string>("Habit Title: ").Validate((title) => title.Length switch
            {
                < 3 => ValidationResult.Error("Too short(min 3 char) or blank."),
                >= 3 => ValidationResult.Success()
            }));
        string unit = AnsiConsole.Prompt(
            new TextPrompt<string>(" Unit(Ex. KM, L, Hour etc.): ").Validate((title) => title.Trim().Length switch
            {
                < 1 => ValidationResult.Error("This can not be blank."),
                >= 1 => ValidationResult.Success()
            }));
        return (habitTitle, unit);
    }

    public HabitLog LogHabit()
    {
        var menu = new Menu();
        var habit = menu.SelectSingleHabit();
        AnsiConsole.WriteLine($"Selected Habit: {habit.Title}");
        int quantity = AnsiConsole.Ask<int>("Enter [green]quantity[/] (" + habit.Unit + "): ");
        DateTime dateTime;
        while (true)
        {
            string date = AnsiConsole.Ask<string>("Enter Date(Ex. DD/MM/YYYY) or 'now' for current date: ");

            if (date.Trim().Length != 0 && date.ToLower() == "now")
            {
                dateTime = DateTime.Now;
                break;
            }
            
            if(DateTime.TryParse(date, out dateTime))
            {
                break;
            }
            AnsiConsole.MarkupLine("[red]Invalid date format. Please enter a valid date or 'now'.[/]");
        }
        var newLog = new HabitLog();
        newLog.LogDate = dateTime;
        newLog.Quantity = quantity;
        newLog.HabitId = habit.Id;
        return newLog;
    }

    public bool ContinueInput(string? extraMessage = null)
    {
        if (extraMessage != null)
        {
            AnsiConsole.MarkupLineInterpolated($"\n[blue]{extraMessage}...[/]");
        }
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

    public ConsoleKeyInfo? ModifyOptionPrompt()
    {
        AnsiConsole.MarkupLine("[green]> Press ESC to return to main menu\n> Press E to edit\n> Press D to delete...[/]");
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
