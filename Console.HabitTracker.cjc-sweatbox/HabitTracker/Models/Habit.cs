// --------------------------------------------------------------------------------------------------
// HabitTracker.Models.Habit
// --------------------------------------------------------------------------------------------------
// Habit data transformation object.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Entities;

namespace HabitTracker.Models;

public class Habit
{
    #region Constructors

    public Habit(HabitEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Measure = entity.Measure;
        IsActive = entity.IsActive;
    }

    public Habit(string name, string measure)
    {
        Name = name;
        Measure = measure;
        
        // Default to active.
        IsActive = true;
    }

    #endregion
    #region Properties

    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Measure { get; set; }

    public bool IsActive { get; set; }

    #endregion
}
