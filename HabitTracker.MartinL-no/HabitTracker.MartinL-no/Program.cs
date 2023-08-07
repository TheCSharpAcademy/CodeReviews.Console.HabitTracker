namespace HabitTracker.MartinL_no;

internal class Program
{
    internal static void Main(string[] args)
    {
        var repo = new HabitRepository();
        var service = new HabitService(repo);
        var application = new HabitTrackerApplication(service);

        application.Execute();
    }
}