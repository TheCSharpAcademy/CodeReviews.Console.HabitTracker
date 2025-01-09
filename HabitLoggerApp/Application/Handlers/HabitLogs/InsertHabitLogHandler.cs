using HabitLoggerLibrary;
using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;
using HabitLoggerLibrary.Ui.Input;

namespace HabitLoggerApp.Application.Handlers.HabitLogs;

public sealed class InsertHabitLogHandler(
    IInputReaderSelector inputReaderSelector,
    IHabitChoiceReader habitChoiceReader,
    IHabitsRepository habitsRepository,
    IHabitLogsRepository habitLogsRepository,
    IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        Console.Clear();
        HabitsRenderer.Render(habitsRepository.GetHabits());
        Console.WriteLine("Choose habit you want to insert log for");
        var habitId = habitChoiceReader.GetChoice();
        var inputReader = inputReaderSelector.GetInputReader();

        var habit = habitsRepository.GetHabitById(habitId);
        Console.Clear();
        Console.WriteLine($"Insert habit log for habit '{habit.HabitName}' ({habit.UnitOfMeasure})");

        Console.WriteLine("Provide date");
        var habitDate = inputReader.GetDateInput();

        Console.WriteLine("Provide quantity");
        var habitQuantity = inputReader.GetNumberInput();

        var habitLog =
            habitLogsRepository.AddHabitLog(new HabitLogDraft(habitId, Convert.ToInt32(habitQuantity), habitDate));
        Console.WriteLine($"Habit log added: {habitLog.HabitName}; {habitLog.HabitDate}; {habitLog.Quantity}");
        Console.WriteLine("Press any key to continue...");
        keyAwaiter.Wait();
    }
}