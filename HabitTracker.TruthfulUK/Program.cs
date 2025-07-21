using HabitTracker.TruthfulUK;
using Microsoft.Data.Sqlite;

if (File.Exists("HabitsTrackerLocalDB.db"))
{
    File.Delete("HabitsTrackerLocalDB.db");
}

DatabaseUtilities.InitializeDatabase();
Console.WriteLine("Database initialized.");

DatabaseUtilities.SeedDatabase();
Console.WriteLine("Database seeded with initial data.");

Console.ReadKey();
SqliteConnection.ClearAllPools();