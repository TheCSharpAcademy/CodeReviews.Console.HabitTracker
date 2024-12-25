using System.Globalization;
using Spectre.Console;

internal static class HelpersGeneral
{
    public const string DateFormat = "yyyy-MM-dd";

    internal static void PressAnyKeyToContinue()
    {
        AnsiConsole.Status()
            .Start("[yellow]Press any key to continue[/]", ctx =>
            {
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("yellow"));
                Console.ReadKey(true);
            });
        Console.Clear();
    }

    internal static DateTime GetDateInput()
    {
        string message = $"Enter date (\"{DateFormat}\" format or press 'Enter' key for today's date): ";
        AnsiConsole.Markup(message);
        string? dateInput = Console.ReadLine();

        while (true)
        {
            dateInput ??= "";
            if (dateInput.Trim() == "")
            {
                dateInput = DateTime.Now.ToString(DateFormat);
                break;
            }
            else if (DateTime.TryParseExact(dateInput, DateFormat,
                new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedDate) && parsedDate > DateTime.Now)
            {
                AnsiConsole.Markup("[red]Entered date can not exceed today's date, retry.[/]\n" +
                    $"Enter other date or press Enter key for today's date: ");
                dateInput = Console.ReadLine();
            }
            else if (!DateTime.TryParseExact(dateInput, DateFormat,
                new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                AnsiConsole.Markup($"[red]Invalid input, retry.[/]\n{message}");
                dateInput = Console.ReadLine();
            }
            else break;
        }

        DateTime.TryParseExact(dateInput, DateFormat,
                new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputDate);
        return inputDate;
    }

    internal static string GetChoiceFromSelectionPrompt(string message, IEnumerable<string> choices)
    {
        var selectedChoice = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                   .Title(message)
                   .PageSize(10)
                   .AddChoices(Database.BackWord)
                   .AddChoices(choices)
               );
        return selectedChoice;
    }

    internal static void DisplayOneRecord(DateTime date, string habitName, int value, string measurementUnit)
    {
        AnsiConsole.MarkupLine($"[yellow]=>[/] {date.ToString(DateFormat)}: [yellow]{habitName}[/] - [green]{value} {measurementUnit}[/]");
        PressAnyKeyToContinue();
    }

    internal static (bool shouldExit, DateTime date, string habitName) GetDateAndHabitName()
    {
        DateTime date = GetDateInput();
        AnsiConsole.MarkupLine($"Date: [yellow]{date.Date.ToString(DateFormat)}[/]");
        string habitName = "";

        while (true)
        {
            habitName = HelpersSql.GetHabitName();

            if (habitName == Database.BackWord)
            {
                Console.Clear();
                return (true, DateTime.Now, string.Empty);
            }
            else if (habitName == Database.CreateHabitName)
            {
                var newHabitName = CrudHabitNames.CreateNewHabitName();
                if (newHabitName == Database.BackWord)
                {
                    Console.Clear();
                    return (true, DateTime.Now, string.Empty);
                }
                return (false, date, newHabitName);
            }
            else if (habitName == Database.DeleteHabitName)
            {
                CrudHabitNames.DeleteHabitName();
                return (true, DateTime.Now, string.Empty);
            }
            else
            {
                AnsiConsole.MarkupLine($"Chosen habit: [yellow]{habitName}[/]");
                return (false, date, habitName);
            }
        }
    }

    internal static int GetPositiveNumberInput(string message)
    {
        var input = AnsiConsole.Ask<int>(message);
        while (true)
        {
            if (input <= 0)
            {
                AnsiConsole.Markup("[red]Invalid input. Only positive numbers accepted.[/]\n");
                input = AnsiConsole.Ask<int>("Enter a valid number:");
            }
            else break;
        }
        return input;
    }

    internal static bool GetYesNoAnswer(string message)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(message)
                .PageSize(10)
                .AddChoices("Yes")
                .AddChoices("No")
            );
        return (choice == "Yes"); 
    }
}
