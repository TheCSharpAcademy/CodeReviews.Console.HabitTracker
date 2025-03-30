namespace habit_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=habit-Tracker.db";
            var repository = new HabitTrackerRepository(connectionString);
            var app = new HabitTrackerApp(repository);

            app.CreateTables();
            app.GetUserInput();
        }
    }
}