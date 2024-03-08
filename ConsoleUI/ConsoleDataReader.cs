using System.Globalization;

namespace ConsoleUI
{
    public static class ConsoleDataReader
    {
 

        public static string GetStringFromConsole(string message, string? regex = null)
        {
            string result;
            do
            {
                Console.Write($"{message}: ");
                result = Console.ReadLine().Trim().ToLower();
            } while (string.IsNullOrEmpty(result));
            return result;
        }

        public static int GetIntFromConsole(string message, string? regex = null)
        {
            int result;
            do
            {
                Console.Write($"{message}: ");
            }
            while (!int.TryParse(Console.ReadLine(), out result));

            return result;
        }

        public static int ConsoleReadRangedInt(string message, int lowerbound, int upperbound)
        {
            int result;
            do
            {
                result = GetIntFromConsole(message);
            } while (!result.IsInRange(lowerbound, upperbound));

            return result;

        }
        public static bool WantToRunAgain(string message)
        {
            string result;

            Console.Write($"{message}: ");
            result = Console.ReadLine().Trim().ToLower();
            return result != "no" && result != "n";
        }

        //extension
        public static bool IsInRange(this int x, int lowerbound, int upperbound)
        {
            return x >= lowerbound && x < upperbound;
        }

        public static DateTime ConsoleReadDate(string message)
        {
            DateTime result;
            string userDateTimeInput;
            do
            {
                Console.Write($"{message}: ");
                userDateTimeInput = Console.ReadLine().Trim();

            } while (!DateTime.TryParse(userDateTimeInput, new CultureInfo("en-US"), out result));

            return result;
        }
    }
}
