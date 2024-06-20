using System;
using System.Collections.Generic;

public class HabitTracker
{
    private readonly DatabaseManager _databaseManager;

    public HabitTracker()
    {
        _databaseManager = new DatabaseManager();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Habit Tracker - Drinking Water");
            Console.WriteLine("1. Insert Habit");
            Console.WriteLine("2. View Habits");
            Console.WriteLine("3. Update Habit");
            Console.WriteLine("4. Delete Habit");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        InsertHabit();
                        break;
                    case 2:
                        ViewHabits();
                        break;
                    case 3:
                        UpdateHabit();
                        break;
                    case 4:
                        DeleteHabit();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private void InsertHabit()
    {
        Console.Clear();
        Console.Write("Enter quantity of water glasses: ");
        if (int.TryParse(Console.ReadLine(), out int quantity))
        {
            var habit = new Habit
            {
                Quantity = quantity,
                Date = DateTime.Now
            };
            _databaseManager.InsertHabit(habit);
            Console.WriteLine("Habit inserted successfully. Press any key to continue...");
        }
        else
        {
            Console.WriteLine("Invalid quantity. Press any key to continue...");
        }
        Console.ReadKey();
    }

    private void ViewHabits()
    {
        Console.Clear();
        var habits = _databaseManager.GetHabits();
        foreach (var habit in habits)
        {
            Console.WriteLine($"ID: {habit.Id}, Quantity: {habit.Quantity}, Date: {habit.Date.ToShortDateString()}");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private void UpdateHabit()
    {
        Console.Clear();
        Console.Write("Enter habit ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Enter new quantity of water glasses: ");
            if (int.TryParse(Console.ReadLine(), out int quantity))
            {
                var habit = new Habit
                {
                    Id = id,
                    Quantity = quantity,
                    Date = DateTime.Now
                };
                _databaseManager.UpdateHabit(habit);
                Console.WriteLine("Habit updated successfully. Press any key to continue...");
            }
            else
            {
                Console.WriteLine("Invalid quantity. Press any key to continue...");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
        }
        Console.ReadKey();
    }

    private void DeleteHabit()
    {
        Console.Clear();
        Console.Write("Enter habit ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _databaseManager.DeleteHabit(id);
            Console.WriteLine("Habit deleted successfully. Press any key to continue...");
        }
        else
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
        }
        Console.ReadKey();
    }
}
