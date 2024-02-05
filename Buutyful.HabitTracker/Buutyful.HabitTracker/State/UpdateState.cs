using Buutyful.HabitTracker.Abstractions;
using Buutyful.HabitTracker.Command;

namespace Buutyful.HabitTracker.State;

public class UpdateState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new ViewState(_stateManager));
    }

    public void Render()
    {
        bool toBeSelectedId = true;
        bool toBeSelectedName = true;
        int id = -1;
        string name = "";
        while (toBeSelectedId)
        {
            Console.WriteLine("Update Habit, instert the habit [id]:");
            if (int.TryParse(Console.ReadLine(), out var i)) id = i;
            Console.WriteLine($"Do you want to updated [{id}]? [y] / [n]");
            var answer = Console.ReadLine();
            if (answer?.ToLower() == "y")
            {
                toBeSelectedId = false;
            }
        }
        while (toBeSelectedName)
        {
            Console.WriteLine("Select new name for Habit, instert the habit [Name]:");
            var input = Console.ReadLine();
            Console.WriteLine($"Do you want to insert [{input}]? [y] / [n]");
            var answer = Console.ReadLine();
            if (answer?.ToLower() == "y")
            {
                name = input ?? "NO_NAME";
                toBeSelectedName = false;
            }
        }
        _stateManager.DbContext.UpdateHabit(id, name);
    }
}