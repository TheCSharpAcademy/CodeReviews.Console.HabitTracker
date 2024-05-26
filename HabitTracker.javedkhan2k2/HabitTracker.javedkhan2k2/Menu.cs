using System.Text.RegularExpressions;
using HabitTracker.Models;

namespace HabitTracker;

internal static class Menu
{
    internal static void ShowAllHabitLogs(List<HabitLog>? habitlogs)
    {
        Console.Clear();
        Console.WriteLine("List of Habitlogs");
        Console.WriteLine("-----------------------------------------------------------------------------");
        Console.WriteLine("{0,-3} | {1, -15} | {2, -5} | {3, -5} | {4,-30}", "ID", "Date", "Qty", "Unit", "Habit");
        Console.WriteLine("-----------------------------------------------------------------------------");
        foreach(var habitlog in habitlogs)
        {
            Console.WriteLine("{0,-3} | {1, -15} | {2, -5} | {3, -5} | {4,-30}", habitlog.Id, habitlog.Date.ToString("dd-MMM-yyyy"), habitlog.Quantity, habitlog.HabitUnit, habitlog.HabitDescription);
            Console.WriteLine("-----------------------------------------------------------------------------");
        }
    }

    internal static void ShowAllHabits(List<Habit>? habits)
    {
        Console.Clear();
        Console.WriteLine("List of Habits");
        Console.WriteLine("-----------------------------------------------------------------------------");
        Console.WriteLine("{0,-3} | {1, -15} | {2,-30}", "ID", "Unit", "Habit");
        Console.WriteLine("-----------------------------------------------------------------------------");
        foreach(var habit in habits)
        {
            Console.WriteLine("{0,-3} | {1, -15} | {2,-30}", habit.Id, habit.Unit, habit.HabitDescription);
            Console.WriteLine("-----------------------------------------------------------------------------");
        }
    }

    internal static int ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine(@$"
            Main Menu

            What would you like to do?

            Type 0 to Close Application
            Type 1 to View All Records
            Type 2 to Insert New Record 
            Type 3 to Delete Record 
            Type 4 to Update Record 
            Type 5 to View All Habits
            Type 6 to Insert New Habit
            Type 7 to View {DateTime.UtcNow.Year} Report
        ");
        Console.WriteLine("----------------------------------\nSelect an Option\n");
        var userInput = Helpers.GetUserInput();
        return userInput;
    }

    internal static void ShowReport(List<HabitReport> report)
    {
        Console.Clear();
        Console.WriteLine($"{DateTime.UtcNow.Year} Report");
        Console.WriteLine("-----------------------------------------------------------------------------");
        Console.WriteLine("{0,-15} | {1, -15} | {2, -15}", "Habit", "Unit", "Sum");
        Console.WriteLine("-----------------------------------------------------------------------------");
        foreach(var record in report)
        {
            Console.WriteLine("{0,-15} | {1, -15} | {2, -15}", record.HabitDescription, record.Unit, record.Sum);
            Console.WriteLine("-----------------------------------------------------------------------------");
        }
    }

}