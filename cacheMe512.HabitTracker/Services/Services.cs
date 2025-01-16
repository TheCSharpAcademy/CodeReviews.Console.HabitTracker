using System;
using System.Globalization;

namespace habit_logger.Services
{
    public static class InputService
    {
        public static DateTime GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-MM-yy)");

            string dateInput = Console.ReadLine();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out var date))
            {
                Console.WriteLine("Invalid date format. Please try again:");
                dateInput = Console.ReadLine();
            }

            return DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"));
        }

        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            while (!int.TryParse(numberInput, out int number) || number < 0)
            {
                Console.WriteLine("Invalid number. Try again:");
                numberInput = Console.ReadLine();
            }

            return int.Parse(numberInput);
        }
    }
}
