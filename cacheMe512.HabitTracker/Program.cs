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

        Console.WriteLine("Enter the name of the habit:");
        string habitName = InputService.GetStringInput("Habit Name:");
        Console.WriteLine();
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
                habitId = habitRepository.GetHabitIdByName(habitName);
            }
            else
            {
                Console.WriteLine("Operation cancelled. No record added.");
                return;
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
    }

    private static void ViewHabitRecords(HabitRepository habitRepository, HabitRecordRepository habitRecordRepository)
    {
        Console.Clear();

        Console.WriteLine("Enter the name of the habit to view records:");
        string viewHabitName = InputService.GetStringInput("Habit Name:");
        Console.WriteLine();
        int? viewHabitId = habitRepository.GetHabitIdByName(viewHabitName);
        if (viewHabitId != null)
        {
            foreach (var record in habitRecordRepository.GetRecordsForHabit(viewHabitId.Value))
            {
                Console.WriteLine($"Date: {record.Date.ToString("dd-MM-yy")}  Quantity: {record.Quantity}");
            }
        }
        else
        {
            Console.WriteLine($"Habit '{viewHabitName}' does not exist.");
        }
    }

    private static void UpdateHabitRecords(HabitRepository habitRepository, HabitRecordRepository habitRecordRepository)
    {
        Console.Clear();

        Console.WriteLine("Enter the name of the habit to update a record:");
        string habitName = InputService.GetStringInput("Habit Name:");

        int? habitId = habitRepository.GetHabitIdByName(habitName);
        if (habitId == null)
        {
            Console.WriteLine($"Habit '{habitName}' does not exist.");
            return;
        }

        var records = habitRecordRepository.GetRecordsForHabit(habitId.Value);

        if (!records.Any())
        {
            Console.WriteLine("\nNo records found for this habit.");
            return;
        }

        Console.WriteLine("\nRecords for the habit:");
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

        Console.WriteLine("Enter the name of the habit to delete a record from:");
        string deleteHabitName = InputService.GetStringInput("Habit Name:");
        Console.WriteLine();
        int? deleteHabitId = habitRepository.GetHabitIdByName(deleteHabitName);
        if (deleteHabitId == null)
        {
            Console.WriteLine($"Habit '{deleteHabitName}' does not exist.");
            return;
        }

        var recordsToDelete = habitRecordRepository.GetRecordsForHabit(deleteHabitId.Value);
        if (!recordsToDelete.Any())
        {
            Console.WriteLine("\nNo records found for this habit.");
            return;
        }

        Console.WriteLine("\nRecords for the habit:");
        foreach (var record in recordsToDelete)
        {
            Console.WriteLine($"ID: {record.Id}, Date: {record.Date.ToString("dd-MM-yy")}, Quantity: {record.Quantity}");
        }

        Console.WriteLine("\nEnter the ID of the record you want to delete:");
        int recordId = InputService.GetNumberInput("Record ID:");

        try
        {
            habitRecordRepository.DeleteHabitRecord(recordId);
            Console.WriteLine("\nHabit record deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError deleting habit record: {ex.Message}");
        }
    }
}