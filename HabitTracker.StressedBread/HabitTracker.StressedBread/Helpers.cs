using System.ComponentModel.Design;

namespace HabitTracker.StressedBread
{
    internal class Helpers
    {
        internal string? ValidateString(string result)
        {
            while (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Your input is not in a correct format!");
                result = Console.ReadLine();
            }
            return result;
        }
        internal int ValidateInt(string result)
        {
            while (string.IsNullOrEmpty(result) || !int.TryParse(result, out _))
            {
                Console.WriteLine("Your input needs to be a whole number!");
                result = Console.ReadLine();
            }
            return int.Parse(result);
        }
    }
}
