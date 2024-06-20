using System;

namespace HabitTracker
{
    public class HabitTrackerApp(string connectionString)
    {
        private HabitRepository habitRepository = new(connectionString);

        public void Run()
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
                        InsertHabit();
                        break;
                    case 2:
                        ViewHabits();
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

        private void InsertHabit()
        {
            try
            {
                Console.WriteLine("Enter habit name:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter quantity:");
                if (!int.TryParse(Console.ReadLine(), out int quantity))
                {
                    Console.WriteLine("Invalid quantity. Please enter a number.");
                    return;
                }

                Habit newHabit = new()
                {
                    Name = name,
                    Quantity = quantity,
                    Date = DateTime.Now
                };

                habitRepository.InsertHabit(newHabit);
                Console.WriteLine("Habit logged successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting habit: {ex.Message}");
            }
        }

        private void ViewHabits()
        {
            List<Habit> habits = habitRepository.GetAllHabits();

            if (habits.Count == 0)
            {
                Console.WriteLine("No habits logged yet.");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine("Logged Habits:");
            foreach (var habit in habits)
            {
                Console.WriteLine($"- {habit.Date:yyyy-MM-dd}: {habit.Name} ({habit.Quantity})");
            }
            
            Console.ReadLine();
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
