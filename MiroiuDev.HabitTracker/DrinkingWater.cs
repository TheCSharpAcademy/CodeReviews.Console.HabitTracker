namespace MiroiuDev.HabitTracker;

internal class DrinkingWater
{
    internal int Id { get; set; }
    internal DateTime Date { get; set; }
    internal int Quantity { get; set; }
    public override string ToString()
    {
        return $"{Id} - {Date:dd-MMM-yyyy} - Quantity: {Quantity}";
    }
}