using System;

namespace HabitTracker
{
    public class HabitTrackerApp(string connectionString)
    {
        private readonly HabitRepository habitRepository = new(connectionString);

        public static void Run()
        {
            while (true)
            {
                ShowMenu();
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        //InsertHabit();
                        break;
                    case 2:
                        //ViewHabits();
                        break;
                    case 3:
                        //UpdateHabit();
                        break;
                    case 4:
                        //DeleteHabit();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("\nHabit Tracker Menu:");
            Console.WriteLine("1. Insert Habit");
            Console.WriteLine("2. View Habits");
            Console.WriteLine("3. Update Habit");
            Console.WriteLine("4. Delete Habit");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }
    }
}
