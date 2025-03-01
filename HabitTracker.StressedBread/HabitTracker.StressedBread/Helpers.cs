using System.Globalization;

namespace HabitTracker.StressedBread
{
    internal class Helpers
    {
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
        internal string? InputCurrentDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}
