namespace DotNETConsole.HabitTracker.DataModels;


public class HabitLog
{
    public int Id { get; set; }
    public DateTime LogDate { get; set; }
    public int Quantity { get; set; }
    public int HabitId { get; set; }
}
