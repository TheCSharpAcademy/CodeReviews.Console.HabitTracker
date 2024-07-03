// --------------------------------------------------------------------------------------------------
// HabitTracker.Models.HabitReport
// --------------------------------------------------------------------------------------------------
// Habit Report data transformation object.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Entities;

namespace HabitTracker.Models;

public class HabitReport
{
    #region Constructors

    public HabitReport(HabitReportEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Measure = entity.Measure;
        IsActive = entity.IsActive;
    }

    #endregion
    #region Properties

    public int Id { get; }
    
    public string? Name { get; }
    
    public string? Measure { get; }

    public bool IsActive { get; }
    
    #endregion
}
