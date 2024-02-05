using Buutyful.HabitTracker.Abstractions;

namespace Buutyful.HabitTracker.Command;

public class ClearCommand : ICommand
{
    public void Execute()
    {
        Console.Clear();
    }
}
