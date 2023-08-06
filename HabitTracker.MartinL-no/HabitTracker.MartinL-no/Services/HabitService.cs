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
        return _repo.GetAllHabits();
    }

    internal Habit Get(string name)
    {
        return _repo.GetHabitByName(name);
    }

    internal void Add(string? name)
    {
        if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException();
        else if (HabitAlreadyExists(name)) throw new InvalidOperationException();

        _repo.AddHabit(name);
    }

    private bool HabitAlreadyExists(string? name)
    {
        try
        {
            return Get(name).Name == name;
        }
        catch
        {
            return false;
        }
    }
}