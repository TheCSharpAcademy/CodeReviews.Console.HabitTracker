using HabitTracker.TruthfulUK.Helpers;
using Spectre.Console;

namespace HabitTracker.TruthfulUK;
internal static class HabitManager
{
    internal static void AddNewHabit() {         
        
        var habitName = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the name of the habit:").Validate(name =>
            {
                return string.IsNullOrWhiteSpace(name) ? ValidationResult.Error("Habit name cannot be empty.") : ValidationResult.Success();
            }));

        var measurement = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the measurement unit for this habit (e.g., ml, miles, km:").Validate(unit =>
            {
                return string.IsNullOrWhiteSpace(unit) ? ValidationResult.Error("Measurement cannot be empty.") : ValidationResult.Success();
            }));

        DbHelpers.AddHabit(habitName, measurement);

        Console.WriteLine();
        AnsiConsole.MarkupLine($"Habit [blue]{habitName}[/] with measurement [blue]{measurement}[/] was added successfully!");
        Console.WriteLine();
    }
}
