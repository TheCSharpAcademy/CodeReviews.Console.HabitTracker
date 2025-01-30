using HabitTracker.FunRunRushFlush.Data.Model;

namespace HabitTracker.FunRunRushFlush.Services.Interfaces
{
    public interface ICrudService
    {
        void CreateOneHabit(HabitModel newHabit);
        void DeleteOneHabit(HabitModel newHabit);
        List<HabitModel> ReturnAllHabits();
        void UpdateOneHabit(HabitModel newHabit);
    }
}