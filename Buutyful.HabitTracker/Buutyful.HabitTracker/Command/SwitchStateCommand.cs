using Buutyful.HabitTracker.Abstractions;
using Buutyful.HabitTracker.State;

namespace Buutyful.HabitTracker.Command;

public class SwitchStateCommand(StateManager manager, IState newState) : ICommand
{
    private readonly StateManager _manager = manager;
    private readonly IState _newState = newState;

    public void Execute()
    {
        _manager.SwitchState(_newState);
    }
}