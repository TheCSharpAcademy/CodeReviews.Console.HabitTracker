// --------------------------------------------------------------------------------------------------
// HabitTracker.ConsoleApp.Views.ConfigureHabitLogReportPage
// --------------------------------------------------------------------------------------------------
// Gets the required input from a user to configure the habit log report.
// --------------------------------------------------------------------------------------------------
using System.Text;
using HabitTracker.ConsoleApp.Utilities;
using HabitTracker.Models;

namespace HabitTracker.ConsoleApp.Views;

internal class ConfigureHabitLogReportPage : BasePage
{
    #region Constants

    private const string PageTitle = "Configure Habit Log Report";

    #endregion
    #region Properties

    private static string DateOptionText
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine("Select an option...");
            sb.AppendLine();
            sb.AppendLine("0 - Back to main menu");
            sb.AppendLine("1 - View all dates");
            sb.AppendLine("2 - View quantity within date range");
            sb.AppendLine();
            return sb.ToString();

        }
    }

    #endregion
    #region Methods: Internal

    internal static HabitLogReportConfiguration? Show(List<Habit> habits)
    {
        HabitLogReportConfiguration? nullOutput = null;

        // Default values.
        int? habitId = null;
        DateTime? dateFrom = null;
        DateTime? dateTo = null;

        Console.Clear();
        
        WriteHeader($"{PageTitle}");

        WriteHabitOptions(habits);
        int habitOption = ConsoleHelper.GetInt("Enter your selection: ", 0, habits.Count + 1);

        switch (habitOption)
        {
            case 0:
                // Back to main menu.
                return nullOutput;
            case 1:
                // View all habits.
                habitId = null;
                break;
            default:
                // View specific habit.
                // NOTE: option is 2-based (list is 0-based)
                habitId = habits[habitOption - 2].Id;
                break;
        }

        Console.Clear();

        WriteHeader($"{PageTitle}");

        Console.Write(DateOptionText);
        int dateOption = ConsoleHelper.GetInt("Enter your selection: ", 0, 2);

        switch (dateOption)
        {
            case 0:
                // Back to main menu.
                return nullOutput;
            case 1:
                // View all dates.
                dateFrom = null;
                dateTo = null;
                break;
            case 2:
                // View quantity within date range.
                DateTime? userDateFrom = ConsoleHelper.GetDate($"Enter the report start date (format yyyy-MM-dd) or 0 for min date: ");
                dateFrom = userDateFrom.HasValue ? userDateFrom.Value : DateTime.MinValue;

                DateTime? userDateTo = ConsoleHelper.GetDate($"Enter the report end date (format yyyy-MM-dd) or 0 for max date: ");
                dateTo = userDateTo.HasValue ? userDateTo.Value : DateTime.MaxValue;
                break;
            default:
                break;
        }

        return new HabitLogReportConfiguration()
        {
            HabitId = habitId,
            DateFrom = dateFrom,
            DateTo = dateTo
        };
    }

    #endregion
    #region Methods: Private

    private static void WriteHabitOptions(List<Habit> habits)
    {
        Console.WriteLine("Select an option...");
        Console.WriteLine();
        Console.WriteLine("0 - Back to main menu");
        Console.WriteLine("1 - View all habits");
        for (int i = 0; i < habits.Count; i++)
        {
            Console.WriteLine($"{i + 2} - {habits[i].Name}");
        }
        Console.WriteLine();
    }

    #endregion
}
