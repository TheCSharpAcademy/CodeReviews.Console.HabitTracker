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
            Console.WriteLine("Enter 0 To close App");
            Console.WriteLine("Enter 1 to view all records");
            Console.WriteLine("Enter 2 to Insert new record");
            Console.WriteLine("Enter 3 to delete record");
            Console.WriteLine("Enter 4 to update record");
            Console.WriteLine("Enter 5 to create tracked habit");
            Console.WriteLine("Enter 6 to select tracked habit");
            Console.WriteLine("Enter 7 to view all habits");
            Console.WriteLine("------------------------------------------------\n");
        }
    }
}
