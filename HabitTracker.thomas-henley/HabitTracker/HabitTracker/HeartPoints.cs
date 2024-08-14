namespace Habits;

internal class HeartPoints
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    public string Display()
    {
        return $"{Id}. {Date.ToString("dd-MM-yy")}, {Quantity} points.";
    }
}
