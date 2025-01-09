namespace HabitLoggerLibrary.Ui.Menu;

public static class HabitLogsMenuRenderer
{
    public static void Render()
    {
        Console.WriteLine($@"{Convert.ToChar(HabitLogsMenuChoice.ViewAllLogs)}: View all habit logs
{Convert.ToChar(HabitLogsMenuChoice.InsertLogEntry)}: Insert new habit log
{Convert.ToChar(HabitLogsMenuChoice.DeleteLogEntry)}: Delete habit log
{Convert.ToChar(HabitLogsMenuChoice.EditLogEntry)}: Edit habit
{Convert.ToChar(HabitLogsMenuChoice.GoBack)}: Back");
    }
}