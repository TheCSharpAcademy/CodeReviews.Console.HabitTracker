using HabitTracker.KamilKolanowski.Data;
using Microsoft.Data.Sqlite;

namespace HabitTracker.KamilKolanowski.Views;

public class HabitsPresentator
{
    public void PresentHabits(DatabaseManager db, SqliteConnection connection)
    {
        var habits = db.ListHabits(connection);

        Console.Clear();
        Console.WriteLine(" Habits List ");

        string line =   "┌───────┬────────────────────┬────────────┬────────────┬──────────────────────────┐";
        string middle = "├───────┼────────────────────┼────────────┼────────────┼──────────────────────────┤";
        string bottom = "└───────┴────────────────────┴────────────┴────────────┴──────────────────────────┘";
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(line);
        Console.WriteLine($"│{"Id".PadCenter(7)}│{"Habit".PadCenter(20)}│{"Quantity".PadCenter(12)}│{"Unit".PadCenter(12)}│{"Insert Date".PadCenter(26)}│");
        Console.WriteLine(middle);
        
        Console.ResetColor();

        foreach (var habit in habits)
        {
            Console.WriteLine($"│{habit.Item1.ToString().PadCenter(7)}│{habit.Item2.PadCenter(20)}│{habit.Item3.ToString().PadCenter(12)}│{habit.Item4.PadCenter(12)}│{habit.Item5.PadCenter(26)}│");
        }
        Console.WriteLine(bottom);
    }
}

public static class StringExtensions
{
    public static string PadCenter(this string text, int width)
    {
        if (text.Length > width)
        {
            text = text.Substring(0, width - 7) + "..."; // Adding "..." to indicate truncation of text that doesn't fit the table
        }

        int padding = width - text.Length;
        int padLeft = padding / 2 + text.Length;
        return text.PadLeft(padLeft).PadRight(width);
    }
}


