using System.Globalization;
using Spectre.Console;

namespace CodingTracker
{
    public static class Validations
    {
        public static string GetValidatedDate(string message)
        {
            string date = AnsiConsole.Prompt(
                    new TextPrompt<string>(message)
                        .PromptStyle("green")
                        .Validate(date =>
                        {
                            return (!DateTime.TryParseExact(date, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _)) ?
                            ValidationResult.Error("[red]Please Enter valid date format (MM/dd/yyyy)[/]") : ValidationResult.Success();
                        }));
            return date;
        }
        public static string GetValidatedTime(string message)
        {
            string time = AnsiConsole.Prompt(
                    new TextPrompt<string>(message)
                        .PromptStyle("green")
                        .Validate(time =>
                        {
                            return (!DateTime.TryParseExact(time, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _)) ?
                            ValidationResult.Error("[red]Please Enter valid time format (HH:mm)[/]") : ValidationResult.Success();
                        }));
            return time;
        }
        public static int GetValidatedInteger(string message)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>(message)
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Enter valid integer[/]"));
        }
        public static int GetValidatedKilometers(string message)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>(message)
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Enter valid integer[/]")
                .Validate(x =>
                {
                    return (x <= 0) ? ValidationResult.Error("[red]kilometers must be large than 0 [/]") : ValidationResult.Success();
                }));
        }
        public static bool ValidateStartAndEndTime(string startAt , string endAt)
        {
            DateTime start =  DateTime.Parse(startAt);
            DateTime end =  DateTime.Parse(endAt);

            return start.CompareTo(end) < 0;
        }
    }
}
