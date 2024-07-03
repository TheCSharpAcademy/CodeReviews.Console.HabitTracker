// --------------------------------------------------------------------------------------------------
// HabitTracker.ConsoleApp.Views.SetHabitLogPage
// --------------------------------------------------------------------------------------------------
// Gets the required input from a user to update an existing habit log.
// --------------------------------------------------------------------------------------------------
using System;
using HabitTracker.ConsoleApp.Utilities;
using HabitTracker.Models;

namespace HabitTracker.ConsoleApp.Views;

internal class SetHabitLogPage : BasePage
{
    #region Constants

    private const string PageTitle = "Update Habit Log";

    #endregion
    #region Methods: Internal

    internal static HabitLog? Show(Habit habit, HabitLog habitLog)
    {
        HabitLog? nullHabitLog = null;

        Console.Clear();

        WriteHeader($"{PageTitle} ({habit.Name})");

        DateTime? date = ConsoleHelper.GetDate($"Enter the date (format yyyy-MM-dd) or 0 to retain '{habitLog.Date:yyyy-MM-dd}': ");
        if(!date.HasValue)
        {
            date = habitLog.Date;
        }
            
        int quantity = ConsoleHelper.GetInt($"Enter the quantity (format integer > 0) or 0 to retain {habitLog.Quantity}: ", 0);
        if(quantity == 0)
        {
            quantity = habitLog.Quantity;
        }

        return new HabitLog(habit.Id, date.Value, quantity);
    }

    #endregion
}
