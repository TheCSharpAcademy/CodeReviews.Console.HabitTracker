namespace HabitTracker.mxrt0;

public class LearningJS
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    public override string ToString()
    {
        return $"Id: {this.Id}, Date: {this.Date.ToShortDateString()} , Quantity: {this.Quantity}";
    }
}