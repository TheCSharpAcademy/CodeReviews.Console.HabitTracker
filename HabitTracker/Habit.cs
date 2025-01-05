namespace HabitTracker;

public class Habit
{
    public int Id { get; set; }
    public int HabitTrackerId { get; set; }
    public string Date {  get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
}
