using System.Globalization;
namespace HabitTracker.joshluca98;

public static class Helper
{
    public static string GetDateInput()
    {
        Console.WriteLine("\nPlease type a date (dd-mm-yy) or type 0 to return to the menu:\n");
        string dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            if (dateInput == "0") return dateInput;
            Console.WriteLine("\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again:\n");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }

    public static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string numberInput = Console.ReadLine();
        int finalInput;

        while (!int.TryParse(numberInput, out finalInput) || finalInput < 0)
        {
            Console.WriteLine("\nPlease enter a valid number..\n");
            numberInput = Console.ReadLine();
        }
        return finalInput;
    }
}