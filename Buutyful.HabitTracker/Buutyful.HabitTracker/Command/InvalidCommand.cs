using Buutyful.HabitTracker.Abstractions;

namespace Buutyful.HabitTracker.Command;

public class InvalidCommand(string? command, string error) : ICommand
{    
    public void Execute()
    {
        Console.WriteLine($"Invalid Command : [{command}], {error}");            
    }
}