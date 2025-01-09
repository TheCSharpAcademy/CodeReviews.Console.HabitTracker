using HabitLoggerLibrary.Repository;
using HabitLoggerLibrary.Ui;
using HabitLoggerLibrary.Ui.Habits;

namespace HabitLoggerApp.Application.Handlers.Habits;

public sealed class ViewHabitsHandler(IHabitsRepository habitsRepository, IKeyAwaiter keyAwaiter)
{
    public void Handle()
    {
        Console.Clear();
        HabitsRenderer.Render(habitsRepository.GetHabits());
        Console.WriteLine("Press any key to continue...");
        keyAwaiter.Wait();
    }
}