namespace DotNETConsole.HabitTracker.Controllers;
using UI;
using Enums;
using DataModels;

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
                    break;
                case MainUI.AddNewHabit:
                    string newHabit = userInput.GetNewHabit();
                    habitController.AddHabit(newHabit);
                    userInput.ContinueInput();
                    break;
                case MainUI.ViewHabits:
                    List<Habit> habits = habitController.GetHabits();
                    userViews.ViewHabits(habits);
                    userInput.ContinueInput();
                    break;
                case MainUI.ModifyHabit:
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
                    break;
                case MainUI.Exit:
                    Running = false;
                    break;
            }
        }
    }
}
