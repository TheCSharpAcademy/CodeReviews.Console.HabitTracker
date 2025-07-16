namespace majeed_yasss.HabitTracker;
public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    override public string ToString() 
    {
        return $"[{Id}] - Date: {Date} - Quantity:{Quantity}";
    }
}
