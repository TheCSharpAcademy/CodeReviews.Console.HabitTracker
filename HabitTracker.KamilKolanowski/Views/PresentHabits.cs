using HabitTracker.KamilKolanowski.Data;
using Microsoft.Data.Sqlite;

namespace HabitTracker.KamilKolanowski.Views;

public class HabitsPresentator
{
    public void PresentTitle()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Habit Logger\n");
        Console.ResetColor();
    }

    public void PresentHabits(DatabaseManager db, SqliteConnection connection)
    {
        var habits = db.ListHabits(connection);

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Habits List");
        Console.ResetColor();
        Console.WriteLine();

        int idWidth = 7;
        int habitWidth = 20;
        int quantityWidth = 12;
        int unitWidth = 12;
        int dateWidth = 26;
    
        string divider = $"├{new string('─', idWidth)}┼{new string('─', habitWidth)}┼{new string('─', quantityWidth)}┼{new string('─', unitWidth)}┼{new string('─', dateWidth)}┤";

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"│{"Id".PadCenter(idWidth)}│{"Habit".PadCenter(habitWidth)}│{"Quantity".PadCenter(quantityWidth)}│{"Unit".PadCenter(unitWidth)}│{"Insert Date".PadCenter(dateWidth)}│");
        Console.WriteLine(divider);
        Console.ResetColor();

        foreach (var habit in habits)
        {
            var habitLines = habit.Value.Item2.WrapText(habitWidth);
            var unitLines = habit.Value.Item4.WrapText(unitWidth);
            var dateLines = habit.Value.Item5.WrapText(dateWidth);

            int maxLines = new[] { habitLines.Count, unitLines.Count, dateLines.Count }.Max();

            for (int i = 0; i < maxLines; i++)
            {
                string id = i == 0 ? habit.Key.ToString().PadCenter(idWidth) : new string(' ', idWidth);
                string habitText = i < habitLines.Count ? habitLines[i].PadCenter(habitWidth) : new string(' ', habitWidth);
                string quantity = i == 0 ? habit.Value.Item3.ToString().PadCenter(quantityWidth) : new string(' ', quantityWidth);
                string unit = i < unitLines.Count ? unitLines[i].PadCenter(unitWidth) : new string(' ', unitWidth);
                string date = i < dateLines.Count ? dateLines[i].PadCenter(dateWidth) : new string(' ', dateWidth);

                Console.WriteLine($"│{id}│{habitText}│{quantity}│{unit}│{date}│");
            }
            Console.WriteLine(divider);
        }
    }

    
    public void PresentReport(DatabaseManager db, SqliteConnection connection)
    {
        var report = db.CreateReport(connection);

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Habits Summary Report");
        Console.ResetColor();
        Console.WriteLine();

        int habitWidth = 20;
        int timeSpentWidth = 15;
        int uomWidth = 10;
    
        string divider = $"├{new string('─', habitWidth)}┼{new string('─', timeSpentWidth)}┼{new string('─', uomWidth)}┤";

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"│{"Habit".PadCenter(habitWidth)}│{"Time Spent".PadCenter(timeSpentWidth)}│{"UoM".PadCenter(uomWidth)}│");
        Console.WriteLine(divider);
        Console.ResetColor();

        foreach (var habitSummarized in report)
        {
            var habitLines = habitSummarized.Item1.WrapText(habitWidth);
            var timeSpent = habitSummarized.Item2.ToString().PadCenter(timeSpentWidth);
            var uom = habitSummarized.Item3.ToString().PadCenter(uomWidth);

            for (int i = 0; i < habitLines.Count; i++)
            {
                string habit = habitLines[i].PadCenter(habitWidth);
                string time = i == 0 ? timeSpent : new string(' ', timeSpentWidth);
                string unit = i == 0 ? uom : new string(' ', uomWidth);
                Console.WriteLine($"│{habit}│{time}│{unit}│");
            }
            Console.WriteLine(divider);
        }
    }

}

public static class StringExtensions
{
    public static List<string> WrapText(this string text, int width)
    {
        var lines = new List<string>();
        while (text.Length > width)
        {
            int splitIndex = text.LastIndexOf(' ', width);
            if (splitIndex == -1) splitIndex = width;

            lines.Add(text.Substring(0, splitIndex).Trim());
            text = text.Substring(splitIndex).Trim();
        }
        lines.Add(text);
        return lines;
    }

    public static string PadCenter(this string text, int width)
    {
        if (text.Length >= width)
            return text;

        int padding = width - text.Length;
        int padLeft = padding / 2 + text.Length;
        return text.PadLeft(padLeft).PadRight(width);
    }
}

