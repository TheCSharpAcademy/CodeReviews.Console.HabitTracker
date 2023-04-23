namespace HabitTracker.barakisbrown;

public class Habit
{
    public readonly string HabitName = "BloodSugar-Readings";
    public int id { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }

    public override string ToString()
    {
        return $"Id = {id}, Date = {Date.ToShortDateString()}, Amount = {Amount}";
    }
}
