namespace HabitTrackerConsole.Models;

public class LogEntryViewModel
{
    public int RecordId { get; set; }
    public DateTime Date { get; set; }
    public string? HabitName { get; set; }
    public int Quantity { get; set; }
}
