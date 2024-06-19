namespace HabitTracker
{
    class Program
    {
        private static readonly string connectionString = "Data Source=habit_tracker.db";

        static void Main()
        {
            HabitTrackerApp app = new(connectionString);
            app.Run();
        }
    }
}
