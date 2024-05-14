using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.DataAccess
{
    public class HabitRepository : IHabitRepository
    {
        private readonly IConsoleInteraction? _consoleInteraction;
        public string GetUserInput()
        {
            Console.Clear();
            _consoleInteraction.DisplayMenu();
            return Console.ReadLine();
        }
    }
}