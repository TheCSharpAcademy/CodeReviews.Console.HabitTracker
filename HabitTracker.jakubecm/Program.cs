namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbFilename = "habit-tracker.db";
            bool requiresClosing = false;
            bool databaseExists = File.Exists(dbFilename);

            Interface appInterface = new Interface();
            DatabaseController dbController = new($@"Data Source={dbFilename}");

            dbController.InitializeDatabase(databaseExists);

            while (!requiresClosing)
            {
                Console.Clear();
                appInterface.PresentMenu();
                var selectedOption = Interface.ParseSelection();
                requiresClosing = appInterface.ExecuteSelected(selectedOption, dbController);
            }
        }
    }


}
