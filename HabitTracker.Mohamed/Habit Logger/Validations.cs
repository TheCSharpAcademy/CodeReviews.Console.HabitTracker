using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Logger
{
    public static class Validations
    {

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                Console.Clear();
                Program.StartMenu();
            }
               

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {

                Helper.printError("Invalid number. Try again.");
               numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }


        internal static string GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.");

            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                Console.Clear();
                Program.StartMenu();
            }

            while (!DateTime.TryParseExact(dateInput , "MM/dd/yyyy hh:mm tt" , new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Helper.printError("Invalid date. (Format: MM/dd/yyyy hh:mm tt). Type 0 to return to main manu or try again:");
                dateInput = Console.ReadLine();
            }

            return dateInput.Trim();
        }
    }
}
