namespace Nelson.Habit_Tracker.DataAccess
{
    public interface IHabitRepository
    {
        void DeleteHabit();
        void GetAllHabits();
        string GetUserInput();
        void InsertHabit();
        void UpdateHabit();
    }
}