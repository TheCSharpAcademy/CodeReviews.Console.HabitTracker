// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Entities.HabitLogReportEntity
// --------------------------------------------------------------------------------------------------
// Represents a database entity in the vw_habit_log_report view.
// --------------------------------------------------------------------------------------------------
using System.Data;
using HabitTracker.Data.Extensions;

namespace HabitTracker.Data.Entities;

public class HabitLogReportEntity
{
    #region Constructors

    public HabitLogReportEntity(IDataReader reader)
    {
        Id = reader.GetInt32("habit_log_id");
        HabitId = reader.GetInt32("habit_id");
        Name = reader.GetString("name");
        Measure = reader.GetString("measure");
        Date = reader.GetDateTime("date");
        Quantity = reader.GetInt32("quantity");
    }

    #endregion
    #region Properties

    public int Id { get; set; }

    public int HabitId { get; set; }
    
    public string? Name { get; set; }

    public string? Measure { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    #endregion
}
