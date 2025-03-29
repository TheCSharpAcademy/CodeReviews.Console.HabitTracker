using HabitTracker.KamilKolanowski.Data;
using Microsoft.Data.Sqlite;

namespace HabitTracker.KamilKolanowski.Views;


// Present application Id to the user instead of Database Id 
// Instead of presenting too long habit in such form "..." wrap it to the new line
public class HabitsPresentator
{
    public void PresentHabits(DatabaseManager db, SqliteConnection connection)
    {
        var habits = db.ListHabits(connection);
        
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Habits List");
        Console.ResetColor();
        
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

    public void PresentReport(DatabaseManager db, SqliteConnection connection)
    {
        var report = db.CreateReport(connection);
        
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Habits Summary Report");
        Console.ResetColor();
        
        string line =   "┌─────────────┬────────────┬────────┐";
        string middle = "├─────────────┼────────────┼────────┤";
        string bottom = "└─────────────┴────────────┴────────┘";
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(line);
        Console.WriteLine($"│{"Habit".PadCenter(13)}│{"Time Spent".PadCenter(12)}│{"UoM".PadCenter(8)}│");
        Console.WriteLine(middle);
        
        Console.ResetColor();
        
        foreach (var habitSummarized in report)
        {
            Console.WriteLine($"│{habitSummarized.Item1.PadCenter(13)}│ {habitSummarized.Item2.ToString().PadCenter(10)} │{habitSummarized.Item3.ToString().PadCenter(8)}│");
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
            text = width > 3 ? text.Substring(0, width - 3) + "\n" : text.Substring(0, width); 
        }

        int padding = width - text.Length;
        int padLeft = padding / 2 + text.Length;
        return text.PadLeft(padLeft).PadRight(width);
    }
}
// public static class StringExtensions
// {
//     public static string PadCenter(this string text, int width)
//     {
//         if (text.Length > width)
//         {
//             text = text.Substring(0, width - 7) + "..."; // Adding "..." to indicate truncation of text that doesn't fit the table
//         }
//
//         int padding = width - text.Length;
//         int padLeft = padding / 2 + text.Length;
//         return text.PadLeft(padLeft).PadRight(width);
//     }
// }
