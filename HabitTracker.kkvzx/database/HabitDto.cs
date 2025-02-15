namespace HabitTracker.kkvzx.database;

public class HabitDto(int id, DateTime date, int quantity)
{
    public int Id { get; } = id;
    public DateTime Date { get; } = date;
    public int Quantity { get; } = quantity;
}