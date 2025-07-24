using HabitTracker.TruthfulUK;
using HabitTracker.TruthfulUK.Helpers;


class HabitTrackerApp
{
    static void Main(string[] args)
    {
        if (!File.Exists("HabitsTrackerLocalDB.db"))
        {
            DbHelpers.InitializeDatabase();

            if (args.Contains("debug"))
                DbHelpers.SeedDatabase();
        }
        UserInterface.DisplayMainMenu();
    }
}