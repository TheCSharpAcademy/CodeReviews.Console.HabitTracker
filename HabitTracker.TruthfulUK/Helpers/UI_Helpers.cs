using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Spectre.Console;

namespace HabitTracker.TruthfulUK.Helpers;
internal static class UIHelpers
{
    public static Dictionary<string, TEnum> GetMenuOptions<TEnum>()
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .ToDictionary(option => GetEnumDisplayName(option), option => option);
    }

    public static string GetEnumDisplayName(Enum value)
    {
        return value.GetType()
            .GetMember(value.ToString())[0]
            .GetCustomAttribute<DisplayAttribute>()?.Name
            ?? value.ToString();
    }

    public static void PressKeyToContinue()
    {
        AnsiConsole.MarkupLine("[blue]Press Any Key to return to the menu...[/]");
        Console.ReadKey();
    }

    public static string AskForHabitSelection()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select an [blue]option[/]:")
                .AddChoices(DB_Helpers.SelectHabits()));
    }

    public static string FormatDouble(double value)
    {
        return value % 1 == 0 ? ((int)value).ToString("N0") : value.ToString("N1");
    }
}
