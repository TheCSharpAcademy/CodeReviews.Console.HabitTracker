using Nelson.Habit_Tracker.DataAccess;
using Nelson.Habit_Tracker.Models;
using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.HabitApp
{
    public class HabitApp
    {
        private readonly IConsoleInteraction _consoleInteraction;
        private readonly IHabitRepository _habitRepository;
        bool closeApp;
        readonly Habit habit = new();

        public HabitApp(IConsoleInteraction consoleInteraction, IHabitRepository habitRepository)
        {
            _consoleInteraction = consoleInteraction;
            _habitRepository = habitRepository;
        }

        public void RunApp()
        {
            while (!closeApp)
            {
                // Get user input
                string userInput = _habitRepository.GetUserInput();

                // Run command based on user input
                SelectUserInput(userInput);
            }
        }

        public void SelectUserInput(string input)
        {
            switch (input)
            {
                case "0":
                    _consoleInteraction.ShowMessage("\nGoodbye!\n");
                    closeApp = true;
                    break;
                case "1":
                    _consoleInteraction.ShowMessage("Getting all habits...");
                    _habitRepository.GetAllHabits();
                    break;
                case "2":
                    _habitRepository.InsertHabit(habit);
                    break;
                case "3":
                    _habitRepository.UpdateHabit();
                    break;
                case "4":
                    _habitRepository.DeleteHabit();
                    break;
                default:
                    _consoleInteraction.ShowMessage("\nInvalid input. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}