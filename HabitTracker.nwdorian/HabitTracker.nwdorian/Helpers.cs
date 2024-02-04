using Spectre.Console;
using System.Globalization;

namespace HabitTracker.nwdorian;

internal class Helpers
{
    internal static string GetDateInput()
    {
        Console.Write("Please insert the date (format: yy-MM-dd): ");

        string dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "yy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.Write("Invalid date. (format: yy-MM-dd): ");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(message)
            .ValidationErrorMessage("[red]Invalid input.[/]")
            .Validate(num =>
            {
                return num switch
                {
                    <= 0 => ValidationResult.Error("[red]Number must be bigger then 0[/]"),
                    _ => ValidationResult.Success()
                };
            })
        );
    }

    internal static string GetHabitByName(string message)
    {
        var habit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(message)
                .PageSize(10)
                .MoreChoicesText("")
                .AddChoices(DbMethods.GetAllHabits()));
        return habit;
    }
}
