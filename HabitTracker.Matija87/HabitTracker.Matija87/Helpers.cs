using System.Globalization;

namespace HabitTracker.Matija87
{
    internal static class Helpers
    {
        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nInsert date (format dd-mm-yy) or type 0 to return to Main Menu");

            string? dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                Program.MainMenu();
            }

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date! (format: dd-mm-yy). Enter date or 0 to return to Main Menu");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string? numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                Program.MainMenu();
            }

            while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number! Try again!\n\n");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}
