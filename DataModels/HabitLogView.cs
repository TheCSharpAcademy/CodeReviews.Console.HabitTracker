namespace DotNETConsole.HabitTracker.DataModels;

public class HabitLogView
{
    public int LogId { get; set; }
    public DateTime EntryDate { get; set; }
    public int Quantity { get; set; }
    public string HabitTitle { get; set; }

    public override string ToString()
    {
        return $"{HabitTitle}({DateOnly.FromDateTime(EntryDate)}) - {Quantity}";
    }
}
