using HabitTracker.TruthfulUK;
using HabitTracker.TruthfulUK.Helpers;


class HabitTrackerApp
{
    static void Main(string[] args)
    {
        if (!File.Exists("HabitsTrackerLocalDB.db"))
        {
            DB_Helpers.InitializeDatabase();

            if (args.Contains("debug"))
                DB_Helpers.SeedDatabase();
        }
        UserInterface.DisplayMainMenu();
    }
}