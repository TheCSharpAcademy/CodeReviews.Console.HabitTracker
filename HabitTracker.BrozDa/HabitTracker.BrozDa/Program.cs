namespace HabitTracker.BrozDa
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dateFormat = "dd-MM-yyyy";
            string databasePath = "habit-tracker.sqlite";
            string connectionString = @$"Data Source={databasePath};Version=3;";

            HabitRepository habitRepository = new HabitRepository(connectionString);
            HabitRecordRepository habitRecordRepository = new HabitRecordRepository(connectionString);
            InputManager inputManager = new InputManager(dateFormat);
            OutputManager outputManager = new OutputManager(dateFormat);

            HabitTrackerApp habitTrackerApp = new HabitTrackerApp(databasePath, habitRepository, habitRecordRepository, inputManager, outputManager);

            habitTrackerApp.Run();

        }
    }
}
