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
        var drinkRepository = new DrinkRepository();

        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("Type a number to select an option:");
            Console.WriteLine("\t0. Close Application.");
            Console.WriteLine("\t1. View All Records.");
            Console.WriteLine("\t2. Insert Record.");
            Console.WriteLine("\t3. Delete Record.");
            Console.WriteLine("\t4. Update Record.");
            Console.WriteLine("------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    break;
                case "1":
                    foreach (var record in drinkRepository.GetAllRecords())
                    {
                        Console.WriteLine($"{record.Id}: {record.Date.ToString("dd-MM-yy")} - {record.Quantity} glasses");
                    }
                    break;
                case "2":
                    var newRecord = new DrinkingWater
                    {
                        Date = InputService.GetDateInput(),
                        Quantity = InputService.GetNumberInput("\nInsert the quantity (no decimals): ")
                    };
                    drinkRepository.InsertDrinkRecord(newRecord);
                    Console.WriteLine("Record inserted successfully.");
                    break;
                case "3":
                    Console.WriteLine("\nInsert the ID of the record to delete:");
                    int deleteId = InputService.GetNumberInput("Enter ID:");
                    drinkRepository.DeleteDrinkRecord(deleteId);
                    Console.WriteLine("Record deleted successfully.");
                    break;
                case "4":
                    Console.WriteLine("\nInsert the ID of the record to update:");
                    int updateId = InputService.GetNumberInput("Enter ID:");
                    var updatedRecord = new DrinkingWater
                    {
                        Id = updateId,
                        Date = InputService.GetDateInput(),
                        Quantity = InputService.GetNumberInput("Enter new quantity:")
                    };
                    drinkRepository.UpdateDrinkRecord(updatedRecord);
                    Console.WriteLine("Record updated successfully.");
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}
