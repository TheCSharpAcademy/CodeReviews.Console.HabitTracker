using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Models;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Domain.Repositories;

public interface IHabitRepository
{
    public IReadOnlyList<HabitDisplayDto> GetAllHabits();
    public HabitDisplayDto? GetHabitById(int id);
    public bool CreateHabit(HabitCreationDto habit);
    public bool UpdateHabit(Habit habit);
    public bool DeleteHabitById(int id);
}