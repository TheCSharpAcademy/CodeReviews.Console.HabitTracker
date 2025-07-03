namespace DotNETConsole.HabitTracker.Controllers;
using UI;
using Enums;
using DataModels;
using Helper;
using DbMigrations;

public class MenuController
{
    static bool Running { get; set; }

    public MenuController()
    {
        Running = true;
    }

    public void MainMenu()
    {
        var menu = new Menu();
        var userInput = new UserInput();
        var userViews = new UserViews();
        var habitController = new HabitController();
        while (Running)
        {
            Console.Clear();
            var userChoice = menu.GetChoice();
            switch (userChoice)
            {
                case MainUI.LogHabit:
                    long numberOfHabit = habitController.HabitCount();
                    if (numberOfHabit < 1)
                    {
                        Console.WriteLine("No Habit Found to log! Please Add new habit then try again.");
                    }
                    else
                    {
                        habitController.LogHabit(userInput.LogHabit());
                    }
                    userInput.ContinueInput();
                    break;
                case MainUI.ViewHabitLog:
                    List<HabitLogView> habitLogs = habitController.GetHabitLogs();
                    userViews.ViewHabitLogs(habitLogs);
                    userInput.ContinueInput();
                    break;
                case MainUI.ModifyHabitLog:
                    var numberOfLog = habitController.HabitLogCount();
                    if (numberOfLog < 1)
                    {
                        Console.WriteLine("No Habit Log Found! Log habits then try again.");
                        userInput.ContinueInput();
                    }
                    else
                    {
                        var habitLog = menu.SelectSingleHabitLog();
                        userViews.HabitLogSummary(habitLog, "Selected");
                        var userAction = userInput.ModifyOptionPrompt();
                        switch (userAction?.Key)
                        {
                            case ConsoleKey.Escape:
                                break;
                            case ConsoleKey.D:
                                habitController.RemoveLog(habitLog.LogId);
                                userInput.ContinueInput($"{habitLog.HabitTitle} - deleted successfully!");
                                break;
                            case ConsoleKey.E:
                                Console.Clear();
                                userViews.HabitLogSummary(habitLog, "Editing");
                                habitController.UpdateHabitLog(habitLog.LogId);
                                userInput.ContinueInput();
                                break;
                        }
                    }
                    break;
                case MainUI.AddNewHabit:
                    (string newHabit, string unit)= userInput.GetNewHabit();
                    habitController.AddHabit(newHabit, unit);
                    userInput.ContinueInput();
                    break;
                case MainUI.ViewHabits:
                    List<DataModels.Habit> habits = habitController.GetHabits();
                    userViews.ViewHabits(habits);
                    userInput.ContinueInput();
                    break;
                case MainUI.ModifyHabit:
                    long numberOfHabits = habitController.HabitCount();
                    if (numberOfHabits < 1)
                    {
                        Console.WriteLine("No Habit Found to Modify! Please Add new habit then try again.");
                        userInput.ContinueInput();
                    }
                    else
                    {
                        var habit = menu.SelectSingleHabit();
                        userViews.HabitSummary(habit, "Selected");
                        var habitModificationAction = userInput.ModifyOptionPrompt();
                        switch (habitModificationAction?.Key)
                        {
                            case ConsoleKey.Escape:
                                break;
                            case ConsoleKey.D:
                                habitController.RemoveHabit(habit.Id);
                                userInput.ContinueInput($"{habit.Title} - deleted successfully!");
                                break;
                            case ConsoleKey.E:
                                Console.Clear();
                                userViews.HabitSummary(habit, "Editing");
                                habitController.UpdateHabit(habit.Id);
                                userInput.ContinueInput();
                                break;
                        }
                    }
                    break;
                case MainUI.Reset:
                    string confimation = userInput.DeleteConfimation();
                    if(confimation == "yes")
                    {
                        var migration = new Migrations();
                        migration.MigrateReset();
                        userInput.ContinueInput("Database Reseted!!!");
                    }
                    Console.Clear();
                    break;
                case MainUI.Exit:
                    Running = false;
                    Console.Clear();
                    break;
            }
        }
    }
}
