namespace habitTracker.fatihskalemci.Models;

internal class HabitEntry
{
    public int Id { get; set; }
    public required string HabitName { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public required string Unit { get; set; }
}
