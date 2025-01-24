using FunRun.HabitTracker.Data.Model;

namespace FunRun.HabitTracker.Services.Interfaces
{
    public interface ISQLOperations
    {
        void SQLCreateHabit(HabitModel newHabit);
        void SQLDeleteHabit(HabitModel newHabit);
        List<HabitModel> SQLReadAllHabits();
        void SQLUpdateHabit(HabitModel newHabit);
    }
}