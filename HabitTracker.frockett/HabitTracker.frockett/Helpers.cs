using System.Globalization;

namespace HabitTracker.frockett;

internal class Helpers
{
    public string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format dd-mm-yy). Type 0 to return to main menu\n\n");

        string? dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime cleanDate))
        {
            Console.WriteLine("\nInvalid date. Format must be (dd-mm-yy). Enter 0 to return to main menu\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }
    public int GetNumberInput(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        string? quantityInput = Console.ReadLine();

        if (!int.TryParse(quantityInput, out int cleanQuantityInput))
        {
            Console.WriteLine("Invalid input, please enter a whole number");
            quantityInput = Console.ReadLine();
        }
        return cleanQuantityInput;
    }



}
