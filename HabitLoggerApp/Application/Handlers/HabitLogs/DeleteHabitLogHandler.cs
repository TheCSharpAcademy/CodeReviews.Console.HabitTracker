using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.HabitLogs;

namespace HabitLoggerApp.Application.Handlers.HabitLogs;

public sealed class DeleteHabitLogHandler(
    IHabitLogsRepository repository,
    IHabitLogChoiceReader habitLogChoiceReader,
    IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        Console.Clear();
        var logs = repository.GetHabitLogs();
        if (logs.Count == 0)
        {
            Console.WriteLine("No habit logs found. Press any key to continue...");
            keyAwaiter.Wait();
            return;
        }

        HabitLogsRenderer.Render(logs);
        Console.WriteLine("Choose habit log you want to delete.");
        var habitLogId = habitLogChoiceReader.GetChoice();
        repository.DeleteHabitLogById(habitLogId);
        Console.WriteLine("Habit log deleted. Press any key to continue...");
        keyAwaiter.Wait();
    }
}