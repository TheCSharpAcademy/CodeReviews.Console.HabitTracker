using System.Text.RegularExpressions;

namespace HabitTracker
{
    public static class Helpers
    {
        public static string InputStringWithValidation()
        {
            while (true)
            {
                string? input = Console.ReadLine();
                if (input != null && input != Environment.NewLine && input.Length > 0)
                {
                    return input;
                }
                Console.WriteLine("Wrong line. Please write a non-null, non-newline symbol and a length greater than zero...");
            }
        }

        public static string InputStringWithRegexValidation(string regex, string infoMessage)
        {
            while (true)
            {
                string? input = Console.ReadLine();
                if (input != null && Regex.IsMatch(input, regex))
                {
                    return input;
                }
                Console.WriteLine(infoMessage);
            }
        }

        public static int InputNumberWithValidation(int min = 0, int max = 0)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    if (min != max && number >= min && number <= max)
                    {
                        return number;
                    }
                    else if (min == max)
                    {
                        return number;
                    }
                }
                Console.WriteLine("Need input a number!");
            }
        }

        public static DateTime InputDataWithValidation(string pattern, DateTime min, DateTime max, string infoMessage)
        {
            while (true)
            {
                string? input = Console.ReadLine();
                if (DateTime.TryParseExact(input, pattern, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate)
                    && parsedDate <= max && parsedDate > min)
                {
                    return parsedDate;
                }
                Console.WriteLine(infoMessage);
            }
        }
    }
}
