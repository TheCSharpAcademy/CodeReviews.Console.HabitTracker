using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_tracker.Ibrahim;

public class helper
{
    public static string ValidateChoice(string menuOptionSelected)
    {
        string retVal = "";
        int number;
        bool result;

        do
        {
            result = int.TryParse(menuOptionSelected, out number);
            if (result)
            {
                if (number <= 5 && number >= 0)
                {
                    retVal = menuOptionSelected;
                }
                else
                {
                    result = false;
                    Console.WriteLine($"please enter a number between 0 and 5");
                    menuOptionSelected = Console.ReadLine();
                }
            }
            else //not a number
            {
                result = false;
                Console.WriteLine($"please enter a number between 0 and 5");
                menuOptionSelected = Console.ReadLine();
            }
        }
        while (!result);

        return retVal;

    }

    public static string ValidateInt(string numOfPrayers)
    {
        string retVal = "";
        int number;
        bool result;

        do
        {
            result = int.TryParse(numOfPrayers, out number);
            if (result)
            {
                if (number <= 5 && number >= 0)
                {
                    result = true;
                    retVal = numOfPrayers;
                }
                else
                {
                    result = false;
                    Console.WriteLine($"please enter a number between 0 and 5");
                    numOfPrayers = Console.ReadLine();
                }
            }
            else //not a number
            {
                result = false;
                Console.WriteLine($"please enter a number between 0 and 5");
                numOfPrayers = Console.ReadLine();
            }
        }
        while (!result);

        return retVal;
    }

    public static string ValidateDate(string date)
    {
        string retVal = "";
        DateTime dateValue;
        bool isValidDate;
        do
        {
            isValidDate = DateTime.TryParseExact(
            date,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out dateValue
        ) && dateValue.Year > 0;
            if (isValidDate)
            {
                retVal = date;
            }
            else
            {
                Console.WriteLine($"'{date}' is invalid. Please try again");
                date = Console.ReadLine();
            }

        }
        while (!isValidDate);

        return retVal;
    }
}
