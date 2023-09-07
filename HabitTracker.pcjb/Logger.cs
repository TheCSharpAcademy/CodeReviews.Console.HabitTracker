namespace HabitTracker;

class Logger
{
    static string logFileName = "HabitTracker.log";

    public static void Error(Exception ex)
    {
        var timestamp = DateTime.Now.ToString("u");
        using StreamWriter sw = File.AppendText(logFileName);
        sw.WriteLine($"{timestamp} ERROR {ex.Message}");
        sw.WriteLine(ex.StackTrace);
    }
}