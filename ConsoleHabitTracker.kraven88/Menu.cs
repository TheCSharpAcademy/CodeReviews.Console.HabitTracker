using ConsoleHabitTracker.kraven88.DataAccess;
using ConsoleHabitTracker.kraven88.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHabitTracker.kraven88;
internal class Menu
{
    private SqliteDB db;
    private string nl = Environment.NewLine;
    public Habit habit;

    public Menu(SqliteDB db)
    {
        this.db = db;
        habit = db.LoadHabit("Drinking_Water");
    }

    internal void MainMenu()
    {
        var isRunning = true;
        while (isRunning) 
        {
            DisplayMenuItems();
            var select = Console.ReadKey(true).KeyChar;
            isRunning = select switch
            {
                '1' => UpdateDailyProgress(),
                '2' => ViewCurrentProgress(),
                '3' => ViewEntireProgress(),
                '4' => SetDailyGoal(),
                '5' => DeleteCurrentProgress(),
                '6' => DeleteAllProgress(),
                '0' => false,
                _ => true
            };
        }

    }

    private bool DeleteAllProgress()
    {
        var validCoice = !ViewEntireProgress();
        Console.WriteLine("Are you sure you want to delete all records? (Yes/No)");

        while (validCoice == false)
        {
            var answer = Console.ReadLine()!.Trim().ToLower();
            if (answer == "yes")
            {
                db.DeleteAllProgress(habit);
                validCoice = true;
                Console.WriteLine(nl + "All records has been deleted.");
            }
            else if (answer == "no")
            {
                validCoice = true;
            }
        }

        return true;
    }

    private bool DeleteCurrentProgress()
    {
        var validChoice = !ViewCurrentProgress();
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("dd.MM.yyyy");
        Console.WriteLine("Are you sure you want to delete that record? (Yes/No)");
        while (validChoice == false)
        {
            var answer = Console.ReadLine()!.Trim().ToLower();
            if (answer == "yes")
            {
                db.DeleteCurrentProgress(habit, today);
                validChoice = true;
                Console.WriteLine(nl + "Record deleted.");
                Console.ReadLine();
            } else if (answer == "no")
            {
                validChoice = true;
            }
        }

        return true;
    }

    private bool ViewEntireProgress()
    {
        HeaderText();
        habit.ProgressList = db.LoadAllProgress(habit);
        var progress = habit.ProgressList.OrderByDescending(x => x.Date);

        foreach (var day in progress)
            Console.WriteLine($" Day: {day.Date}, [ {day.Quantity} / {day.DailyGoal} ] {habit.UnitOfMeasurement}");

        Console.ReadKey();

        return true;
    }

    private bool ViewCurrentProgress()
    {
        HeaderText();
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("dd.MM.yyyy");

        db.SaveProgress(habit, today, 0);   // This ensures that, when selected, Current Progress will always include current day data
        habit.ProgressList = db.LoadCurrentProgress(habit);

        var current = habit.ProgressList.LastOrDefault();
        Console.WriteLine($"Todays progress for {habit.Name}: [ {habit.ProgressList.First().Quantity} / {habit.ProgressList.First().DailyGoal} ] {habit.UnitOfMeasurement}.");
        Console.ReadLine();

        return true;
    }

    private bool UpdateDailyProgress()
    {
        HeaderText();
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("dd.MM.yyyy");
        habit.ProgressList = db.LoadCurrentProgress(habit);

        var newProgress = AskForInteger($"Please enter the number of ({habit.UnitOfMeasurement}) since your last update: ");
        if (newProgress == 0) return true;
        else
        {
            db.SaveProgress(habit, today, newProgress);
            Console.WriteLine("Progress succesfuly updated.");
            Console.ReadKey();
            return true;
        }
    }

    private bool SetDailyGoal()
    {
        HeaderText();
        habit.ProgressList = db.LoadCurrentProgress(habit);
        Console.WriteLine($"{habit.Name} current daily goal is {habit.ProgressList.First().DailyGoal} {habit.UnitOfMeasurement}");

        var newGoal = AskForInteger("Enter new daily goal (0 to leave unchanged): ");

        if (newGoal == 0) return true;
        else db.SaveNewGoal(habit, newGoal);

        return true;
        
    }

    private int AskForInteger(string message)
    {
        Console.Write(message);
        var input = Console.ReadLine()!.Trim();
        if (int.TryParse(input, out var value) && value >= 0)
            return value;
        else
        {
            Console.WriteLine("Invalid input. Please enter a whole, positive number." + nl);
            return AskForInteger(message);
        }
    }

    private void HeaderText()
    {
        Console.Clear();
        Console.WriteLine("============================");
        Console.WriteLine("   Habit Logger by kraven   ");
        Console.WriteLine("============================" + nl);
    }

    private void DisplayMenuItems()
    {
        HeaderText();
        Console.WriteLine("  1 - Update daily progress");
        Console.WriteLine("  2 - View todays progress");
        Console.WriteLine("  3 - View entire progress");
        Console.WriteLine("  4 - Set habit daily goal");
        Console.WriteLine("  5 - Delete current progress");
        Console.WriteLine("  6 - Delete all progress");

        Console.WriteLine($"{nl}  0 - Quit");
    }
}
