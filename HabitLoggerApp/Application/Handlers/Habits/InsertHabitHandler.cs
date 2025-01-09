using HabitLoggerLibrary;
using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Input;

namespace HabitLoggerApp.Application.Handlers.Habits;

public sealed class InsertHabitHandler(
    IInputReaderSelector inputReaderSelector,
    IHabitsRepository repository,
    IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        var inputReader = inputReaderSelector.GetInputReader();
        Console.Clear();
        Console.WriteLine("Provide habit name");
        var habitName = inputReader.GetStringInput();

        Console.Clear();
        Console.WriteLine("Provide unit of measure");
        var unitOfMeasure = inputReader.GetStringInput();

        var habit = repository.AddHabit(new HabitDraft(habitName, unitOfMeasure));

        Console.Clear();
        Console.WriteLine($"Habit added: {habit.HabitName}; {habit.UnitOfMeasure};");
        Console.WriteLine("Press any key to continue...");
        keyAwaiter.Wait();
    }
}