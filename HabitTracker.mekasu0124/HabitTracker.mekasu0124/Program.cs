using HabitTracker.Models;

namespace HabitTracker;

public class Program
{
    private static readonly string dbFile = "habits.db";
    
    public static void Main(string[] args)
    {
        if (!File.Exists(dbFile))
        {
            Database.CreateDatabase();
            Console.Clear();
            MainMenu.ShowMenu();
        }
        else
        {
            MainMenu.ShowMenu();
        }
    }
}