namespace HabitLoggerLibrary.Ui.Menu;

public static class MainMenuRenderer
{
    public static void Render()
    {
        Console.WriteLine($@"{Convert.ToChar(MainMenuChoice.ViewAllHabits)}: View all habits
{Convert.ToChar(MainMenuChoice.InsertHabit)}: Insert habit
{Convert.ToChar(MainMenuChoice.DeleteHAbit)}: Delete habit
{Convert.ToChar(MainMenuChoice.EditHabit)}: Edit habit
{Convert.ToChar(MainMenuChoice.HabitLogs)}: Habit logs
{Convert.ToChar(MainMenuChoice.Statistics)}: Show statistics
{Convert.ToChar(MainMenuChoice.Quit)}: Quit");
    }
}