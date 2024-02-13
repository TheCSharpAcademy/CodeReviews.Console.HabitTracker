using HabitLogger.enums;
using HabitLogger.logic.utils;
using HabitLogger.view;

namespace HabitLogger;

/// <summary>
/// The entry point class for the HabitLogger program.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    private static void Main(string[] args)
    {
        const string connectionString = "Data Source=habit-Tracker.db";

        Start(connectionString);
    }

    private static void Start(string connectionString)
    {
        var logger = new logic.HabitLogger(connectionString);
        var isRunning = true;
        var mainMenu = new Dictionary<string, Action>
        {
            { "Add Habit", logger.AddHabit },
            { "Delete Habit", logger.DeleteHabit },
            { "Update Habit", logger.UpdateHabit },
            { "Create Habit Report", () => GetReport(logger) },
            { "Add Record", logger.AddRecord },
            { "Delete Record", logger.DeleteRecord },
            { "View Records", logger.GetRecords },
            { "Update Record", logger.UpdateRecord },
            { "Help", MenuView.ShowHelp},
            { "Quit", () => throw new Utilities.ExitFromAppException() }
        };

        MenuView.ShowHelp();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        
        while (isRunning)
        {
            try
            {
                var userChoice = MenuView.MainMenu((mainMenu.Keys).ToArray());
                mainMenu[userChoice].Invoke();

                if (userChoice.Equals("Create Habit Report"))
                {
                    continue;
                }
                
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Utilities.ExitFromAppException e)
            {
                Console.WriteLine(e.Message + " \nGoodbye!");
                isRunning = false;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Invalid choice. Please select one of the above.");
            }
        }
    }

    private static void GetReport(logic.HabitLogger logger)
    {
        var isRunning = true;
        var reportsMenu = new Dictionary<string, ReportType>
            {
                { "From a specific date to Today", ReportType.DateToToday},
                {"From a specific date to another specific date", ReportType.DateToDate },
                { "View total of a given month", ReportType.TotalForMonth },
                { "Year to date", ReportType.YearToDate },
                { "View total for a specific year", ReportType.TotalForYear },
                { "View all records", ReportType.Total },
                { "Return to main menu", ReportType.ReturnToMainMenu }
        };

        Console.Clear();

        logger.GetHabits();
            
        int id = Utilities.ValidateNumber("Enter the ID of the habit:");
        
        while (isRunning)
        {
            Console.Clear();
            
            try
            {
                var userChoice = MenuView.ReportsMenu(reportsMenu.Keys.ToArray());
                var reportType = reportsMenu[userChoice];
                
                if (reportType == ReportType.ReturnToMainMenu)
                {
                    isRunning = false;
                }
                else
                {
                    logger.GenerateHabitReport(reportType, id);
                }
            }
            catch (Utilities.ExitToMainException e)
            {
                Console.WriteLine(e.Message);
                isRunning = false;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Invalid choice. Please select one of the above.");
            }
        }
    }
}