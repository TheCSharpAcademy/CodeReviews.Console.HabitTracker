namespace iamryanmacdonald.Console.HabitTracker.Models;

public class HabitEntry
{
    internal DateOnly Date { get; set; }
    internal int HabitId { get; set; }
    internal int Id { get; set; }
    internal int Quantity { get; set; }
}