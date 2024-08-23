using HabitLogger.Models;
namespace HabitLogger.Shared
{
    public interface IHabitService
    {
        void AddHabit(Habit habit);
        void UpdateHabit(Habit habit);
        Habit GetHabit(int id);
        void DeleteHabit(int id);
    }
}

