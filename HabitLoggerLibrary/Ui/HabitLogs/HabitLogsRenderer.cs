namespace HabitLoggerLibrary.Ui.HabitLogs;

public static class HabitLogsRenderer
{
    public static void Render(List<HabitLog> habitLogs)
    {
        foreach (var habitLog in habitLogs)
            Console.WriteLine(
                $"{habitLog.Id}; {habitLog.HabitName}; {habitLog.HabitUnitOfMeasure}: {habitLog.Quantity}; {habitLog.HabitDate}");
    }
}