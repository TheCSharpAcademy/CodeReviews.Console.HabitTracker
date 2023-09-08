using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HabitTracker.TomDonegan
{
    internal static class InputValidation
    {
        internal static double GetQuantityInput()
        {
            Console.WriteLine(
                "Please insert the quantity. Type 0 to return to the main menu."
            );

            string quantityInput = Console.ReadLine();

            while (!double.TryParse(quantityInput, out _))
            {
                Console.WriteLine("Your quantity must be a number. Try again.");
                quantityInput = Console.ReadLine();
            }

            if (quantityInput == "0")
                UserInterface.MainMenu();

            return double.Parse(quantityInput);
        }

        internal static string GetDateInput()
        {
            Console.WriteLine(
                "\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu."
            );

            string dateInput = Console.ReadLine();

            if (dateInput == "0")
                UserInterface.MainMenu();

            string requiredFormat = @"\d{2}-\d{2}-\d{2}";

            while (!Regex.IsMatch(dateInput, requiredFormat) || dateInput.Length < 6)
            {
                Console.WriteLine(
                    "Please enter the date in the required format and length. Try again."
                );
                dateInput = Console.ReadLine();
            }
            ;

            int dayNumber = Convert.ToInt32(dateInput[..2]);
            int monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));

            while (dayNumber < 01 || dayNumber > 31 || monthNumber < 01 || monthNumber > 12)
            {
                Console.WriteLine(
                    "Day date must be between 01 and 31.\nMonth date must be between 01 and 12. Try again."
                );

                dateInput = Console.ReadLine();
                dayNumber = Convert.ToInt32(dateInput[..2]);
                monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));
            }
            ;

            return dateInput;
        }
    }
}
