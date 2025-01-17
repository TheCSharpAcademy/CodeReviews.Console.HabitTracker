using System;
using habit_logger.Models;
using habit_logger.Data;
using habit_logger.Repositories;
using habit_logger.Services;

class Program
{
    static void Main(string[] args)
    {
        Database.InitializeDatabase();
        var habitRepository = new HabitRepository();
        var habitRecordRepository = new HabitRecordRepository();

        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("1. View All Habits");
            Console.WriteLine("2. Add New Habit");
            Console.WriteLine("3. Add Record for Habit");
            Console.WriteLine("4. View Habit Records");
            Console.WriteLine("0. Exit");
            string command = Console.ReadLine();

            switch (command)
            {
                case "1":
                    foreach (var habit in habitRepository.GetAllHabits())
                    {
                        Console.WriteLine($"{habit.Id}: {habit.Name} (Unit: {habit.Unit})");
                    }
                    break;
                case "2":
                    var newHabit = new Habit
                    {
                        Name = InputService.GetStringInput("Enter habit name:"),
                        Unit = InputService.GetStringInput("Enter unit of measurement:")
                    };
                    habitRepository.InsertHabit(newHabit);
                    Console.WriteLine("Habit added successfully.");
                    break;
                case "3":
                    Console.WriteLine("Enter the name of the habit:");
                    string habitName = InputService.GetStringInput("Habit Name:");

                    int? habitId = habitRepository.GetHabitIdByName(habitName);

                    if (habitId == null)
                    {
                        Console.WriteLine($"Habit '{habitName}' does not exist. Would you like to create it? (y/n)");
                        string response = Console.ReadLine()?.ToLower();

                        if (response == "y")
                        {
                            var habit = new Habit
                            {
                                Name = habitName,
                                Unit = InputService.GetStringInput("Enter unit of measurement for the habit:")
                            };
                            habitRepository.InsertHabit(habit);
                            Console.WriteLine($"Habit '{habitName}' added successfully.");
                            habitId = habitRepository.GetHabitIdByName(habitName);
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled. No record added.");
                            break;
                        }
                    }

                    var newRecord = new HabitRecord
                    {
                        HabitId = habitId.Value,
                        Date = InputService.GetDateInput(),
                        Quantity = InputService.GetNumberInput("Enter quantity:")
                    };
                    habitRecordRepository.InsertHabitRecord(newRecord);
                    Console.WriteLine("Record added successfully.");
                    break;

                case "4":
                    Console.WriteLine("Enter the name of the habit to view records:");
                    string viewHabitName = InputService.GetStringInput("Habit Name:");

                    int? viewHabitId = habitRepository.GetHabitIdByName(viewHabitName );
                    if(viewHabitId != null)
                        foreach (var record in habitRecordRepository.GetRecordsForHabit(viewHabitId.Value))
                        {
                            Console.WriteLine($"Date: {record.Date.ToString("dd-MM-yy")}  Quantity: {record.Quantity}");
                        }
                    else
                    {
                        Console.WriteLine($"Habit '{viewHabitName}' does not exist.");
                    }
                    break;
                case "0":
                    closeApp = true;
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }
}
