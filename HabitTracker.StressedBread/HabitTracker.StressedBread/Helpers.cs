using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.StressedBread
{
    internal class Helpers
    {
        internal string? ValidateString(string text)
        {
            Console.WriteLine(text);
            string? result = Console.ReadLine();

            while (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Your input is not valid!");
                result = Console.ReadLine();
            }
            return result;
        }
        internal int ValidateInt(string text)
        {
            Console.WriteLine(text);
            string? result = Console.ReadLine();

            while (string.IsNullOrEmpty(result) || !int.TryParse(result, out int num) || num < 0)
            {
                Console.WriteLine("Your input needs to be a positive whole number!");
                result = Console.ReadLine();
            }
            return int.Parse(result);
        }
        internal string? ValidateDate(string text)
        {
            Console.WriteLine(text);
            string? result = Console.ReadLine();
            string formatedDate = "dd/MM/yyyy";

            while (string.IsNullOrEmpty(result) || !DateTime.TryParseExact(result, formatedDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Your input is not in a correct format!");
                result = Console.ReadLine();
            }
            return result;
        }

        internal string? GetDateInput()
        {
            Console.WriteLine("Do you wish to input current date? Press Y for yes or N for no");
            ConsoleKeyInfo input = Console.ReadKey();
            switch (input.Key)
            {
                case ConsoleKey.Y:
                    Console.WriteLine();
                    return InputCurrentDate();
                case ConsoleKey.N:
                    return ValidateDate("\nInput date in dd/mm/yyyy format.");
                default:
                    return ValidateDate("\nInvalid input! Insert date manually in dd/mm/yyyy format.");
            }
        }
        internal string? InputCurrentDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}
