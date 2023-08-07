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
                    ViewHabit();
                    break;
                case "h":
                    AddHabit();
                    break;
                case "r":
                    AddHabitRecord();
                    break;
                case "0":
                    Console.WriteLine("Program ended");
                    return;
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
            v - View habit statistics
            h - Add/ replace habit
            r - Add habit record
            0 - Exit program

            """);
    }

    private void ViewHabit()
    {
        try
        {
            var habit = _service.Get();

            Console.Clear();
            Console.WriteLine($"{habit.Name} statistics");
            Console.WriteLine("| Date                | Count |");

            foreach (var date in habit.Dates)
            {
                Console.WriteLine($"{date.Date.ToString().PadLeft(10).PadRight(24)}{date.Count}");
            }
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("No habit currently being recorded, add habit first");
        }
        Thread.Sleep(3000);
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
                Console.WriteLine($"{name} added as habit");
                Thread.Sleep(2000);
                break;
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid entry please try again");
            }
            catch (InvalidOperationException)
            {
                Console.Write($"Another habit is already stored in the system, do you want to replace it with {name} (Enter y/n)? ");
                var op = Console.ReadLine();

                if (op == "y") ReplaceHabit(name);

                break;
            }
        }
    }

    private void ReplaceHabit(string? name)
    {
        _service.Delete();
        _service.Add(name);
        Console.WriteLine($"You have changed your habit to {name}");
        Thread.Sleep(2000);
    }

    private void AddHabitRecord()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("Which date would you like to add a record to? ");
            var dateString = Console.ReadLine();
            Console.Write("How many times did you repeat the habit that day? ");
            var repetitionsString = Console.ReadLine();

            try
            {
                var date = DateOnly.Parse(dateString);
                var repetitions = Int32.Parse(repetitionsString);

                _service.AddRecord(date, repetitions);
                Console.WriteLine("Entry added!");
                Thread.Sleep(3000);
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid date or repetitions entry, please try again");
                Thread.Sleep(3000);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No habit currently being recorded, add habit first");
                Thread.Sleep(3000);
                break;
            }
        }
    }
}