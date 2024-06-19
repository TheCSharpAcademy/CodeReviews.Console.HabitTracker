namespace HabitTracker
{
    class Program
    {
        private static string connectionString = "Data Source=habit_tracker.db";

        static void Main(string[] args)
        {
            HabitTrackerApp app = new HabitTrackerApp(connectionString);
            app.Run();
        }
    }
}
