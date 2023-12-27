using System.Globalization;

namespace HabitTracker
{
    internal class Helpers
    {
        public static string connectionString = @"Data Source=habit-Tracker.db";
        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu.");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") MenuFunctions.UserMenu();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Please enter a correctly formatted date, or type 0 to return to the main menu:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0") MenuFunctions.UserMenu();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Please try again.");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}
