namespace HabitTracker;

class Program
{
    private const string DatabaseFilename = "HabitTracker.db";

    static void Main(string[] args)
    {
        var habit = new Habit("Daily Walking", "steps");
        var database = new Database(DatabaseFilename);
        var appState = AppState.MainMenu;
        HabitLogRecord? selectedLogRecord = null;

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
                            Screen.Message($"New habit log entry saved.");
                        }
                        else
                        {
                            Screen.Message($"Technical Error: New habit log entry could not be saved. The error was logged.");
                        }
                        appState = AppState.MainMenu;
                    }
                    else
                    {
                        appState = AppState.MainMenu;
                    }
                    break;
                case AppState.LogViewList:
                    var habitlog = database.GetHabitLogRecords();
                    selectedLogRecord = Screen.LogViewList(habit, habitlog);
                    if (selectedLogRecord != null)
                    {
                        appState = AppState.LogViewOne;
                    }
                    else
                    {
                        appState = AppState.MainMenu;
                    }
                    break;
                case AppState.LogViewOne:
                    if (selectedLogRecord != null)
                    {
                        appState = Screen.LogViewOne(habit, selectedLogRecord);
                    }
                    else
                    {
                        appState = AppState.MainMenu;
                    }
                    break;
                case AppState.LogEdit:
                    if (selectedLogRecord != null)
                    {
                        var editedLogRecord = Screen.LogEdit(habit, selectedLogRecord);
                        if (database.UpdateHabitLogRecord(editedLogRecord))
                        {
                            Screen.Message($"Habit log entry {editedLogRecord.ID} updated.");
                        }
                        else
                        {
                            Screen.Message($"Technical Error: Habit log entry {editedLogRecord.ID} could not be updated. The error was logged.");
                        }
                    }
                    appState = AppState.LogViewList;
                    break;
                case AppState.LogDelete:
                    if (selectedLogRecord != null)
                    {
                        if (database.DeleteHabitLogRecord(selectedLogRecord.ID))
                        {
                            Screen.Message($"Habit log entry {selectedLogRecord.ID} deleted.");
                        }
                        else
                        {
                            Screen.Message($"Technical Error: Habit log entry {selectedLogRecord.ID} could not be deleted. The error was logged.");
                        }
                    }
                    appState = AppState.LogViewList;
                    break;
            }
        }
        Console.Clear();
        Console.WriteLine("Thank you for using HabitTracker. Goodbye.");
    }

}