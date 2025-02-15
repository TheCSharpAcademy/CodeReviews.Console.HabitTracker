namespace HabitTracker.kkvzx.database;

public class HabitModel(string date, int quantity)
{
    public string Date { get; } = date;
    public int Quantity { get; } = quantity;
}