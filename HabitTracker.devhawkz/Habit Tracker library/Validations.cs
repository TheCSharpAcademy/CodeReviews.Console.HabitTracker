using System.Globalization;

namespace Habit_Tracker_library;

internal static class Validations
{
    internal static string GetDate(string date)
    {
        if(date != "0")
        {
            while (!DateTime.TryParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.Clear();
                Console.WriteLine("Invalid date format.");
                Console.Write("Enter date in dd-mm-yyyy format: ");
                date = Console.ReadLine();

            }
        }

        return date;
    }

    internal static int IsValidNumber(string number)
    {
        bool isValid = int.TryParse(number, out int finalInput);

        while (!isValid || finalInput < 0)
        {
            Console.Clear();
            Console.WriteLine("Invalid number. Please enter a number equal or higher than 0.");
            Console.Write("Enter number: ");
            number = Console.ReadLine();
            isValid = int.TryParse(number, out finalInput);
        }

        return finalInput;
    }

    internal static string GetYear(string year)
    {
        if(year != "0") 
        {
            while (!DateTime.TryParseExact(year, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.Clear();
                Console.WriteLine("Invalid year.");
                Console.Write("Enter year in yyyy format: ");
                year = Console.ReadLine();
            }
        }

        return year;
    }

    internal static string GetMonth(string month) 
    {
        int mesec = 0;
        if (month != "0")
        {
            while (!int.TryParse(month, out mesec) && mesec > 12)
            {
                Console.Clear();
                Console.WriteLine("Invalid month.");
                Console.Write("Enter month in MM format: ");
                month = Console.ReadLine();
            }
        }

        return month;
    }
    
}
