namespace HabitTracker.mxrt0;

public class LearningJS
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    public override string ToString()
    {
        return $"Id: {this.Id}, Date: {new string(this.Date.ToShortDateString().TakeWhile(c => c != ' ').ToArray())} , Quantity: {this.Quantity}";
    }
}