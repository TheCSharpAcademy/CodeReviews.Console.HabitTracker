// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Entities.HabitReportEntity
// --------------------------------------------------------------------------------------------------
// Represents a database entity in the vw_habit_report view.
// --------------------------------------------------------------------------------------------------
using System.Data;
using HabitTracker.Data.Extensions;

namespace HabitTracker.Data.Entities;

public class HabitReportEntity
{
    #region Constructors

    public HabitReportEntity(IDataReader reader)
    {
        Id = reader.GetInt32("habit_id");
        Name = reader.GetString("name");
        Measure = reader.GetString("measure");
        IsActive = reader.GetBoolean("is_active");
    }

    #endregion
    #region Properties

    public int Id { get; }

    public string? Name { get; }
    
    public string? Measure { get; }

    public bool IsActive { get; }

    #endregion
}
