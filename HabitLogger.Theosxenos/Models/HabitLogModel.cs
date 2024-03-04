namespace HabitLogger.Models;

public class HabitLogModel
{
    public int LogId { get; set; }
    public int HabitId { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
}