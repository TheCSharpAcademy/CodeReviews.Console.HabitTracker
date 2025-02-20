using System.Globalization;
using HabitLogger.mrgee1978.DomainLayer.Interfaces;

namespace HabitLogger.mrgee1978.DomainLayer;

public class ValidationOfRecord : IValidation
{
    public int GetValidInteger(string message, string info)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        int validInteger;
        string errorMessage = $"Please input a valid {info} ";

        while (!int.TryParse(input, out validInteger))
        {
            Console.Write(errorMessage);
            input = Console.ReadLine();
        }

        return validInteger;
    }

    public string GetValidString(string message, string info)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        string errorMessage = $"{info} cannot be empty or null! Please enter a valid {info} ";

        while (string.IsNullOrEmpty(input))
        {
            Console.Write(errorMessage);
            input = Console.ReadLine();
        }

        return input;
    }

    public string GetValidDateString(string message, string info)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        if (input.Equals("0"))
        {
            return "0";
        }

        string errorMessage = $"Invalid date entered! Format: dd-mm-yy Please try again {info} ";

        while (!DateTime.TryParseExact(input, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            Console.Write(errorMessage);
            input = Console.ReadLine();
        }

        return input;
    }
}
