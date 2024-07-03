// --------------------------------------------------------------------------------------------------
// HabitTracker.Models.HabitLogReportConfiguration
// --------------------------------------------------------------------------------------------------
// Holds parameters for a Habit Log Report.
// --------------------------------------------------------------------------------------------------

namespace HabitTracker.Models;

public class HabitLogReportConfiguration
{
    #region Properties

    public int? HabitId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    #endregion
}
