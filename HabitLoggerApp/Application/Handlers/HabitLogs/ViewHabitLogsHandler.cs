using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.HabitLogs;

namespace HabitLoggerApp.Application.Handlers.HabitLogs;

public sealed class ViewHabitLogsHandler(IHabitLogsRepository repository, IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        Console.Clear();
        var logs = repository.GetHabitLogs();
        HabitLogsRenderer.Render(logs);
        Console.WriteLine("Press any key to continue...");
        keyAwaiter.Wait();
    }
}