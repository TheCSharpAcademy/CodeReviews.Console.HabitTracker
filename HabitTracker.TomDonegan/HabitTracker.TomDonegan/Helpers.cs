using System.Globalization;

namespace HabitTracker.TomDonegan
{
    internal class Helpers
    {
        /*internal static int NumberValidation(string requiresValidation)
        {
            int checkedNumber;

            while (!int.TryParse(requiresValidation, out checkedNumber)) 
            {
                Console.WriteLine("Please enter a valid number.");
                Console.ReadLine();
            }

            return checkedNumber;
        }*/

        internal static void DisplayHeader(string header)
        {
            if (header.Contains('_')) {
                header = header.Replace('_', ' ');
            }

            header = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(header);

            Console.WriteLine("-----------------------------");
            Console.WriteLine(header);
            Console.WriteLine("-----------------------------\n");
        }
    }
}
