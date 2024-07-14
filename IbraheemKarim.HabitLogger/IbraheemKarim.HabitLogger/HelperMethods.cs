using System.Globalization;

namespace IbraheemKarim.HabitLogger
{
    public static class HelperMethods
    {        
        public static int GetPositiveIntegerFromUser(string integerName)
        {
            bool firstIteration = true;
            do
            {
                if (!firstIteration)
                    Console.WriteLine("Invalid input!! \n");

                PromptUserToEnterANumber(integerName);

                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    if (number >= 0)
                        return number;
                }

                firstIteration = false;
            } while (true);
        }

        public static DateTime GetDateFromUser()
        {
            bool firstIteration = true;
            do
            {
                Console.Clear();

                if (!firstIteration)
                    Console.WriteLine("Invalid input!! \n");

                Console.WriteLine("Please enter a date in the format dd-mm-yyyy:");
                Console.WriteLine("---------------------------------------------");
                var dateString = Console.ReadLine();

                if (DateTime.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
                {
                    return dateValue;
                }

                firstIteration = false;
            } while (true);
        }

        public static void PauseAppUntilUserPressesAkey()
        {
            Console.WriteLine("\nPress any key to go back to the main menu...");
            Console.ReadKey();
        }

        public static void PromptUserToEnterANumber(string integerName)
        {
            Console.WriteLine($"Please enter {integerName}:");
            Console.WriteLine("---------------------------------------------");
        }
    }
}
