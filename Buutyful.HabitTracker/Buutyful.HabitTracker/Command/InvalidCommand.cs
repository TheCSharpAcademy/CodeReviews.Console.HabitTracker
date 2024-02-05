using Buutyful.HabitTracker.Abstractions;

namespace Buutyful.HabitTracker.Command;

public class InvalidCommand(string? command) : ICommand
{
    private readonly string _command = command ??= string.Empty;
    public void Execute()
    {
        Console.WriteLine($"Invalid Command : {_command}");            
    }
}