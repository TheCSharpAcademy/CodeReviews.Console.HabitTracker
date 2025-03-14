using System.Text.RegularExpressions;
using Tomi.HabitTracker.Data;
using System.Globalization;

namespace Tomi.HabitTracker.Prompts;

internal class HabitsPrompts
{

    public static void PrintAppMenu()
    {
        Console.WriteLine("\nMain Menu\n");
        Console.WriteLine("What will you like to do ?");
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View All Records");
        Console.WriteLine("Type 2 to Insert Records");
        Console.WriteLine("Type 3 to Delete Records");
        Console.WriteLine("Type 4 to Update Records");
        Console.Write("Your option: ");
    }

    public static string PromptForMenu()
    {
        PrintAppMenu();
        string input;

        while (true)
        {
            input = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrEmpty(input) && Regex.IsMatch(input, "[0|1|2|3|4]"))
            {
                break;
            }
            Console.WriteLine("oops! wrong option selected.. ");
            PrintAppMenu();
        }

        return input;
    }


    public static (string HabitDate, string Habit, double Quantity) GetHabitDetails()
    {
        string validatedDate = PromptForDate();
        string habit = GetTextInput("What Habit do you wanna log?");
        double quantity = PromptForNumericInput();

        return (validatedDate, habit, quantity);
    }

    public static string PromptForDate()
    {
        Console.WriteLine("\nWhat date do you wanna log for this habit? (Date Format: yyyy-mm-dd ). Type d for today's date");
        string habitDate;
        while (true)
        {
            Console.Write("\nEnter date: ");
            habitDate = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;

            if (string.IsNullOrEmpty(habitDate))
            {
                Console.WriteLine("Date cannot be empty, enter date");
                continue;
            }
            else if (habitDate == "d")
                habitDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool isValidDte = IsValidDate(habitDate);

            if (!isValidDte)
            {
                Console.WriteLine("You may have entered an invalid date.. (Date Format: yyyy-mm-dd )");
                continue;
            }

            break;
        }

        return habitDate;
    }

    public static string GetTextInput(string PromptMessage)
    {
        string input;
        Console.WriteLine(PromptMessage);

        while (true)
        {
            Console.WriteLine("Your response: ");
            input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                Console.Write("Your response cannot be empty");
                continue;
            }

            break;
        }

        return input;
    }

    public static int PromptForId()
    {
        int id;
        while (true)
        {
            string inputId = GetTextInput("\nEnter the Record ID");
            if (!int.TryParse(inputId, out id))
            {
                Console.WriteLine("Enter a non-floating numeric value");
                continue;
            }
            break;
        }

        return id;
    }

    public static double PromptForNumericInput()
    {
        double quantity;

        while (true)
        {
            string response = GetTextInput("\nWhat Quantity do you wanna log?");

            if (!double.TryParse(response, out quantity))
            {
                Console.WriteLine("Invalid format.. enter a numeric value");
                continue;
            }

            break;
        }

        return quantity;
    }

    public static bool IsValidDate(string dateString)
    {
        try
        {

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            bool isValid = DateTime.TryParseExact(dateString, "yyyy-MM-dd", culture, DateTimeStyles.None, out DateTime convertedDate);

            if (isValid)
            {
                int comparisonResult = DateTime.Compare(convertedDate, DateTime.Now);
                if (comparisonResult > 0)
                {
                    Console.WriteLine("Date shouldn't be later than today..");
                    isValid = false;
                }
            }

            return isValid;
        }
        catch (Exception)
        {
            Console.WriteLine($"There was an error processing the date..");
            return false;
        }
    }



}
