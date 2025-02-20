using System.ComponentModel.DataAnnotations;

namespace HabitLogger.mrgee1978.PresentationLayer.Enumerations;


public enum MenuOptions
{
    [Display (Name = "Add Habit")]
    AddHabit,

    [Display (Name = "Update Habit")]
    UpdateHabit,

    [Display (Name = "Delete Habit")]
    DeleteHabit,

    [Display (Name = "View Habits")]
    ViewHabits,

    [Display (Name = "Add Record")]
    AddRecord,

    [Display (Name = "Update Record")]
    UpdateRecord,

    [Display (Name = "Delete Record")]
    DeleteRecord,

    [Display (Name = "View Records")]
    ViewRecords,

    [Display (Name = "Quit Program")]
    Quit,
}
