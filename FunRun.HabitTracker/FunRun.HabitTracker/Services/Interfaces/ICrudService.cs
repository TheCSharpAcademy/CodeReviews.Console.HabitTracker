using FunRun.HabitTracker.Data.Model;

namespace FunRun.HabitTracker.Services.Interfaces
{
    public interface ICrudService
    {
        void CreateOneHabit(HabitModel newHabit);
        List<HabitModel> ReturnAllHabits();
    }
}