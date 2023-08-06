using HabitTracker.MartinL_no.Models;

namespace HabitTracker.MartinL_no;

internal class HabitService
{
    private HabitRepository _habitRepo;

    public HabitService(HabitRepository habitRepo)
    {
        _habitRepo = habitRepo;
    }

    internal List<Habit> GetAllHabits()
    {
        return _habitRepo.GetHabits();
    }
}