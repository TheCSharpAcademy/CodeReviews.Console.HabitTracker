using Buutyful.HabitTracker.Abstractions;
using Buutyful.HabitTracker.Command;

namespace Buutyful.HabitTracker.State;

public class ViewState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    public ICommand GetCommand()
    {
       return new SwitchStateCommand(_stateManager, new MainMenuState(_stateManager));
    }

    public void Render()
    {
        Console.WriteLine("=====LOADING HABITS=====");
        _stateManager.DbContext.DisplayHabits();
        Console.WriteLine("========================");
    }
}
