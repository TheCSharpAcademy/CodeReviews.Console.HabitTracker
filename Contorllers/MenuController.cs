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
            var userChoice = menu.GetChoice();
            switch (userChoice)
            {
                case MainUI.LogRecord:
                    habitController.LogHabit(userInput.LogHabit());
                    userInput.ContinueInput();
                    break;
                case MainUI.ViewLog:
                    List<HabitLog> habitLogs = habitController.GetHabitLogs();
                    userViews.ViewHabitLogs(habitLogs);
                    userInput.ContinueInput();
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
                case MainUI.Exit:
                    Running = false;
                    break;
            }
        }
    }
}
