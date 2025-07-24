using System.ComponentModel.DataAnnotations;

namespace HabitTracker.TruthfulUK;

internal class Enums
{
    internal enum MainMenu
    {
        [Display(Name = "Manage Habit Logs")]
        ManageHabitLogs,

        [Display(Name = "Add a New Habit")]
        AddHabit,

        [Display(Name = "Habit Report")]
        HabitReport,

        [Display(Name = "Exit Application")]
        ExitApplication
    }

    internal enum ManageHabitLogs
    {
        [Display(Name = "Add New Habit Log")]
        AddNewHabitLog,

        [Display(Name = "View Habit Logs")]
        ViewHabitLogs,

        [Display(Name = "Delete a Habit Log")]
        DeleteHabitLog,

        [Display(Name = "Update a Habit Log")]
        UpdateHabitLog,

        [Display(Name = "Back to Main Menu")]
        BackToMainMenu
    }

    internal enum ReportOptions
    {
        [Display(Name = "Day Report")]
        DayReport,

        [Display(Name = "Date Range Report")]
        DateRangeReport,

        [Display(Name = "Back to Main Menu")]
        BackToMainMenu
    }
}


