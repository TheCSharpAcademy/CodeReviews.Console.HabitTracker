using HabitLogger.Database;

namespace HabitLogger;

internal static class Program
{
    internal static IDatabase _database = new SQLiteDB("Data Source=HabitLogger.db");

    static void Main()
    {
        var mainMenu = new MainMenu(_database);
        mainMenu.Show();
    }
}