namespace HabitTracker.TomDonegan
{
    public static class HabitTrackerApp
    {
        public static void Run()
        {
            DatabaseAccess.DatabaseCreation("", "");
            UserInterface.MainMenu();
        }
    }
}
