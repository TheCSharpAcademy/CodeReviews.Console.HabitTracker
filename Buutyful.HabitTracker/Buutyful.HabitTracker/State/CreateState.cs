using Buutyful.HabitTracker.Abstractions;
using Buutyful.HabitTracker.Command;

namespace Buutyful.HabitTracker.State
{
    public class CreateState(StateManager manager) : IState
    {
        private readonly StateManager _stateManager = manager;
        public ICommand GetCommand()
        {
            return new SwitchStateCommand(_stateManager, new ViewState(_stateManager));
        }

        public void Render()
        {
            bool toBeSelected = true;
            string habit = "";
            while (toBeSelected)
            {
                Console.WriteLine("Create Habit, instert the habit [Name]:");
                var name = Console.ReadLine();
                Console.WriteLine($"Do you want to insert [{name}]? [y] / [n]");
                var answer = Console.ReadLine();
                if (answer?.ToLower() == "y")
                {
                    habit = name ?? "NO_NAME";
                    toBeSelected = false;
                }
            }
            _stateManager.DbContext.InsertHabit(habit);
        }
    }
}
