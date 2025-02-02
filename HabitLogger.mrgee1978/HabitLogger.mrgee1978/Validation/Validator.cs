using Spectre.Console;
using System.Globalization;

namespace mrgee1978.HabitLogger.Validation;

// This class is responsible for handing input validation
public class Validator
{
    /// <summary>
    /// Gets a valid string representing a date
    /// </summary>
    /// <param name="message"></param>
    /// <returns>A valid string in the required format for a date</returns>
    public string GetValidDateString(string message)
    {
        Console.WriteLine(message);
        string? dateString = Console.ReadLine();

        while (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
        {
            AnsiConsole.WriteLine("\n[red]Invalid date. (Format: dd-mm-yyyy). Please try again[/]");
            dateString = Console.ReadLine();
        }

        return dateString;
    }

    /// <summary>
    /// Gets a valid string that is neither null, empty or exceeds 100 characters
    /// </summary>
    /// <param name="message"></param>
    /// <returns>A valid non empty string</returns>
    public string GetValidString(string message)
    {
        Console.WriteLine(message);
        string? validString = Console.ReadLine();

        while (string.IsNullOrEmpty(validString) || validString.Length > 100)
        {
            AnsiConsole.WriteLine("\n[red]String cannot be null or empty or exceed 100 characters. Please try again[/]");
            validString = Console.ReadLine();
        }

        return validString;
    }

    /// <summary>
    /// Gets a valid integer
    /// </summary>
    /// <param name="message"></param>
    /// <returns>A valid positive integer</returns>
    public int GetValidNumericInput(string message)
    {
        Console.WriteLine(message);
        string? numericString = Console.ReadLine();

        int number = 0;
        while (!int.TryParse(numericString, out number) || Convert.ToInt32(numericString) < 0)
        {
            AnsiConsole.WriteLine("\n[red]Invalid input. Please enter a valid number[/]");
            numericString = Console.ReadLine();
        }

        return number;
    }
}
