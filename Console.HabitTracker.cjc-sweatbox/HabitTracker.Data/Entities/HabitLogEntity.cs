// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Entities.HabitLogEntity
// --------------------------------------------------------------------------------------------------
// Represents a database entity in the habit_log table.
// --------------------------------------------------------------------------------------------------
using System.Data;
using HabitTracker.Data.Extensions;

namespace HabitTracker.Data.Entities;

public class HabitLogEntity
{
    #region Constructors

    public HabitLogEntity(IDataReader reader)
    {
        Id = reader.GetInt32("habit_log_id");
        HabitId = reader.GetInt32("habit_id");
        Date = reader.GetDateTime("date");
        Quantity = reader.GetInt32("quantity");
    }

    #endregion
    #region Properties

    public int Id { get; set; }

    public int HabitId { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    #endregion
}
