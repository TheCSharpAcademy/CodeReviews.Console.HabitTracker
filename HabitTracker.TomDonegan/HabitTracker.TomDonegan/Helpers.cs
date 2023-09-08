using System.Globalization;

namespace HabitTracker.TomDonegan
{
    internal class Helpers
    {
        internal static void DisplayHeader(string header)
        {
            if (header.Contains('_'))
            {
                header = header.Replace('_', ' ');
            }

            header = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(header);

            Console.WriteLine("-----------------------------");
            Console.WriteLine(header);
            Console.WriteLine("-----------------------------\n");
        }
    }
}
