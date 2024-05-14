using Nelson.Habit_Tracker.Models;
using Nelson.Habit_Tracker.UserConsoleInteraction;
using Nelson.Habit_Tracker.Utils;

namespace Nelson.Habit_Tracker.DataAccess
{
    public class HabitRepository : IHabitRepository
    {
        private readonly IConsoleInteraction? _consoleInteraction;
        private readonly IInputValidator _inputValidator;
        private readonly IDateValidator _dateValidator;

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

        public void InsertHabit(Habit habit)
        {
            habit.Date = _dateValidator.GetDateInput();
            habit.Quantity = _inputValidator.ConvertToInt(_inputValidator.GetQualityInput());
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