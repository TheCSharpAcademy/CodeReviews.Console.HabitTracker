// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Entities.HabitEntity
// --------------------------------------------------------------------------------------------------
// Represents a database entity in the habit table.
// --------------------------------------------------------------------------------------------------
using System.Data;
using HabitTracker.Data.Extensions;

namespace HabitTracker.Data.Entities;

public class HabitEntity
{
    #region Constructors

    public HabitEntity(IDataReader reader)
    {
        Id = reader.GetInt32("habit_id");
        Name = reader.GetString("name");
        Measure = reader.GetString("measure");
        IsActive = reader.GetBoolean("is_active");
    }

    #endregion
    #region Properties

    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Measure { get; set; }

    public bool IsActive { get; set; }

    #endregion
}
