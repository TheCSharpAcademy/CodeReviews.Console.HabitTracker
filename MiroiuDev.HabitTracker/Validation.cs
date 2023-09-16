using System.Globalization;

namespace MiroiuDev.HabitTracker;
internal static class Validation
{
    internal static string GetString()
    {
        string? input = Console.ReadLine()?.Trim();

        while (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Invalid input. Please try again!");
            input = Console.ReadLine()?.Trim();
        }

        return input;
    }

    internal static int GetNumber()
    {
        var num = GetString();

        if (num == "0") return 0;

        while (!int.TryParse(num, out _))
        {
            if (num == "0") return 0;

            Console.WriteLine("\n\nInvalid number. (No decimals allowed). Type 0 to return to main menu or try again:\n\n");
            num = GetString();
        }

        return int.Parse(num);
    }

    internal static string GetDateAsString()
    {
        var date = GetString();

        if (date == "0") return "0";

        while (!DateTime.TryParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            if (date == "0") return "0";

            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yyyy). Type 0 to return to main menu or try again:\n\n");
            date = GetString();
        }

        return date;
    }

    internal static void PrintStartSeparator()
    {
        Console.WriteLine("--------------------------------\n");
    }

    internal static void PrintEndSeparator()
    {
        Console.WriteLine("\n--------------------------------\n");
    }
}
