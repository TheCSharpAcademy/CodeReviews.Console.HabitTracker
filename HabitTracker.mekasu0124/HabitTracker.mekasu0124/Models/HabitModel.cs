namespace HabitTracker.Models;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int Count { get; set; }
    public string Description { get; set; }
}