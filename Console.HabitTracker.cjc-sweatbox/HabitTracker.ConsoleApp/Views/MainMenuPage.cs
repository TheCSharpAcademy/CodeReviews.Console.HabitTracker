// --------------------------------------------------------------------------------------------------
// HabitTracker.ConsoleApp.Views.MainMenuPage
// --------------------------------------------------------------------------------------------------
// The main menu page view for the application.
// --------------------------------------------------------------------------------------------------
using System.Data;
using System.Text;
using ConsoleTableExt;
using HabitTracker.ConsoleApp.Enums;
using HabitTracker.ConsoleApp.Utilities;
using HabitTracker.Models;

namespace HabitTracker.ConsoleApp.Views;

internal class MainMenuPage : BasePage
{
    #region Constants

    private const string PageTitle = "Main Menu";

    #endregion
    #region Fields

    private readonly HabitTrackerService _habitTrackerService;

    #endregion
    #region Constructors

    public MainMenuPage(HabitTrackerService habitTrackerService)
    {
        _habitTrackerService = habitTrackerService;
    }

    #endregion
    #region Properties

    internal static string MenuText
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine("Select an option...");
            sb.AppendLine();
            sb.AppendLine("0 - Close application");
            sb.AppendLine();
            sb.AppendLine("- Recording -");
            sb.AppendLine("1 - Record habit");
            sb.AppendLine();
            sb.AppendLine("- Reporting -");
            sb.AppendLine("2 - View habit report");
            sb.AppendLine("3 - View habit log report");
            sb.AppendLine();
            sb.AppendLine("- Management -");
            sb.AppendLine("4 - Add new habit");
            sb.AppendLine("5 - Activate habit");
            sb.AppendLine("6 - Deactivate habit");
            sb.AppendLine("7 - Update habit log entry");
            sb.AppendLine("8 - Delete habit log entry");
            sb.AppendLine();

            return sb.ToString();
        }
    }

    #endregion
    #region Methods: Internal

    internal void Show()
    {
        var status = PageStatus.Opened;

        while(status != PageStatus.Closed)
        {
            Console.Clear();

            WriteHeader(PageTitle);

            Console.Write(MenuText);

            var option = ConsoleHelper.GetInt("Enter your selection: ");
            status = PerformOption(option);
        }
    }

    #endregion
    #region Methods: Private

    private PageStatus PerformOption(int option)
    {
        var output = PageStatus.Opened;

        switch (option)
        {
            case 0:
            
                // Close application.
                output = PageStatus.Closed;
                break;

            case 1:

                // Record habit.
                RecordHabit();
                break;

            case 2:

                // View Habit report.
                ViewHabitReportPage();
                break;

            case 3:

                // View Habit Log report.
                ViewHabitLogReportPage();
                break;

            case 4:

                // Add new habit.
                NewHabit();
                break;

            case 5:

                // Activate habit.
                SetHabitIsActive(true);
                break;

            case 6:

                // Deactivate habit.
                SetHabitIsActive(false);
                break;

            case 7:

                // Update habit log entry.
                UpdateHabitLog();
                break;

            case 8:

                // Delete habit log entry.
                DeleteHabitLog();
                break;

            default:

                MessagePage.Show("Error", "Invalid option selected.");
                break;
        }

        return output;
    }

    private void NewHabit()
    {
        // Get required data.
        var habit = AddHabitPage.Show();
        if (habit == null)
        {
            return;
        }

        // Add to database.
        _habitTrackerService.AddHabit(habit.Name!, habit.Measure!);

        // Display output.
        MessagePage.Show("New Habit", $"Habit added successfully.");
    }

    private void RecordHabit()
    {
        // Get all active habits.
        var habits = _habitTrackerService.GetHabitsByIsActive(true);

        // There has to be a Habit added to record against.
        if (habits.Count < 1)
        {
            MessagePage.Show("Error", "No active habits.");
            return;
        }

        // Get habit to record against.
        var habit = HabitMenuPage.Show("Record", habits);
        if (habit == null)
        {
            return;
        }

        // Get required data.
        var habitLog = AddHabitLogPage.Show(habit);
        if (habitLog == null)
        {
            return;
        }

        // Add to database.
        _habitTrackerService.AddHabitLog(habit.Id, habitLog.Date, habitLog.Quantity);

        // Display output.
        MessagePage.Show("Record Habit", $"Habit recorded successfully.");
    }

    private void SetHabitIsActive(bool setToStatus)
    {
        // Get all habits NOT the SetToStatus.
        var habits = _habitTrackerService.GetHabitsByIsActive(!setToStatus);

        // There has to be a Habit returned.
        if (habits.Count < 1)
        {
            MessagePage.Show("Error", $"No {(setToStatus ? "inactive" : "active")} habits.");
            return;
        }

        var action = setToStatus ? "Activate" : "Deactivate";

        // Get habit to record against.
        var habit = HabitMenuPage.Show(action, habits);
        if (habit == null)
        {
            return;
        }

        // Update database.
        _habitTrackerService.SetHabitIsActive(habit.Id, setToStatus);

        // Display output.
        MessagePage.Show($"{action} Habit", $"Habit {(setToStatus ? "activated" : "deactivated")} successfully.");
    }

    private void UpdateHabitLog()
    {
        // Get all habits logs.
        var habitLogs = _habitTrackerService.GetHabitLogReport();

        // There has to be something returned.
        if (habitLogs.Count < 1)
        {
            MessagePage.Show("Error", $"No habit logs found.");
            return;
        }

        // Get object to record against.
        HabitLog? oldHabitLog = HabitLogMenuPage.Show("Update", habitLogs);
        if (oldHabitLog == null)
        {
            return;
        }

        // Get related object.
        Habit? habit = _habitTrackerService.GetHabit(oldHabitLog.HabitId);
        if (habit == null)
        {
            return;
        }

        // Get required data.
        HabitLog? newHabitLog = SetHabitLogPage.Show(habit, oldHabitLog);
        if (newHabitLog == null)
        {
            return;
        }

        // Update database.
        _habitTrackerService.SetHabitLog(oldHabitLog.Id, habit.Id, newHabitLog.Date, newHabitLog.Quantity);

        // Display output.
        MessagePage.Show("Update Habit Log Entry", $"Habit log entry updated successfully.");
    }

    private void DeleteHabitLog()
    {
        // Get all habits logs.
        var habitLogs = _habitTrackerService.GetHabitLogReport();

        // There has to be something returned.
        if (habitLogs.Count < 1)
        {
            MessagePage.Show("Error", $"No habit logs found.");
            return;
        }

        // Get object to record against.
        HabitLog? habitLog = HabitLogMenuPage.Show("Delete", habitLogs);
        if (habitLog == null)
        {
            return;
        }

        // Update database.
        _habitTrackerService.DeleteHabitLog(habitLog.Id);

        // Display output.
        MessagePage.Show("Delete Habit Log Entry", $"Habit log entry deleted successfully.");
    }

    private void ViewHabitLogReportPage()
    {
        // Additional specific report support:
        // Allow user to choose between a report for all habits or a specific one.

        // Get required data.
        var habits = _habitTrackerService.GetHabits();

        var reportConfig = ConfigureHabitLogReportPage.Show(habits);

        if (reportConfig == null)
        {
            // Go back to main menu.
            return;
        }

        // Configure table data.
        DataTable dataTable = new DataTable();
        if (reportConfig.DateFrom.HasValue && reportConfig.DateTo.HasValue)
        {
            // View quantity within date range.
            dataTable.Columns.Add("Habit");
            dataTable.Columns.Add("Quantity");
            dataTable.Columns.Add("Measure");

            if (reportConfig.HabitId.HasValue)
            {
                // Specific habit.
                var report = _habitTrackerService.GetHabitLogSumReportByHabitId(reportConfig.DateFrom.Value, reportConfig.DateTo.Value, reportConfig.HabitId.Value);
                foreach (var x in report)
                {
                    dataTable.Rows.Add([x.Name, x.Quantity, x.Measure]);
                }
            }
            else
            {
                // All habits.
                var report = _habitTrackerService.GetHabitLogSumReport(reportConfig.DateFrom.Value, reportConfig.DateTo.Value);
                foreach (var x in report)
                {
                    dataTable.Rows.Add([x.Name, x.Quantity, x.Measure]);
                }
            }
        }
        else
        {
            // View all dates.
            dataTable.Columns.Add("Date");
            dataTable.Columns.Add("Habit");
            dataTable.Columns.Add("Quantity");
            dataTable.Columns.Add("Measure");

            if (reportConfig.HabitId.HasValue)
            {
                // Specific habit.
                var report = _habitTrackerService.GetHabitLogReportByHabitId(reportConfig.HabitId.Value);
                foreach (var x in report)
                {
                    dataTable.Rows.Add([x.Date.ToShortDateString(), x.Name, x.Quantity, x.Measure]);
                }
            }
            else
            {
                // All habits.
                var report = _habitTrackerService.GetHabitLogReport();
                foreach (var x in report)
                {
                    dataTable.Rows.Add([x.Date.ToShortDateString(), x.Name, x.Quantity, x.Measure]);
                }
            }
        }
        
        // Configure console table.
        var consoleTable = ConsoleTableBuilder.From(dataTable);

        // Display report.
        MessagePage.Show("Habit Log Report", consoleTable.Export().ToString());
    }

    private void ViewHabitReportPage()
    {
        // Get raw data.
        var habitReport = _habitTrackerService.GetHabitReport();

        // Configure table data.
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Name");//, typeof(string));
        dataTable.Columns.Add("Measure");//, typeof(string));
        dataTable.Columns.Add("IsActive");//, typeof(bool));
        foreach (var x in habitReport)
        {
            dataTable.Rows.Add([x.Name, x.Measure, x.IsActive]);
        }

        // Configure console table.
        var consoleTable = ConsoleTableBuilder.From(dataTable);

        // Display report.
        MessagePage.Show("Habit Report", consoleTable.Export().ToString());
    }

    #endregion
}
