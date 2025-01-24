using FunRun.HabitTracker.Data.Model;

namespace FunRun.HabitTracker.Services.Interfaces
{
    public interface ICrudService
    {
        void CreateOneHabit(HabitModel newHabit);
        void DeleteOneHabit(HabitModel newHabit);
        List<HabitModel> ReturnAllHabits();
        void UpdateOneHabit(HabitModel newHabit);
    }
}