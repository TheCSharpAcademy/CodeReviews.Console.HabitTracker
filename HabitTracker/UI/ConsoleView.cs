using HabitTracker.Application.DTOs;
using HabitTracker.UI.Interfaces;

namespace HabitTracker.UI;

public class ConsoleView : IView
{
    public void DisplayHeader()
    {
        Console.WriteLine("<><><> Habit Tracking App <><><><><><><><><><><><><><><><><><><>");
        Console.WriteLine("This app allows to track habits you might undertake.");
    }

    public void DisplayMainMenuOptions(IEnumerable<string> options)
    {
        Console.WriteLine();
        int i = 0;
        foreach (var option in options)
        {
            Console.WriteLine($"{++i}. {option}");
        }
    }

    public HabitCreationDto GetNewHabit()
    {
        var name = GetUserNonNullString("name");
        var unit = GetUserNonNullString("unit of measure");
        return new HabitCreationDto(name, unit);
    }

    private string GetUserNonNullString(string what)
    {
        Console.WriteLine($"Please enter a {what}.");
        do
        {
            var entry = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(entry)) Console.WriteLine("Please enter something!");
            else return entry;
        } while (true);
    }
    
    public void DisplayHabits(IEnumerable<HabitDisplayDto> habits)
    {
        Console.WriteLine();
        int i = 0;
        foreach (var habit in habits)
        {
            Console.WriteLine($"{++i} - {habit.Name}");
        }
    }
    public void DisplayHabit(HabitDisplayDto habit, OccurrenceDisplayDto? lastOccurrence)
    {
        Console.WriteLine($"{habit.Name} Details :");
        Console.WriteLine($"Occurrences: {habit.Occurrences} {habit.Unit}");
        if (lastOccurrence != null)
        {
            Console.WriteLine($"Last occurrence was on {lastOccurrence.Date}");
        }
    }
    
    public int GetUserChoice()
    {
        Console.WriteLine("Please enter a number corresponding to the desired option.");
        bool success = false;
        do
        {
            success = Int32.TryParse(Console.ReadLine(), out var selection);
            if (!success) Console.WriteLine("Please enter a valid number.");
            else return selection;
        } while (true);
    }

    public void DisplayOccurrences(IEnumerable<OccurrenceDisplayDto> occurrences)
    {
        foreach (var occurrence in occurrences)
        {
            Console.WriteLine(occurrence);
        }
    }

    public OccurrenceCreationDto GetUserOccurrence(int habitId)
    {
        var date = GetUserDate();
        return new OccurrenceCreationDto(date, habitId);
    }

    private string GetUserDate()
    {
        Console.WriteLine("Please enter a date.");
        bool success = false;
        do
        {
            var entry = Console.ReadLine();
            success = DateTime.TryParse(entry, out _);
            if (entry == null) Console.WriteLine("Please enter something!");
            else if (!success) Console.WriteLine("Please enter a valid date.");
            else return entry;
        } while (true);
    }

    public HabitUpdateDto GetUserHabitModification()
    {
        var name = GetUserNullableString("name");
        var unit = GetUserNullableString("unit of measure");
        return new HabitUpdateDto(name, unit);
    }

    private string? GetUserNullableString(string what)
    {
        Console.WriteLine($"Please enter a {what}, press Enter to skip.");
        var entry = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(entry)) return null;
        else return entry;
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ClearMessages()
    {
        Console.Clear();
    }
}