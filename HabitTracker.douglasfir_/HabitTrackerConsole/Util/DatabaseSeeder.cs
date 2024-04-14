using HabitTrackerConsole.Models;
using HabitTrackerConsole.Services;
using System.Collections.Generic;

namespace HabitTrackerConsole.Util;

/// <summary>
/// DatabaseSeeder contains methods to seed the database with initial data for habits and habit log entries.
/// It utilizes random data generation for dates and quantities to provide a variety of sample data.
/// </summary>
public class DatabaseSeeder
{
    private static Random _random = new Random();
    private static string[] habitNames = { "Exercise", "Reading", "Meditation", "Journaling", "Walking", "Programming" };

    /// <summary>
    /// Seeds the database with predefined habit names.
    /// </summary>
    /// <param name="habitService">Service used to interact with the habits table.</param>
    public static void SeedHabits(HabitService habitService)
    {
        foreach (string habitName in habitNames)
        {
            habitService.InsertHabitIntoHabitsTable(habitName);
        }
    }

    /// <summary>
    /// Seeds the database with log entries for existing habits, creating multiple logs per habit.
    /// </summary>
    /// <param name="logEntryService">Service used to interact with the habit logs table.</param>
    /// <param name="habitService">Service used to fetch habit data.</param>
    /// <param name="numOfLogs">Number of log entries to create per habit.</param>
    public static void SeedLogEntries(LogEntryService logEntryService, HabitService habitService, int numOfLogs)
    {
        List <HabitViewModel> habits = habitService.GetAllHabitsOverviews();
        if (!habits.Any()) return;  // Make sure there are habits to log against

        foreach (HabitViewModel habit in habits)
        {
            for (int i = 0; i < numOfLogs; i++)
            {
                int daysOffset = _random.Next(365);
                string date = DateTime.Today.AddDays(-daysOffset).ToString("yyyy-MM-dd");
                int quantity = _random.Next(1, 10);
                logEntryService.InsertLogEntryIntoHabitLog(date, habit.HabitId, quantity);
            }
        }
    }
}
