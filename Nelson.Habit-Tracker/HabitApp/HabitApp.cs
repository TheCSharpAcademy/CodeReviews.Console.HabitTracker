using Nelson.Habit_Tracker.DataAccess;
using Nelson.Habit_Tracker.Models;
using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.HabitApp
{
    public class HabitApp
    {
        private readonly IConsoleInteraction _consoleInteraction;
        private readonly IHabitRepository _habitRepository;
        private readonly IDatabaseInitializer _dbInitializer;
        bool closeApp;
        readonly Habit habit = new();

        public HabitApp(IConsoleInteraction consoleInteraction, IHabitRepository habitRepository, IDatabaseInitializer dbInitializer)
        {
            _consoleInteraction = consoleInteraction;
            _habitRepository = habitRepository;
            _dbInitializer = dbInitializer;
        }

        public void RunApp()
        {
            // Initialize database
            _dbInitializer.InitializeDatabase();

            // Run app loop
            while (!closeApp)
            {
                // Display menu
                _consoleInteraction.DisplayMenu();
                
                // Get user input
                string userInput = _consoleInteraction.GetUserInput();

                // Run command based on user input
                SelectUserInput(userInput);
            }
        }

        public void SelectUserInput(string input)
        {
            switch (input)
            {
                case "0":
                    _consoleInteraction.ShowMessageTime("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    _consoleInteraction.ShowMessageTime("Getting all habits...");
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
                    _consoleInteraction.ShowMessageTime("\nInvalid input. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}