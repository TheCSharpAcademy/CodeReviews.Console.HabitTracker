using MapDataReader;

namespace HabitTracker.edvaudin;

[GenerateDataReaderMapper]
public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Measurement { get; set; } = string.Empty;

    public string GetString() => $"[#{Id}] {Name} (Measured in {Measurement})";
}
