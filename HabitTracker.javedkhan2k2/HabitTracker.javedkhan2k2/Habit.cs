namespace HabitTracker.Models;

internal class HabitLog
{
    public int Id { get; set; }
    public string? HabitDescription { get; set; } = default!;
    public string? HabitUnit { get; set; } = default!;
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}

internal class Habit
{
    public string? HabitDescription { get; set; } = default!;
    public int Id { get; set; }
    public string? Unit {get;set;}
}

internal class HabitReport
{
    public string? HabitDescription { get; set; } = default!;
    public string? Sum { get; set; }
    public string? Unit {get;set;}
}