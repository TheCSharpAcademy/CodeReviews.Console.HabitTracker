namespace HabitTracker;

class Program
{
    private const string DatabaseFilename = "HabitTracker.db";

    static void Main(string[] args)
    {
        var habit = new Habit("Daily Walking", "steps");
        var database = new Database(DatabaseFilename);
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
                    var newHabitLogRecord = Screen.LogInsert(habit);
                    if (newHabitLogRecord.Quantity > 0)
                    {
                        if (database.AddHabitLogRecord(newHabitLogRecord))
                        {
                            Screen.LogInsertOK();
                        }
                        else
                        {
                            Screen.LogInsertError();
                        }
                        appState = AppState.MainMenu;
                    }
                    else
                    {
                        appState = AppState.MainMenu;
                    }
                    break;
                case AppState.LogView:
                    var habitlog = database.GetHabitLogRecords();
                    Screen.LogView(habit, habitlog);
                    appState = AppState.MainMenu;
                    break;
            }
        } 
        Console.Clear();
        Console.WriteLine("Thank you for using HabitTracker. Goodbye.");
    }

}