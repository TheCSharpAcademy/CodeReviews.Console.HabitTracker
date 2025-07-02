namespace DotNETConsole.HabitTracker.DataModels;


public class Habit
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public string Unit { get; set; }

    public override string ToString()
    {
        return Title;
    }
}
