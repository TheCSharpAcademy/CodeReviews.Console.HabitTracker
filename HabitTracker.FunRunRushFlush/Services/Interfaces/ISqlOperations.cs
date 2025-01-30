using HabitTracker.FunRunRushFlush.Data.Model;

namespace HabitTracker.FunRunRushFlush.Services.Interfaces;

public interface ISqlOperations
{
    void SqlCreateHabit(HabitModel newHabit);
    void SqlDeleteHabit(HabitModel newHabit);
    List<HabitModel> SqlReadAllHabits();
    void SqlUpdateHabit(HabitModel newHabit);
}