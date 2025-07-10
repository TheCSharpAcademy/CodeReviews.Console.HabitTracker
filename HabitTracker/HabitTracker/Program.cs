using HabitTrackerLibrary;

namespace HabitTracker
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Preparing databases, please wait...");
            SqlHelper.CreateDatabaseIfNotExists();
            Console.Clear();

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Welcome to your Habit Tracker.");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("\nPress enter to continue to the Main Menu");
            Console.ReadKey();

            Menu.StartMainMenuLoop();
        }
    }
}


