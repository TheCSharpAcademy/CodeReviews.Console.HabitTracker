using FunRun.HabitTracker.Data.Model;

namespace FunRun.HabitTracker.Services.Interfaces;

public interface ISqlOperations
{
    void SqlCreateHabit(HabitModel newHabit);
    void SqlDeleteHabit(HabitModel newHabit);
    List<HabitModel> SqlReadAllHabits();
    void SqlUpdateHabit(HabitModel newHabit);
}