using HabitTracker.MartinL_no.Models;

namespace HabitTracker.MartinL_no;

internal class HabitTrackerApplication
{
    private readonly HabitService _service;

    internal HabitTrackerApplication(HabitService service)
    {
        _service = service;
    }

    internal void Execute()
    {
        while (true)
        {
            ShowMenuOptions();

            Console.Write("Your choice: ");
            var op = Console.ReadLine();

            switch (op)
            {
                case "v":
                    ViewAllHabits();
                    break;
                case "a":
                    AddHabit();
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again");
                    Thread.Sleep(3000);
                    break;
            }
        }
    }

    private void ShowMenuOptions()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Habit Tracker app!");
        Console.WriteLine("---------------------------------\n");

        Console.WriteLine("""
            Select an option:
            v - View all habits
            a - Add habit
            0 - Exit program

            """);
    }

    private void ViewAllHabits()
    {
        var habits = _service
            .GetAll()
            .OrderBy(h => h.Name)
            .ThenBy(h => h.Dates);

        Console.Clear();
        Console.WriteLine("| Habit           | Date                | Count |");
        foreach (var habit in habits)
        {
            PrintHabitTotal(habit);

            foreach (var date in habit.Dates)
            {
                Console.WriteLine($"{date.Date.ToString().PadLeft(28)}{date.Count.ToString().PadLeft(15)}");

            }
        }
        Thread.Sleep(3000);
    }

    private void PrintHabitTotal(Habit habit)
    {
        if (habit.Dates.Count == 0)
        {
            Console.WriteLine($"  {habit.Name.PadRight(17)} No dates registered");
        }
        else
        {
            Console.WriteLine($"  {habit.Name.PadRight(17)} All dates {habit.Dates.Sum(d => d.Count).ToString().PadLeft(13)}");
        }
    }

    private void AddHabit()
    {
        Console.Clear();

        while (true)
        {
            Console.Write("Enter your habit name: ");
            var name = Console.ReadLine();

            try
            {
                _service.Add(name);
                Console.WriteLine($"{name} added as habit\n");
                Thread.Sleep(3000);
                break;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid entry please try again");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Habit already entered");
                Thread.Sleep(3000);
                break;
            }
        }
    }

    
}