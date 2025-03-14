namespace Tomi.HabitTracker.Data;

public struct HabitCompactGist
{
    public required int Id { get; set; }
    public required string Habit { get; set; }
    public required double Quantity { get; set; }

    public required DateTime HabitDate { get; set; }

    public readonly void PrintHabitGist()
    {
        Console.WriteLine($"{Id.ToString().PadRight(10)} | {Habit.PadRight(20)} | {Quantity.ToString("F1").PadRight(10)} | {HabitDate.ToString("yyyy-MM-dd").PadRight(15)}");
    }
}