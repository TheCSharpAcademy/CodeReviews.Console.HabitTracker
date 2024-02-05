using Buutyful.HabitTracker.Abstractions;
using Buutyful.HabitTracker.Command;

namespace Buutyful.HabitTracker.State;

public class MainMenuState(StateManager stateManager) : IState
{
    private readonly StateManager _manager = stateManager;

    public ICommand GetCommand()
    {
        var command = Console.ReadLine()?.ToLower();
        return MenuSelector(command);
    }

    public void Render()
    {
        Console.WriteLine("Track your habits. Select [info] for navigation help");

    }
    private ICommand MenuSelector(string? command)
    {
        return command switch
        {
            "info" => new InfoCommand(),
            "view" => new SwitchStateCommand(_manager, new ViewState(_manager)),
            "create" => new SwitchStateCommand(_manager, new CreateState(_manager)),
            "back" => new SwitchStateCommand(_manager, _manager.PastState()),
            "forward" => new SwitchStateCommand(_manager, _manager.FutureState()),
            "clear" => new ClearCommand(),
            "quit" => new QuitCommand(),
            _ => new InvalidCommand(command),
        };
    }
}
