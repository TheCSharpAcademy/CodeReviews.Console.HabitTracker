using Models;
using Repository;

namespace Services;

public class HabitService
{
    private HabitRepo _habitRepo { get; set; }
    public HabitService()
    {
        _habitRepo = new HabitRepo();
    }
    public bool AddNewHabit(Habit habit, int userId)
    {
        bool result = false;
        if (habit != null)
        {
            result = _habitRepo.AddNewHabit(habit, userId);
        }
        return result;
    }
    public Habit? GetHabitById(int habitId, int userId)
    {
        Habit? result = null;
        if (habitId >= 0 && userId >= 0)
        {
            result = _habitRepo.GetHabit(habitId, userId);
        }
        return result;
    }
    public bool DeleteHabit(int habitId, int userId)
    {
        bool result = false;
        if (habitId >= 0 && userId >= 0)
        {
            result = _habitRepo.DeleteAHabit(habitId, userId);
        }
        return result;
    }

    public List<Habit>? GetAllHabit(int userId)
    {
        List<Habit>? result = null;
        if (userId >= 0)
        {
            result = _habitRepo.GetAllRecords(userId);
        }
        return result;
    }
    public bool DeleteAllHabits(int userId)
    {
        bool result = false;
        if (userId >= 0)
        {
            result = _habitRepo.DeleteAllHabits(userId);
        }
        return result;
    }

    public bool UpdateHabit(Habit habit, int userId)
    {
        bool result = false;
        if (habit != null && userId >= 0)
        {
            result = _habitRepo.UpdateHabit(habit, userId);
        }
        return result;
    }
}