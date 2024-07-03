// --------------------------------------------------------------------------------------------------
// HabitTracker.ConsoleApp.Views.HabitLogMenuPage
// --------------------------------------------------------------------------------------------------
// Displays a list of HabitLogs to a user and returns the users selection.
// --------------------------------------------------------------------------------------------------
using System.Text;
using ConsoleTableExt;
using HabitTracker.ConsoleApp.Enums;
using HabitTracker.ConsoleApp.Utilities;
using HabitTracker.Models;

namespace HabitTracker.ConsoleApp.Views;

internal class HabitLogMenuPage : BasePage
{
    #region Constants

    private const string PageTitle = "Habit Log Menu";

    #endregion
    #region Properties

    private static string MenuText
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine("Select an option...");
            sb.AppendLine();
            sb.AppendLine("0 - Back to main menu");
            return sb.ToString();
        }
    }

    #endregion
    #region Methods: Internal

    internal static HabitLog? Show(string action, List<HabitLogReport> habitLogs)
    {
        var status = PageStatus.Opened;

        HabitLog? output = null;

        while (status != PageStatus.Closed)
        {
            Console.Clear();

            WriteHeader($"{PageTitle} ({action})");

            WriteMenuText(habitLogs);
            
            var option = ConsoleHelper.GetInt("Enter your selection: ");

            switch (option)
            {
                case 0:
                    
                    // Go back to main menu.
                    status = PageStatus.Closed;
                    break;

                default:

                    if (option < 1 || option > habitLogs.Count)
                    {
                        MessagePage.Show("Error", "Invalid option selected.");
                    }
                    else
                    {
                        // NOTE: option is 1-based (list is 0-based)
                        output = new HabitLog(habitLogs[option - 1]);
                        status = PageStatus.Closed;
                    }
                    break;
            }
        }

        return output;
    }

    #endregion
    #region Methods: Private

    private static void WriteMenuText(List<HabitLogReport> habitLogs)
    {
        Console.Write(MenuText);

        WriteHabitLogSelections(habitLogs);

        Console.WriteLine();
    }

    private static void WriteHabitLogSelections(List<HabitLogReport> habitLogs)
    {
        // Configure table data.
        // NOTE: list<object> will create a headerless table which will append to the menu.
        var data = new List<List<object>>();
        for (int i = 0; i < habitLogs.Count; i++)
        {
            data.Add([$"{i + 1} -", habitLogs[i].Date.ToShortDateString(), habitLogs[i].Name, habitLogs[i].Quantity, habitLogs[i].Measure]);
        }

        // Configure & write console table.
        ConsoleTableBuilder.
            From(data).
            WithFormat(ConsoleTableBuilderFormat.Minimal).
            ExportAndWriteLine();
    }

    #endregion
}
