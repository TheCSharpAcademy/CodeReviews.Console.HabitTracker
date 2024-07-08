using HabitTracker.kwm0304.Data;
using HabitTracker.kwm0304.Models;

namespace HabitTracker.kwm0304.Repositories;

public class HabitRepository
{
    private readonly DbActions _dbActions;
    public HabitRepository()
    {
      _dbActions = new DbActions();
      _dbActions.CreateDatabaseOnStart();
    }
    public void CreateHabit(Habit habit)
    {
      _dbActions.InsertHabit(habit);
    }
    public Habit GetHabit(int habitId)
    {
      return _dbActions.GetHabitById(habitId)!;
    }
    public IEnumerable<Habit> GetHabits()
    {
      return _dbActions.GetHabits();
    }
    public void UpdateHabitRepetitions(int reps, int habitId)
    {
      _dbActions.UpdateHabitRepetitions(reps, habitId);
    }
    public void UpdateHabitFields(string field, string newField,int habitId)
    {
      _dbActions.UpdateHabitFields(field, newField, habitId);
    }
    public void DeleteHabit(int habitId)
    {
      _dbActions.DeleteHabit(habitId);
    }
}
