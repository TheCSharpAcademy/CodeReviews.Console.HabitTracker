// --------------------------------------------------------------------------------------------------
// HabitTracker.ConsoleApp.Utilities.ConsoleHelper
// --------------------------------------------------------------------------------------------------
// Helper methods for System.Console as that cannot be extended.
// --------------------------------------------------------------------------------------------------
using System.Globalization;

namespace HabitTracker.ConsoleApp.Utilities;

internal static class ConsoleHelper
{
    #region Methods: Internal

    internal static char GetChar(string message)
    {
        string? input = "";
        char output;

        // Display message and await input response.
        Console.WriteLine(message);
        input = Console.ReadLine();

        // Validation: Input must be something and an only one character.
        while (string.IsNullOrWhiteSpace(input) || input.Length != 1)
        {
            Console.WriteLine($"Invalid input. {message}");
            input = Console.ReadLine();
        }

        // Input has been validated..
        output = input.First();

        return output;
    }

    internal static int GetInt(string message)
    {
        string? input = "";
        int output;

        // Display message and await input response.
        Console.WriteLine(message);
        input = Console.ReadLine();

        // Validation: Input must be something and an integer.
        while (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out _))
        {
            Console.WriteLine($"Invalid input. {message}");
            input = Console.ReadLine();
        }

        // Input has been validated as an integer.
        output = int.Parse(input);

        return output;
    }

    internal static int GetInt(string message, int min)
    {
        string? input = "";
        int output;

        // Display message and await input response.
        Console.WriteLine(message);
        input = Console.ReadLine();

        // Validation: Input must be something and an integer.
        while (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out _) || int.Parse(input) < min)
        {
            Console.WriteLine($"Invalid input. {message}");
            input = Console.ReadLine();
        }

        // Input has been validated as an integer.
        output = int.Parse(input);

        return output;
    }

    internal static int GetInt(string message, int min, int max)
    {
        string? input = "";
        int output;

        // Display message and await input response.
        Console.WriteLine(message);
        input = Console.ReadLine();

        // Validation: Input must be something and an integer.
        while (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out _) || int.Parse(input) < min || int.Parse(input) > max)
        {
            Console.WriteLine($"Invalid input. {message}");
            input = Console.ReadLine();
        }

        // Input has been validated as an integer.
        output = int.Parse(input);

        return output;
    }

    internal static DateTime? GetDate(string message)
    {
        string? input = "";
        DateTime? output= null;

        Console.WriteLine(message);
        input = Console.ReadLine();

        if(!string.IsNullOrWhiteSpace(input) && input == "0")
        {
            return output;
        }

        while (string.IsNullOrWhiteSpace(input) || !DateTime.TryParseExact(input, "yyyy-MM-dd", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
        {
            if (!string.IsNullOrWhiteSpace(input) && input == "0")
            {
                return output;
            }

            Console.WriteLine($"Invalid input. {message}");
            input = Console.ReadLine();
        }

        output = DateTime.ParseExact(input, "yyyy-MM-dd", new CultureInfo("en-GB"));
        return output;
    }

    internal static string GetString(string message)
    {
        string? input = "";
        string output = "";

        // Display message and await input response.
        Console.WriteLine(message);
        input = Console.ReadLine();

        // Validation: Input must be something.
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine($"Invalid input. {message}");
            input = Console.ReadLine();
        }

        // Input has been validated..
        output = input;

        return output;
    }

    #endregion
}
