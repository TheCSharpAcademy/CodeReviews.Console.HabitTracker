using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;
using HabitLoggerLibrary.Ui.Input;

namespace HabitLoggerApp.Application.Handlers.Habits;

public sealed class EditHabitHandler(
    IHabitsRepository repository,
    IKeyAwaiter keyAwaiter,
    IHabitChoiceReader habitChoiceReader,
    IInputReaderSelector inputReaderSelector)
{
    public void Handle()
    {
        Console.Clear();

        var habits = repository.GetHabits();
        if (habits.Count == 0)
        {
            Console.WriteLine("No habits found. Press any key to continue.");
            keyAwaiter.Wait();
            return;
        }

        HabitsRenderer.Render(habits);
        Console.WriteLine("Choose habit to edit.");
        var habit = repository.GetHabitById(habitChoiceReader.GetChoice());

        var inputReader = inputReaderSelector.GetInputReader();

        Console.Clear();
        Console.WriteLine($"Editing habit: {habit.HabitName}; {habit.UnitOfMeasure}");

        Console.WriteLine("Provide new habit name:");
        var newHabitName = inputReader.GetStringInput();

        Console.WriteLine("Provide new unit of measure:");
        var newUnitOfMeasure = inputReader.GetStringInput();

        repository.UpdateHabit(habit with { HabitName = newHabitName, UnitOfMeasure = newUnitOfMeasure });

        Console.WriteLine("Habit updated. Press any key to continue...");

        keyAwaiter.Wait();
    }
}