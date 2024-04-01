using STUDY.ConsoleApp.HabitLogger;
internal class Program
{
    public static string connectionString = @"Data Source=habit-Tracker.db";
    private static void Main(string[] args)
    {
        Helpers.CreateDB(connectionString);
        Menu.GetUserInput();

    }
}