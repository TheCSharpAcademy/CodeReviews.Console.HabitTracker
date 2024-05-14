using Nelson.Habit_Tracker.DataAccess;
using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.HabitApp
{
    public class HabitApp
    {
        private readonly IConsoleInteraction _consoleInteraction;
        private readonly IHabitRepository _habitRepository;
        bool closeApp;

        public HabitApp(IConsoleInteraction consoleInteraction, IHabitRepository habitRepository)
        {
            _consoleInteraction = consoleInteraction;
            _habitRepository = habitRepository;
        }

        public void RunApp()
        {
            while (!closeApp)
            {
                string userInput = _habitRepository.GetUserInput();
            }
        }

        public string SelectUserInput(string input)
        {
            switch (input)
            {
                case "0":
                    _consoleInteraction.ShowMessage("\nGoodbye!\n");
                    closeApp = true;
                    break;
            }
        }
    }
}