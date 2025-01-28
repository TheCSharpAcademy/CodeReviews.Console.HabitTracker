using System.Globalization;
namespace HabitLogger
{
    public class InputHelpers
    {
        public static string GetDateInput()
        {
            Console.WriteLine("Insert the date: Format dd-mm-yy. 0 to return to menu");

            string? dateInput = Console.ReadLine();
            if (dateInput == "0") AppMenu.ShowMenu();
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date,Insert the date: Format dd-mm-yy,0 to return to menu ");
                dateInput = Console.ReadLine();
                if (dateInput == "0") AppMenu.ShowMenu();
            }

            return dateInput;
        }

        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string? numberInput = Console.ReadLine();
            if (numberInput == "0") AppMenu.ShowMenu();
            int parsedNumber;
            while (!int.TryParse(numberInput, out parsedNumber) || parsedNumber <= 0)
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine(message);
                numberInput = Console.ReadLine();
            }

            return parsedNumber;
        }
    }
}
