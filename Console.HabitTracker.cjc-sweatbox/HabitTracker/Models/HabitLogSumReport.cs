// --------------------------------------------------------------------------------------------------
// HabitTracker.Models.HabitLogSumReport
// --------------------------------------------------------------------------------------------------
// Habit Log Sum Report data transformation object.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Entities;

namespace HabitTracker.Models;

public class HabitLogSumReport
{
    #region Constructors

    public HabitLogSumReport(HabitLogSumReportEntity entity)
    {
        Name = entity.Name;
        Measure = entity.Measure;
        Quantity = entity.Quantity;
    }

    #endregion
    #region Properties

    public string? Name { get; set; }
    
    public string? Measure { get; set; }

    public int Quantity { get; set; }

    #endregion
}
