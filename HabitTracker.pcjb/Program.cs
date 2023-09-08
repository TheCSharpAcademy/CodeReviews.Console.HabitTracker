namespace HabitTracker;

class Program
{
    private const string DatabaseFilename = "HabitTracker.db";

    static void Main(string[] args)
    {
        var database = new Database(DatabaseFilename);
        var habitLogController = new HabitLogController(database);
        var appState = AppState.MainMenu;
        
        if (!database.CreateDatabaseIfNotPresent())
        {
            Console.WriteLine($"Technical error: Could not create database {DatabaseFilename}. The error was logged.");
            Environment.Exit(1);
        }

        while (appState != AppState.Exit)
        {
            switch (appState)
            {
                case AppState.MainMenu:
                    appState = Screen.MainMenu();
                    break;
                case AppState.LogInsert:
                    appState = habitLogController.Create();
                    break;
                case AppState.LogViewList:
                    appState = habitLogController.List();
                    break;
                case AppState.LogViewOne:
                    appState = habitLogController.Read();
                    break;
                case AppState.LogEdit:
                    appState = habitLogController.Update();
                    break;
                case AppState.LogDelete:
                    appState = habitLogController.Delete();
                    break;
            }
        }
        Console.Clear();
        Console.WriteLine("Thank you for using HabitTracker. Goodbye.");
    }

}