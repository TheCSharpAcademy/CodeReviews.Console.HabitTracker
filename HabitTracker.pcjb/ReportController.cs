namespace HabitTracker;

class ReportController
{
    private Database database;

    public ReportController(Database database)
    {
        this.database = database;
    }

    public AppState FrequencyAndTotalPerMonth(Habit? habit)
    {
        if (habit == null) {
            Screen.Message("Please select a habit before requesting this report.");
            return AppState.MainMenu;
        }

        var reportData = database.GetFrequencyAndTotalsPerMonth(habit.ID);
        Screen.ReportFrequencyAndTotalPerMonth(habit, reportData);

        return AppState.MainMenu;
    }
}