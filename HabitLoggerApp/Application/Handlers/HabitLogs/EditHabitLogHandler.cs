using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.HabitLogs;
using HabitLoggerLibrary.Ui.Input;

namespace HabitLoggerApp.Application.Handlers.HabitLogs;

public class EditHabitLogHandler(
    IHabitLogsRepository repository,
    IHabitLogChoiceReader habitLogChoiceReader,
    IInputReaderSelector inputReaderSelector,
    IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        Console.Clear();
        HabitLogsRenderer.Render(repository.GetHabitLogs());

        Console.WriteLine("Choose log you want to edit:");
        var habitLogId = habitLogChoiceReader.GetChoice();
        var inputReader = inputReaderSelector.GetInputReader();

        Console.Clear();

        var habitLog = repository.GetHabitLogById(habitLogId);
        Console.WriteLine(
            $"Editing habit '{habitLog.HabitName}' in '{habitLog.HabitUnitOfMeasure}': {habitLog.HabitDate}; {habitLog.Quantity}");

        Console.WriteLine("Provide new date");
        var habitDate = inputReader.GetDateInput();

        Console.WriteLine("Provide new quantity");
        var habitQuantity = inputReader.GetNumberInput();

        repository.UpdateHabitLog(habitLog with { HabitDate = habitDate, Quantity = Convert.ToInt32(habitQuantity) });

        Console.WriteLine("Habit log updated. Press any key to continue...");
        keyAwaiter.Wait();
    }
}