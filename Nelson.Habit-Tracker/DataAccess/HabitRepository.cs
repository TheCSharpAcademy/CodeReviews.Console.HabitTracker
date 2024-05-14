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

        public void GetAllHabits()
        {
            throw new NotImplementedException();
        }

        public void InsertHabit()
        {
            
        }

        public void UpdateHabit()
        {
            throw new NotImplementedException();
        }
        public void DeleteHabit()
        {
            throw new NotImplementedException();
        }
    }
}