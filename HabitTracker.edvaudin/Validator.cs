namespace HabitTracker.edvaudin;

public static class Validator
{
    public static bool IsValidOption(string? input)
    {
        string[] validOptions = { "v", "h", "a", "d", "u", "c", "0" };
        foreach (string validOption in validOptions)
        {
            if (input == validOption)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsPositiveIntInput(string input) => int.TryParse(input, out int parsed) && parsed > 0;

    public static bool IsIntInputWithResult(out int result) => int.TryParse(Console.ReadLine(), out result);

    public static bool IsNonEmptyNonFullString(string input) => input.Length == 0 || input.Length > 255;

    public static bool IsValidDateInput(string input) => DateTime.TryParse(input, out DateTime date);

    public static bool IsExitCode(string input) => input == "-1";
}
