namespace majeed_yasss.HabitTracker;
enum Option { None, CreateLog, ViewLogs, UpdateLog, DeleteLog }
internal class Options
{
    public static readonly Dictionary<string, Option> Labels =
        new()
        {
            { "Create a new log", Option.CreateLog },
            { "View existing logs", Option.ViewLogs },
            { "Update an existing log", Option.UpdateLog },
            { "Delete a log", Option.DeleteLog },
            { "Exit", Option.None }
        };
}
