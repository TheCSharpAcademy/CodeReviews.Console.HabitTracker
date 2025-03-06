using System.Globalization;

namespace Erix101.HabitTracker.Services
{

    internal class UserInputServices
    {
        internal static string GetStringInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        internal static int GetNumberInput(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                string numberInput = Console.ReadLine();
                if (int.TryParse(numberInput, out int result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("\nInvalid input.  Please enter an integer.\n");
                }
            }
        }

        internal static DateTime GetDateInput()
        {
            while (true)
            {
                Console.WriteLine("\n\nPlease insert date: (Format: dd-mm-yy) or type 'Today' to enter todays date");
                string dateInput = Console.ReadLine();

                if (dateInput.ToLower() == "today")
                {
                    return DateTime.Today;
                }
                else if (DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
                else
                {
                    Console.WriteLine("\nInvalid date format. Please enter a valid date (dd-MM-yy).\n");
                }
            }
        }
    }
}
