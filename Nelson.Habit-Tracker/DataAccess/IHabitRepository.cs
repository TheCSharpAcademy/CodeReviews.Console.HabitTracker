using Nelson.Habit_Tracker.Models;

namespace Nelson.Habit_Tracker.DataAccess
{
    public interface IHabitRepository
    {
        void DeleteHabit();
        void GetAllHabits();
        string GetUserInput();
        void InsertHabit(Habit habit);
        void UpdateHabit();
    }
}