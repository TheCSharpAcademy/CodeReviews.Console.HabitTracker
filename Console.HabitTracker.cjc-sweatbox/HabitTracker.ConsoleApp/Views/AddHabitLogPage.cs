// --------------------------------------------------------------------------------------------------
// HabitTracker.ConsoleApp.Views.AddHabitLogPage
// --------------------------------------------------------------------------------------------------
// Gets the required input from a user to add a new habit log.
// --------------------------------------------------------------------------------------------------
using HabitTracker.ConsoleApp.Utilities;
using HabitTracker.Models;

namespace HabitTracker.ConsoleApp.Views;

internal class AddHabitLogPage : BasePage
{
    #region Constants

    private const string PageTitle = "Record Habit";

    #endregion
    #region Methods: Internal

    internal static HabitLog? Show(Habit habit)
    {
        HabitLog? nullHabitLog = null;

        Console.Clear();

        WriteHeader($"{PageTitle} ({habit.Name})");

        DateTime? date = ConsoleHelper.GetDate("Enter the date (format yyyy-MM-dd) or 0 to return to main menu: ");
        if(!date.HasValue)
        {
            return nullHabitLog;
        }
            
        int quantity = ConsoleHelper.GetInt("Enter the quantity (format integer > 0) or 0 to return to main menu: ", 0);
        if(quantity == 0)
        {
            return nullHabitLog;
        }

        return new HabitLog(habit.Id, date.Value, quantity);
    }

    #endregion
}
