using HabitTrackerConsole.Services;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace HabitTrackerConsole.Util;

/// <summary>
/// ApplicationHelper provides utility methods to enhance the user interface interactions and string manipulations.
/// It includes methods for displaying messages, parsing enums, and splitting camel case strings.
/// </summary>
public class ApplicationHelper
{
    /// <summary>
    /// Writes a markup message to the console with a newline.
    /// </summary>
    /// <param name="message">Markup object containing the message to be displayed.</param>
    public static void AnsiWriteLine(Markup message)
    {
        AnsiConsole.Write(message);
        Console.WriteLine();
    }

    /// <summary>
    /// Converts a string with spaces into an enum value, ignoring case.
    /// </summary>
    /// <param name="friendlyString">String to be converted into an enum.</param>
    /// <returns>Enum value corresponding to the string.</returns>
    public static T FromFriendlyString<T>(string friendlyString) where T : struct, Enum
    {
        string enumString = friendlyString.Replace(" ", "");
        return Enum.Parse<T>(enumString, true);
    }

    /// <summary>
    /// Splits a camelCase string into words separated by spaces.
    /// </summary>
    /// <param name="input">The camelCase string to split.</param>
    /// <returns>A string with each word separated by a space.</returns>
    public static string SplitCamelCase(string input)
    {
        return Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
    }

    /// <summary>
    /// Pauses execution and waits for the user to press any key, displaying a prompt message.
    /// </summary>
    public static void PauseForContinueInput()
    {
        AnsiConsole.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
