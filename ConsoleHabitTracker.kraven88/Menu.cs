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
                //'1' => UpdateDailyProgress(),
                //'2' => ViewCurrentProgress(),
                //'3' => ViewEntireProgress(),
                '4' => SetDailyGoal(),
                '0' => false,
                _ => true
            }; ;
        }

    }

    private bool SetDailyGoal()
    {
        HeaderText();
        Console.WriteLine($"{habit.Name} current daily goal is {habit.CurrentGoal} {habit.UnitOfMeasurement}");

        var newGoal = AskForInteger("Enter new daily goal (0 to leave unchanged): ");

        if (newGoal == 0) return true;
        else db.SetNewGoal(habit, newGoal);

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

        Console.WriteLine($"{nl}  0 - Quit");
    }
}
