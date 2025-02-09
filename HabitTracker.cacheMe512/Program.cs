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

        while (true)
        {
            Console.Clear();
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("1. View All Habits");
            Console.WriteLine("2. Add New Habit");
            Console.WriteLine("3. Add Record for Habit");
            Console.WriteLine("4. View Habit Records");
            Console.WriteLine("5. Update Habit Records");
            Console.WriteLine("6. Delete Habit Record");
            Console.WriteLine("0. Exit");
            string command = Console.ReadLine();

            switch (command)
            {
                case "1":
                    ViewAllHabits(habitRepository);
                    break;
                case "2":
                    AddNewHabit(habitRepository);
                    break;
                case "3":
                    AddRecordForHabit(habitRepository, habitRecordRepository);
                    break;
                case "4":
                    ViewHabitRecords(habitRepository, habitRecordRepository);
                    break;
                case "5":
                    UpdateHabitRecords(habitRepository, habitRecordRepository);
                    break;
                case "6":
                    DeleteHabitRecord(habitRepository, habitRecordRepository);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
            Console.Write("\nPress any key to continue...");
            Console.ReadLine();
        }
    }

    private static void ViewAllHabits(HabitRepository habitRepository)
    {
        Console.Clear();

        foreach (var habit in habitRepository.GetAllHabits())
        {
            Console.WriteLine($"{habit.Id}: {habit.Name} (Unit: {habit.Unit})");
        }
    }

    private static void AddNewHabit(HabitRepository habitRepository)
    {
        Console.Clear();

        var newHabit = new Habit
        {
            Name = InputService.GetStringInput("Enter habit name:"),
            Unit = InputService.GetStringInput("Enter unit of measurement:")
        };
        habitRepository.InsertHabit(newHabit);
    }

    private static void AddRecordForHabit(HabitRepository habitRepository, HabitRecordRepository habitRecordRepository)
    {
        Console.Clear();

        Habit habit = InputService.SelectHabit(habitRepository);
        if (habit == null)
        {
            Console.WriteLine("No habit selected. Would you like to add a new habit? (y/n)");
            string response = Console.ReadLine()?.ToLower();
            if (response == "y")
            {
                AddNewHabit(habitRepository);
                habit = InputService.SelectHabit(habitRepository);
                if (habit == null)
                {
                    Console.WriteLine("No habit available.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled. No record added.");
                return;
            }
        }

        var newRecord = new HabitRecord
        {
            HabitId = habit.Id,
            Date = InputService.GetDateInput(),
            Quantity = InputService.GetNumberInput("Enter quantity:")
        };
        habitRecordRepository.InsertHabitRecord(newRecord);
        Console.WriteLine("Record added successfully.");
    }

    private static void ViewHabitRecords(HabitRepository habitRepository, HabitRecordRepository habitRecordRepository)
    {
        Console.Clear();

        Habit habit = InputService.SelectHabit(habitRepository);
        if (habit == null)
        {
            Console.WriteLine("No habit selected.");
            return;
        }
        int habitId = habit.Id;
        var records = habitRecordRepository.GetRecordsForHabit(habitId);
        Console.WriteLine($"\nRecords for habit: {habit.Name}");
        bool hasRecords = false;
        foreach (var record in records)
        {
            hasRecords = true;
            Console.WriteLine($"ID: {record.Id}, Date: {record.Date.ToString("dd-MM-yy")}, Quantity: {record.Quantity}");
        }
        if (!hasRecords)
        {
            Console.WriteLine("No records found for this habit.");
        }
    }

    private static void UpdateHabitRecords(HabitRepository habitRepository, HabitRecordRepository habitRecordRepository)
    {
        Console.Clear();

        Habit habit = InputService.SelectHabit(habitRepository);
        if (habit == null)
        {
            Console.WriteLine("No habit selected.");
            return;
        }
        int habitId = habit.Id;
        var records = habitRecordRepository.GetRecordsForHabit(habitId);

        bool hasRecords = false;
        foreach (var rec in records)
        {
            hasRecords = true;
            break;
        }
        if (!hasRecords)
        {
            Console.WriteLine("No records found for this habit.");
            return;
        }

        Console.WriteLine($"\nRecords for habit: {habit.Name}");
        foreach (var record in records)
        {
            Console.WriteLine($"ID: {record.Id}, Date: {record.Date.ToString("dd-MM-yy")}, Quantity: {record.Quantity}");
        }

        Console.WriteLine("\nEnter the ID of the record you want to update:");
        int recordId = InputService.GetNumberInput("Record ID:");

        HabitRecord recordToUpdate = null;
        foreach (var record in records)
        {
            if (record.Id == recordId)
            {
                recordToUpdate = record;
                break;
            }
        }
        if (recordToUpdate == null)
        {
            Console.WriteLine("Record not found.");
            return;
        }

        Console.WriteLine("\nEnter the new details for the record:");
        DateTime newDate = InputService.GetDateInput();
        int newQuantity = InputService.GetNumberInput("Enter new quantity:");

        recordToUpdate.Date = newDate;
        recordToUpdate.Quantity = newQuantity;

        try
        {
            habitRecordRepository.UpdateHabitRecord(recordToUpdate);
            Console.WriteLine("Record updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating record: " + ex.Message);
        }
    }

    private static void DeleteHabitRecord(HabitRepository habitRepository, HabitRecordRepository habitRecordRepository)
    {
        Console.Clear();

        Habit habit = InputService.SelectHabit(habitRepository);
        if (habit == null)
        {
            Console.WriteLine("No habit selected.");
            return;
        }
        int habitId = habit.Id;
        var recordsToDelete = habitRecordRepository.GetRecordsForHabit(habitId);

        if (!recordsToDelete.Any())
        {
            Console.WriteLine("No records found for this habit.");
            return;
        }

        Console.WriteLine($"\nRecords for habit: {habit.Name}");
        foreach (var record in recordsToDelete)
        {
            Console.WriteLine($"ID: {record.Id}, Date: {record.Date:dd-MM-yy}, Quantity: {record.Quantity}");
        }

        int recordId;
        while (true)
        {
            recordId = InputService.GetNumberInput("Enter the ID of the record you want to delete:");
            if (recordsToDelete.Any(record => record.Id == recordId))
            {
                break;
            }
            else
            {
                Console.WriteLine("Record not found for the selected habit. Please enter a valid record ID.");
            }
        }

        try
        {
            habitRecordRepository.DeleteHabitRecord(recordId);
            Console.WriteLine("Habit record deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error deleting habit record: " + ex.Message);
        }
    }

}