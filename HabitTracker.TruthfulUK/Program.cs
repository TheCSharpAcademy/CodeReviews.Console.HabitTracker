using HabitTracker.TruthfulUK;
using HabitTracker.TruthfulUK.Helpers;

// [Debug] Delete old database file if it exists
if (File.Exists("HabitsTrackerLocalDB.db"))
{
    File.Delete("HabitsTrackerLocalDB.db");
}

// [Debug] Initialize new database and seed it
if (!File.Exists("HabitsTrackerLocalDB.db"))
{
    DB_Helpers.InitializeDatabase();
    DB_Helpers.SeedDatabase();
}

UserInterface.DisplayMainMenu();