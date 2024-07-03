// --------------------------------------------------------------------------------------------------
// HabitTracker.Models.HabitLogReport
// --------------------------------------------------------------------------------------------------
// Habit Log Report data transformation object.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Entities;

namespace HabitTracker.Models;

public class HabitLogReport
{
    #region Constructors

    public HabitLogReport(HabitLogReportEntity entity)
    {
        Id = entity.Id;
        HabitId = entity.HabitId;
        Name = entity.Name;
        Measure = entity.Measure;
        Date = entity.Date;
        Quantity = entity.Quantity;
    }

    #endregion
    #region Properties

    public int Id { get; set; }

    public int HabitId { get; set; }

    public DateTime Date { get; set; }

    public string? Name { get; set; }
    
    public string? Measure { get; set; }

    public int Quantity { get; set; }

    #endregion
}
