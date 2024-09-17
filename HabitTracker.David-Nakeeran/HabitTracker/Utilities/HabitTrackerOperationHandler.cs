using System.Globalization;
namespace HabitTracker.Utilities;
internal class HabitTrackerOperationHandler
{
    internal string GetDateInput(string? message)
    {
        Console.WriteLine(message);
        string? readResult = Console.ReadLine();
        readResult = CheckInputNullOrWhitespace(readResult, message);
        if (readResult == "0") return readResult;
        while (!isDateValid(readResult))
        {
            readResult = Console.ReadLine();
        }
        return readResult;
    }
    internal bool IsDateValid(string input)
    {
        if (!DateTime.TryParseExact(input, "dd-MM-yy", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\n Invalid date. (Format: dd-mm-yy). Try again:\n\n");
            return false;
        }
        return true;
    }
    internal int GetNumberInput(string message)
    {
        string? readResult;
        int inputNum;
        do
        {
            Console.WriteLine(message);
            readResult = Console.ReadLine();
            readResult = CheckInputNullOrWhitespace(readResult, message);
            inputNum = ParseInt(readResult, message);
        } while (inputNum < 0);
        return inputNum;
    }
    internal string CheckInputNullOrWhitespace(string? input, string message)
    {
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine(message);
            input = Console.ReadLine();
        }
        return input;
    }
    internal int ParseInt(string? input, string message)
    {
        int cleanNum;
        while (!int.TryParse(input, out cleanNum))
        {
            Console.WriteLine(message);
            input = Console.ReadLine();
        }
        return cleanNum;
    }
}