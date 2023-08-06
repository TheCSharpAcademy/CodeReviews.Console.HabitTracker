using HabitTracker.MartinL_no.Models;

namespace HabitTracker.MartinL_no;

internal class HabitService
{
    private HabitRepository _repo;

    internal HabitService(HabitRepository habitRepo)
    {
        _repo = habitRepo;
    }

    internal List<Habit> GetAll()
    {
        return _repo.GetHabits();
    }

    internal void Add(string? name)
    {
        if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException();

        _repo.AddHabit(name);
    }
}