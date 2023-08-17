using System.Globalization;

namespace HabitTracker.lordWalnuts
{
    public static class Helpers
    {
        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.\n\n");
            string dateInput = Console.ReadLine();
            if (dateInput == "0") Program.ShowMenu();


            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 for main menu:\n\n");
                dateInput = Console.ReadLine();
                if (dateInput == "0") Program.ShowMenu();
            }
            return dateInput;
        }

        internal static string GetHabitInput()
        {
            Console.WriteLine("\n\nPlease Enter the Habit name. Type 0 to return to main manu");
            string habitInput = Console.ReadLine();
            if (habitInput == "0") Program.ShowMenu();

            while (string.IsNullOrEmpty(habitInput) || char.IsDigit(habitInput[0]))
            {
                Console.WriteLine("\n\nPlease Enter a valid Habit name. Type 0 for main manu");
                habitInput = Console.ReadLine();
                if (habitInput == "0") Program.ShowMenu();
            }
            return habitInput;
        }

        internal static string GetUnitInput()
        {
            Console.WriteLine("\n\nPlease Enter the Unit of Measurement. Type 0 to return to main manu");
            string unitInput = Console.ReadLine();
            if (unitInput == "0") Program.ShowMenu();

            while (string.IsNullOrEmpty(unitInput) || char.IsDigit(unitInput[0]))
            {
                Console.WriteLine("\n\nPlease Enter a valid Unit of Measurement. Type 0 for main manu");
                unitInput = Console.ReadLine();
                if (unitInput == "0") Program.ShowMenu();
            }
            return unitInput;
        }

        internal static string GetNumberInput(string message)
        {
            Console.WriteLine(message);
            var numberInput = Console.ReadLine();
            if (numberInput == "0") Program.ShowMenu();

            while (string.IsNullOrEmpty(numberInput) || !Int32.TryParse(numberInput, out _) || int.Parse(numberInput) < 0)
            {
                Console.WriteLine("\n\nYour answer needs to be an positive integer.Try again or 0 for main menu");
                numberInput = Console.ReadLine();
                if (numberInput == "0") Program.ShowMenu();
            }
            return numberInput;
        }
    }
}