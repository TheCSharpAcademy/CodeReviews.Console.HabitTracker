// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Entities.HabitLogSumReportEntity
// --------------------------------------------------------------------------------------------------
// Represents a modified database entity in the vw_habit_log_report view.
// --------------------------------------------------------------------------------------------------
using System.Data;
using HabitTracker.Data.Extensions;

namespace HabitTracker.Data.Entities;

public class HabitLogSumReportEntity
{
    #region Constructors

    public HabitLogSumReportEntity(IDataReader reader)
    {
        Name = reader.GetString("name");
        Measure = reader.GetString("measure");
        Quantity = reader.GetInt32("quantity");
    }

    #endregion
    #region Properties

    public string? Name { get; set; }

    public string? Measure { get; set; }

    public int Quantity { get; set; }

    #endregion
}
