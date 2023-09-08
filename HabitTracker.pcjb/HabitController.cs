namespace HabitTracker;

class HabitController
{
    private readonly Database database;

    public HabitController(Database database)
    {
        this.database = database;
    }

    public AppState Create()
    {
        var newHabit = Screen.HabitInsert();
        if (newHabit != null)
        {
            if (database.AddHabit(newHabit))
            {
                Screen.Message($"New habit saved.");
            }
            else
            {
                Screen.Message($"Technical Error: New habit could not be saved. The error was logged.");
            }
        }
        return AppState.MainMenu;
    }

    public Habit? Select()
    {
        var habits = database.GetHabits();
        return Screen.HabitSelect(habits);
    }
}