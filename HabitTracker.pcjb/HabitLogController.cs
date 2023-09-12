namespace HabitTracker;

class HabitLogController
{
    private readonly Database database;
    private HabitLogRecord? selectedLogRecord = null;

    public HabitLogController(Database database)
    {
        this.database = database;
    }

    public AppState Create(Habit? habit)
    {
        if (habit == null) {
            Screen.Message("Please select a habit before adding log entries.");
            return AppState.MainMenu;
        }

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

    public AppState List(Habit? habit)
    {
        if (habit == null) {
            Screen.Message("Please select a habit before viewing log entries.");
            return AppState.MainMenu;
        }

        var habitlog = database.GetHabitLogRecords(habit.ID);
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
        if (selectedLogRecord == null) {
            Screen.Message("No log entry selected.");
            return AppState.LogViewList;
        }

        var habit = database.GetHabit(selectedLogRecord.HabitID);
        if (habit == null) {
            Screen.Message("No matching habit for log entry.");
            return AppState.LogViewList;
        }
        return Screen.LogViewOne(habit, selectedLogRecord);
    }

    public AppState Update()
    {
        if (selectedLogRecord == null) {
            Screen.Message("No log entry selected.");
            return AppState.LogViewList;
        }

        var habit = database.GetHabit(selectedLogRecord.HabitID);
        if (habit == null) {
            Screen.Message("No matching habit for log entry.");
            return AppState.LogViewList;
        }

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
        if (selectedLogRecord == null) {
            Screen.Message("No log entry selected.");
            return AppState.LogViewList;
        }

        if (database.DeleteHabitLogRecord(selectedLogRecord.ID))
        {
            Screen.Message($"Habit log entry {selectedLogRecord.ID} deleted.");
        }
        else
        {
            Screen.Message($"Technical Error: Habit log entry {selectedLogRecord.ID} could not be deleted. The error was logged.");
        }
        return AppState.LogViewList;
    }
}