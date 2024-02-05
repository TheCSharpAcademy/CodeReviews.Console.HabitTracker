namespace Buutyful.HabitTracker.Abstractions;

public interface IState
{
    void Render();
    ICommand GetCommand();
}
