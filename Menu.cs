using System.Transactions;

namespace habit_tracker
{
    public static class Menu
    {
        public static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Habit Tracker App!");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Current selected habit: " + Repository.ShowCurrentHabbit());
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("\nSelect option");
            Console.WriteLine("Enter 0 To close App\n");
            Console.WriteLine("Enter 1 to view all records");
            Console.WriteLine("Enter 2 to insert new record");
            Console.WriteLine("Enter 3 to delete record");
            Console.WriteLine("Enter 4 to update record");
            Console.WriteLine("Enter 5 to create habit");
            Console.WriteLine("Enter 6 to select habit");
            Console.WriteLine("Enter 7 to view all habits");
            Console.WriteLine("Enter 8 to generate random records");
            Console.WriteLine("------------------------------------------------\n");
        }
    }
}
