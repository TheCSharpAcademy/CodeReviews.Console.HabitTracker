
using System.Globalization;
using HabitTracker.Models;

namespace HabitTracker;

internal class App
{
    private readonly string dbPath = "habits.db";
    private List<Habit>? habits;
    private List<HabitLog>? habitLogs;
    internal HabitTrackerDbContext dbContext { get; init; } = default!;
    //internal Menu menu { get; set; }

    internal App()
    {
        dbContext = new HabitTrackerDbContext(dbPath);
        UpdateData();
    }

    internal void Run()
    {
        bool runApp = true;
        while (runApp)
        {
            var selectedOption = Menu.ShowMainMenu();
            switch (selectedOption)
            {
                case 0:
                    runApp = false;
                    break;
                case 1:
                    Menu.ShowAllHabitLogs(habitLogs);
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();
                    break;
                case 2:
                    InsertNewHabitLog();
                    break;
                case 3:
                    DeleteHabitLog();
                    break;
                case 4:
                    UpdateHabitLog();
                    break;
                case 5:
                    Menu.ShowAllHabits(habits);
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();
                    break;
                case 6:
                    InsertNewHabit();
                    break;
                case 7:
                    ViewYearlyReport();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();
                    break;
                default:
                    break;
            }
        }


    }

    private void ViewYearlyReport()
    {
        List<HabitReport>? report = dbContext.GenerateYearlyReport();
        Menu.ShowReport(report);
    }

    private void InsertNewHabit()
    {
        string HabitDescription = Helpers.GetStringInput("Please enter new Habit Description");
        string HabitUnit = Helpers.GetStringInput("Please enter Habit Unit");
        dbContext.InsertHabit(HabitDescription, HabitUnit);
        Console.WriteLine($"The New Habit ({HabitDescription}, {HabitUnit}) is created successfully.");
        UpdateData();
        Console.WriteLine("Press any key to continue.");
        Console.ReadLine();
    }

    private void UpdateHabitLog()
    {
        int id = Helpers.GetHabitLogId(habitLogs);
        int quantity = Helpers.GetIntegerValue($"Enter Quantity new Quantity");
        string Date = Helpers.GetDateInput();
        int result = dbContext.UpdateHabitLog(id, Date, quantity);
        if (result == 0)
        {
            Console.WriteLine($"The record with id: {id} is not found.");
            DeleteHabitLog();
        }
        else
        {
            Console.WriteLine($"The record with id: {id} is updated successfully.");
            Console.WriteLine("Press any key to continue.");
            UpdateData();
            Console.ReadLine();
        }
    }

    private void DeleteHabitLog()
    {
        int id = Helpers.GetHabitLogId(habitLogs);
        var result = dbContext.DeleteHabitLog(id);
        if (result == 0)
        {
            Console.WriteLine($"The record with id: {id} is not found.");
            DeleteHabitLog();
        }
        else
        {
            Console.WriteLine($"The record with id: {id} is deleted successfully.");
            Console.WriteLine("Press any key to continue.");
            UpdateData();
            Console.ReadLine();
        }
    }

    private void InsertNewHabitLog()
    {
        Habit? habit = Helpers.GetHabitId(habits);
        Console.WriteLine($"Habit: {habit.HabitDescription}, Unit: {habit.Unit}");
        int quantity = Helpers.GetIntegerValue($"Enter Quantity in ({habit.Unit}) for {habit.HabitDescription}");
        Console.WriteLine($"You entered: {quantity}");
        string Date = Helpers.GetDateInput();
        dbContext.InsertHabitLog(Date, quantity, habit.Id);
        Console.WriteLine($"New record ({habit.HabitDescription}, {Date}, {quantity}) is added successfully");
        Console.WriteLine("Press any key to continue.");
        Console.ReadLine();
        UpdateData();
    }

    private void UpdateData()
    {
        habits = dbContext.GetAllHabits();
        habitLogs = dbContext.GetAllHabitLogs();
    }

}