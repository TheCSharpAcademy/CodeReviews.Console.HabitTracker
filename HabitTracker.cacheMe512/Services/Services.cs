using System;
using System.Globalization;
using habit_logger.Models;
using habit_logger.Repositories;

namespace habit_logger.Services
{
    public static class InputService
    {
        public static DateTime GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-MM-yy)");

            string dateInput = Console.ReadLine();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out var date))
            {
                Console.WriteLine("Invalid date format. Please try again:");
                dateInput = Console.ReadLine();
            }

            return DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"));
        }

        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            while (!int.TryParse(numberInput, out int number) || number < 0)
            {
                Console.WriteLine("Invalid number. Try again:");
                numberInput = Console.ReadLine();
            }

            return int.Parse(numberInput);
        }

        public static string GetStringInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static Habit SelectHabit(HabitRepository habitRepository)
        {
            List<Habit> habits = new List<Habit>();
            foreach (var habit in habitRepository.GetAllHabits())
            {
                habits.Add(habit);
            }

            if (habits.Count == 0)
            {
                Console.WriteLine("No habits available.");
                return null;
            }

            Console.WriteLine("Select a habit from the list below:");
            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {habits[i].Name} (Unit: {habits[i].Unit})");
            }
            int selectedIndex = InputService.GetNumberInput("\nEnter the number of the habit:") - 1;
            if (selectedIndex < 0 || selectedIndex >= habits.Count)
            {
                Console.WriteLine("Invalid selection.");
                return null;
            }
            return habits[selectedIndex];
        }
    }
}
