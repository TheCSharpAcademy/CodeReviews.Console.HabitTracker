using HabitTracker.MartinL_no.Models;
using HabitTracker.MartinL_no.DataAccessLayers;

namespace HabitTracker.MartinL_no.Services;

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

    internal HabitTotal GetTotal(int habitId)
    {
        return _repo.GetHabitTotal(habitId);
    }

    internal HabitTotal GetTotalSinceDate(int habitId, DateOnly date)
    {
        return _repo.GetHabitTotalSinceDate(habitId, date);
    }

    internal void Add(string name)
    {
        if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid entry");

        try
        {
            // If there is no habit in the database an InvalidOperationException is thrown
            var currentHabit = Get();
        }
        catch (InvalidOperationException)
        {
            // Add habit if there isn't one already in the database
           _repo.AddHabit(new Habit(name, new List<HabitDate>()));
            return;
        }
        // New exception thrown when there is already a habit in the database
        throw new InvalidOperationException("Another habit is already stored in the system");
    }

    internal void Delete()
    {
        _repo.DeleteHabit();
    }

    internal void AddDate(DateOnly date, int repetitions)
    {
        if (repetitions < 1) throw new ArgumentException();

        var habit = Get();

        _repo.AddDate(new HabitDate(date, repetitions, habit.Id));
    }

    internal void UpdateDate(DateOnly date, int repetitions)
    {
        if (repetitions < 1) throw new ArgumentException();

        var habit = Get();

        _repo.UpdateDate(new HabitDate(date, repetitions, habit.Id));
    }

    internal void DeleteDate(DateOnly date)
    {
        _repo.DeleteDate(date);
    }
}