namespace HabitTracker;

class HabitLogController
{
    private readonly Database database;
    private readonly Habit habit = new("Daily Walking", "steps");
    private HabitLogRecord? selectedLogRecord = null;

    public HabitLogController(Database database)
    {
        this.database = database;
    }

    public AppState Create()
    {
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
        }
        return AppState.MainMenu;
    }

    public AppState List()
    {
        var habitlog = database.GetHabitLogRecords();
        selectedLogRecord = Screen.LogViewList(habit, habitlog);
        if (selectedLogRecord != null)
        {
            return AppState.LogViewOne;
        }
        else
        {
            return AppState.MainMenu;
        }
    }

    public AppState Read()
    {
        if (selectedLogRecord != null)
        {
            return Screen.LogViewOne(habit, selectedLogRecord);
        }
        else
        {
            return AppState.MainMenu;
        }
    }

    public AppState Update()
    {
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
        return AppState.LogViewList;
    }

    public AppState Delete()
    {
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
        return AppState.LogViewList;
    }
}