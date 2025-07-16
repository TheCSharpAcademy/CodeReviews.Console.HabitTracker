using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;
using HabitTracker.Domain.Repositories;

namespace HabitTracker.Application.Services;

public class HabitService(IHabitRepository habitRepo)
{
    public IReadOnlyList<HabitDisplayDto> GetAllHabits()
    {
        return habitRepo.GetAllHabits();
    }

    public HabitDisplayDto? GetHabit(int id)
    {
        return habitRepo.GetHabitById(id);
    }

    public bool CreateHabit(HabitCreationDto habit)
    {
        return habitRepo.CreateHabit(habit);
    }

    public bool UpdateHabit(int habitId, HabitUpdateDto habit)
    {
        var updatedHabit = new Habit(habitId, habit.Name ?? "unknown", habit.Unit ?? "unknown");
        return habitRepo.UpdateHabit(updatedHabit);
    }

    public bool DeleteHabit(int id)
    {
        return habitRepo.DeleteHabitById(id);
    }
}