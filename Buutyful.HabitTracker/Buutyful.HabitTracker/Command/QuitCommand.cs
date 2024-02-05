using Buutyful.HabitTracker.Abstractions;

namespace Buutyful.HabitTracker.Command;

internal class QuitCommand : ICommand
{
    public void Execute()
    {
        Environment.Exit(0);
    }
}
