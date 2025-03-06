using Erix101.HabitTracker.Services;
using HabitTracker.DataHelpers;

internal class Program
{

    static string connectionString = @"Data Source=habit-Tracker.db";
    private static void Main(string[] args)
    {
        MenuService.DisplayHeading();

        SQLite connection = new SQLite(connectionString);
        connection.SetUpDataBase(connectionString);
        connection.AddExampleDataIfEmpty(connectionString);

        MenuService menu = new MenuService(connection);
        menu.OpenMainMenu();
    }
}