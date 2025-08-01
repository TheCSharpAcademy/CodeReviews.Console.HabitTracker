using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using HabitLoggerApp.Services;
using HabitLoggerApp.Utils;

namespace HabitLoggerApp.App
{
    public class HabitTrackerApp(HabitService habitService, HabitEntryService habitEntryService, ReportService reportService)
    {
        private readonly HabitService _habitService = habitService;
        private readonly HabitEntryService _habitEntryService=habitEntryService;
        private readonly ReportService _reportService = reportService;


        public void Run()
        {

            ShowSeedResult();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Habit Tracker App");
                Console.WriteLine("------------------");
                Console.WriteLine("1. Add Habit");
                Console.WriteLine("2. View Habits");
                Console.WriteLine("3. Delete Habit");
                Console.WriteLine("4. Log Habit Entry");
                Console.WriteLine("5. View Habit Entries");
                Console.WriteLine("6. View Habit Report (yearly)");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(choice))
                {
                    Console.WriteLine("Invalid choice, try again.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case "1":
                        HandleAddHabit();
                        break;
                    case "2":
                        HandleViewHabits();
                        break;
                    case "3":
                        HandleDeleteHabit();
                        break;
                    case "4": 
                        HandleLogHabitEntry();
                        break;
                    case "5":
                        HandleViewHabitEntries();
                        break;
                    case "6":
                        HandleViewHabitReport();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void HandleViewHabits()
        {
            var habits = _habitService.GetAllHabits();

            if (habits.Count == 0)
            {
                Console.WriteLine("No habits found.");
                return;
            }

            Console.WriteLine("Your Habits:");
            foreach (var habit in habits)
            {
                Console.WriteLine($"• ID: {habit.Id} | {habit.Name} ({habit.UnitOfMeasure})");
            }
        }

        private void HandleAddHabit()
        {
            string name = Helpers.GetNonEmptyInput("Enter habit name");
            string unit = Helpers.GetNonEmptyInput("Enter unit of measure");

            var (_, message) = _habitService.AddHabit(name, unit);
            Console.WriteLine(message);
        }

        private void ShowSeedResult()
        {
            bool seeded = _habitService.SeedInitialData();

            if (seeded)
                Console.WriteLine("Database seeded with test data.");
            else
                Console.WriteLine("Seed skipped: database already contains habits.");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void HandleDeleteHabit()
        {
            Console.Write("Enter the ID of the habit to delete: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int habitId) || habitId <= 0)
            {
                Console.WriteLine("Invalid ID. Please try again.");
                return;
            }

            Console.Write($"Are you sure you want to delete habit {habitId}? (y/n): ");
            string? confirm = Console.ReadLine();
            if (confirm?.ToLower() != "y")
            {
                Console.WriteLine("Deletion canceled.");
                return;
            }

            bool deleted = _habitService.DeleteHabit(habitId);
            if (!deleted)
            {
                Console.WriteLine($"No habit found with ID {habitId}.");
                return;
            }

            Console.WriteLine($"Habit with ID {habitId} deleted successfully.");
        }

        private void HandleLogHabitEntry()
        {
            var habits = _habitService.GetAllHabits();
            if (habits.Count == 0)
            {
                Console.WriteLine("No habits available. Add a habit first.");
                return;
            }

            Console.WriteLine("Available Habits:");
            foreach (var habit in habits)
            {
                Console.WriteLine($"• ID: {habit.Id} | {habit.Name} ({habit.UnitOfMeasure})");
            }

            Console.Write("Enter the ID of the habit to log: ");
            string? idInput = Console.ReadLine();
            if (!int.TryParse(idInput, out int habitId))
            {
                Console.WriteLine("Invalid habit ID.");
                return;
            }

            Console.Write("Enter quantity: ");
            string? quantityInput = Console.ReadLine();
            if (!int.TryParse(quantityInput, out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }

            Console.Write("Enter date (yyyy-MM-dd) or press Enter for today: ");
            string? dateInput = Console.ReadLine();

            DateTime date;
            if (string.IsNullOrWhiteSpace(dateInput) || string.Equals(dateInput, "today", StringComparison.OrdinalIgnoreCase))
            {
                date = DateTime.Today;
            }
            else if (!DateTime.TryParse(dateInput, out date))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            if (_habitEntryService.LogEntry(habitId, quantity, date))
            {
                Console.WriteLine("Entry logged successfully.");
            }
            else
            {
                Console.WriteLine("Failed to log entry. Make sure the habit ID exists.");
            }
        }

        private void HandleViewHabitEntries()
        {
            var habits = _habitService.GetAllHabits();
            if (habits.Count == 0)
            {
                Console.WriteLine("No habits available.");
                return;
            }

            Console.WriteLine("Available Habits:");
            foreach (var habit in habits)
            {
                Console.WriteLine($"• ID: {habit.Id} | {habit.Name} ({habit.UnitOfMeasure})");
            }

            Console.Write("Enter the ID of the habit to view entries: ");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int habitId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var entries = _habitEntryService.GetEntriesForHabit(habitId);
            if (entries.Count == 0)
            {
                Console.WriteLine("No entries found for this habit.");
                return;
            }

            Console.WriteLine("\nEntries for selected habit:");
            foreach (var entry in entries)
            {
                Console.WriteLine($"• {entry.Date:yyyy-MM-dd} — {entry.Quantity}");
            }
        }

        private void HandleViewHabitReport()
        {
            var habits = _habitService.GetAllHabits();
            if (habits.Count == 0)
            {
                Console.WriteLine("No habits available.");
                return;
            }

            Console.WriteLine("Available Habits:");
            foreach (var habit in habits)
            {
                Console.WriteLine($"• ID: {habit.Id} | {habit.Name} ({habit.UnitOfMeasure})");
            }

            Console.Write("Enter the ID of the habit: ");
            string? habitIdInput = Console.ReadLine();
            if (!int.TryParse(habitIdInput, out int habitId))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("Enter the year to view report (e.g., 2024): ");
            string? yearInput = Console.ReadLine();
            if (!int.TryParse(yearInput, out int year))
            {
                Console.WriteLine("Invalid year.");
                return;
            }

            var report = _reportService.GenerateHabitReport(habitId, year);
            if (report == null)
            {
                Console.WriteLine("Habit not found or no data.");
                return;
            }

            Console.WriteLine($"\n Report for '{report.HabitName}' in {report.Year}");
            Console.WriteLine($"- Entries Logged: {report.EntryCount}");
            Console.WriteLine($"- Total Quantity: {report.TotalQuantity}");
            Console.WriteLine($"- Average per Entry: {report.AverageQuantityPerEntry:F2}");
        }


    }
}

