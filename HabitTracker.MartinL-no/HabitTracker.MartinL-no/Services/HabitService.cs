using HabitTracker.MartinL_no.Models;

namespace HabitTracker.MartinL_no;

internal class HabitService
{
    private HabitRepository _repo;

    internal HabitService(HabitRepository habitRepo)
    {
        _repo = habitRepo;
    }

    internal Habit Get()
    {
        return _repo.GetHabit();
    }

    internal void Add(string? name)
    {
        if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid entry");

        try
        {
            // If there is no habit in the database an InvalidOperationException is thrown
            var currentHabit = Get();
        }
        // Add habit if there isn't one in the database
        catch (InvalidOperationException)
        {
           _repo.AddHabit(name);
            return;
        }
        // New exception thrown when there is already a habit in the database
        throw new InvalidOperationException("Another habit is already stored in the system");
    }

    internal void Delete()
    {
        _repo.DeleteHabit();
    }

    internal void AddRecord(DateOnly date, int repetitionsCount)
    {
        if (repetitionsCount < 1) throw new ArgumentException();

        var habit = Get();

        _repo.AddHabitRecord(new HabitDate(date, repetitionsCount, habit.Id));
    }
}