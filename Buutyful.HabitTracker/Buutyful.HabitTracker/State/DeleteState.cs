using Buutyful.HabitTracker.Abstractions;
using Buutyful.HabitTracker.Command;

namespace Buutyful.HabitTracker.State;

public class DeleteState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new ViewState(_stateManager));
    }

    public void Render()
    {
        bool toBeSelected = true;
        int id = -1;
        while (toBeSelected)
        {
            Console.WriteLine("Delete Habit, instert the habit [id]:");
            if(int.TryParse(Console.ReadLine(), out var i)) id = i;
            Console.WriteLine($"Do you want to delete [{id}]? [y] / [n]");
            var answer = Console.ReadLine();
            if (answer?.ToLower() == "break") return;
            if (answer?.ToLower() == "y")
            {               
                toBeSelected = false;
            }
        }
        _stateManager.DbContext.DeleteHabit(id);
    }
}
