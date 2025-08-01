using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using HabitLoggerApp.Helpers;
using HabitLoggerApp.Models;
using HabitLoggerApp.Services;
using Spectre.Console;

namespace HabitLoggerApp.App
{
    public class HabitTrackerApp(HabitService habitService, HabitEntryService habitEntryService, ReportService reportService)
    {
        private readonly HabitService _habitService = habitService;
        private readonly HabitEntryService _habitEntryService = habitEntryService;
        private readonly ReportService _reportService = reportService;

        public void Run()
        {
            ShowSeedResult();

            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]What do you want to do?[/]")
                        .PageSize(10)
                        .AddChoices([
                            "Add Habit", "View Habits", "Delete Habit",
                            "Log Habit Entry", "View Habit Entries",
                            "Edit Habit Entries", "Delete Habit Entries",
                            "View Habit Report", "Exit"
                        ])
                );

                switch (choice)
                {
                    case "Add Habit":
                        HandleAddHabit();
                        break;
                    case "View Habits":
                        HandleViewHabits();
                        break;
                    case "Delete Habit":
                        HandleDeleteHabit();
                        break;
                    case "Log Habit Entry":
                        HandleLogHabitEntry();
                        break;
                    case "View Habit Entries":
                        HandleViewHabitEntries();
                        break;
                    case "Edit Habit Entries":
                        HandleEditHabitEntries();
                        break;
                    case "Delete Habit Entries":
                        HandleDeleteHabitEntries();
                        break;
                    case "View Habit Report":
                        HandleViewHabitReport();
                        break;
                    case "Exit":
                        return;
                }

                AnsiConsole.MarkupLine("[grey]\nPress any key to continue...[/]");
                Console.ReadKey();
            }
        }

        private void HandleViewHabits()
        {
            var habits = _habitService.GetAllHabits();

            if (habits.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No habits found.[/]");
                return;
            }

            AnsiConsole.MarkupLine("[green]Your Habits:[/]");
            foreach (var habit in habits)
            {
                AnsiConsole.MarkupLine($"• [yellow]ID: {habit.Id}[/] | {habit.Name} ({habit.UnitOfMeasure})");
            }
        }

        private void HandleAddHabit()
        {
            string name = AnsiConsole.Ask<string>("Enter habit name:");
            string unit = AnsiConsole.Ask<string>("Enter unit of measure:");

            var (_, message) = _habitService.AddHabit(name, unit);
            AnsiConsole.MarkupLine($"[blue]{message}[/]");
        }

        private void ShowSeedResult()
        {
            bool seeded = _habitService.SeedInitialData();

            AnsiConsole.MarkupLine(seeded
                ? "[green]Database seeded with test data.[/]"
                : "[grey]Seed skipped: database already contains habits.[/]");

            AnsiConsole.MarkupLine("[grey]\nPress any key to continue...[/]");
            Console.ReadKey();
            Console.Clear();
        }

        private void HandleDeleteHabit()
        {
            var habits = _habitService.GetAllHabits();
            if (habits.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No habits found.[/]");
                return;
            }

            var selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                    .Title("Select a habit to delete")
                    .UseConverter(h => $"ID: {h.Id} | {h.Name} ({h.UnitOfMeasure})")
                    .AddChoices(habits)
            );

            if (!AnsiConsole.Confirm($"Are you sure you want to delete habit '{selectedHabit.Name}'?"))
                return;

            bool deleted = _habitService.DeleteHabit(selectedHabit.Id);
            AnsiConsole.MarkupLine(deleted
                ? $"[green]Habit '{selectedHabit.Name}' deleted successfully.[/]"
                : $"[red]Failed to delete habit '{selectedHabit.Name}'.[/]");
        }

        private void HandleLogHabitEntry()
        {
            var habits = _habitService.GetAllHabits();
            if (habits.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No habits available. Add a habit first.[/]");
                return;
            }

            var selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                    .Title("Select a habit to log entry for")
                    .UseConverter(h => $"ID: {h.Id} | {h.Name} ({h.UnitOfMeasure})")
                    .AddChoices(habits)
            );

            int quantity = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter quantity:")
                    .Validate(q => q > 0 ? ValidationResult.Success() : ValidationResult.Error("Quantity must be greater than 0."))
            );

            DateTime date = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter date (yyyy-MM-dd):")
                    .DefaultValue(DateTime.Today)
                    .Validate(d => d <= DateTime.Today ? ValidationResult.Success() : ValidationResult.Error("Date cannot be in the future."))
            );

            if (_habitEntryService.LogEntry(selectedHabit.Id, quantity, date))
                AnsiConsole.MarkupLine("[green]Entry logged successfully.[/]");
            else
                AnsiConsole.MarkupLine("[red]Failed to log entry. Make sure the habit exists.[/]");
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
                AnsiConsole.MarkupLine("[red]No habits available.[/]");
                return;
            }

            var selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                    .Title("Select a habit to view report")
                    .UseConverter(h => $"ID: {h.Id} | {h.Name} ({h.UnitOfMeasure})")
                    .AddChoices(habits)
            );

            int year = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter the year to view report (e.g., 2024):")
                    .Validate(y => y > 1900 && y <= DateTime.Now.Year
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Please enter a valid year."))
            );

            var report = _reportService.GenerateHabitReport(selectedHabit.Id, year);
            if (report == null)
            {
                AnsiConsole.MarkupLine("[red]Habit not found or no data.[/]");
                return;
            }

            AnsiConsole.MarkupLine($"\n[bold]Report for '{report.HabitName}' in {report.Year}[/]");
            AnsiConsole.MarkupLine($"- Entries Logged: [yellow]{report.EntryCount}[/]");
            AnsiConsole.MarkupLine($"- Total Quantity: [yellow]{report.TotalQuantity}[/]");
            AnsiConsole.MarkupLine($"- Average per Entry: [yellow]{report.AverageQuantityPerEntry:F2}[/]");
        }


        private void HandleDeleteHabitEntries()
        {
            var habits = _habitService.GetAllHabits();
            if (habits.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No habits found.[/]");
                return;
            }

            var selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                    .Title("Select a habit to delete entries from")
                    .UseConverter(h => $"ID: {h.Id} | {h.Name} ({h.UnitOfMeasure})")
                    .AddChoices(habits)
            );

            var entries = _habitEntryService.GetEntriesForHabit(selectedHabit.Id);
            if (entries.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]No entries found.[/]");
                return;
            }

            var selectedEntry = AnsiConsole.Prompt(
                new SelectionPrompt<HabitEntry>()
                    .Title("Select an entry to delete")
                    .UseConverter(e => $"{e.Date:yyyy-MM-dd} — {e.Quantity}")
                    .AddChoices(entries)
            );

            if (!AnsiConsole.Confirm("Are you sure you want to delete this entry?"))
                return;

            bool success = _habitEntryService.DeleteEntry(selectedEntry.Id);
            AnsiConsole.MarkupLine(success
                ? "[green]Entry deleted successfully.[/]"
                : "[red]Failed to delete entry.[/]");
        }


        private void HandleEditHabitEntries()
        {
            var habits = _habitService.GetAllHabits();
            if (habits.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No habits found.[/]");
                return;
            }

            var selectedHabit = AnsiConsole.Prompt(
                new SelectionPrompt<Habit>()
                    .Title("Select a habit to edit entries for")
                    .UseConverter(h => $"ID: {h.Id} | {h.Name} ({h.UnitOfMeasure})")
                    .AddChoices(habits)
            );

            var entries = _habitEntryService.GetEntriesForHabit(selectedHabit.Id);
            if (entries.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]No entries to edit.[/]");
                return;
            }

            var selectedEntry = AnsiConsole.Prompt(
                new SelectionPrompt<HabitEntry>()
                    .Title("Select an entry to edit")
                    .UseConverter(e => $"{e.Date:yyyy-MM-dd} — {e.Quantity}")
                    .AddChoices(entries)
            );

            int newQuantity = AnsiConsole.Prompt(
                new TextPrompt<int>($"Enter new quantity (current: {selectedEntry.Quantity}):")
                    .AllowEmpty()
                    .DefaultValue(selectedEntry.Quantity)
                    .Validate(q => q > 0 ? ValidationResult.Success() : ValidationResult.Error("Quantity must be greater than 0."))
            );

            DateTime newDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>($"Enter new date (current: {selectedEntry.Date:yyyy-MM-dd}) or leave blank:")
                    .AllowEmpty()
                    .DefaultValue(selectedEntry.Date)
                    .Validate(d => d <= DateTime.Today ? ValidationResult.Success() : ValidationResult.Error("Date cannot be in the future."))
            );

            selectedEntry.Quantity = newQuantity;
            selectedEntry.Date = newDate;

            bool success = _habitEntryService.UpdateEntry(selectedEntry);
            AnsiConsole.MarkupLine(success
                ? "[green]Entry updated successfully.[/]"
                : "[red]Failed to update entry.[/]");
        }
    }
}

