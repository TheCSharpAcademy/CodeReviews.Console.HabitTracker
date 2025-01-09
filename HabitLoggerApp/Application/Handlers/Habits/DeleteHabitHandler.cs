using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;

namespace HabitLoggerApp.Application.Handlers.Habits;

public sealed class DeleteHabitHandler(
    IHabitChoiceReader habitChoiceReader,
    IHabitsRepository habitsRepository,
    IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        Console.Clear();
        var habits = habitsRepository.GetHabits();
        if (habits.Count == 0)
        {
            Console.WriteLine("No habits found. Press any key to continue....");
            keyAwaiter.Wait();
            return;
        }

        HabitsRenderer.Render(habits);
        Console.WriteLine("Choose habit to delete. All logs for it will also be deleted.");
        var recordId = habitChoiceReader.GetChoice();
        habitsRepository.DeleteHabitById(recordId);
        Console.WriteLine("Habit deleted. Press any key to continue...");
        keyAwaiter.Wait();
    }
}