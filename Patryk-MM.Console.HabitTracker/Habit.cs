namespace Patryk_MM.Console.HabitTracker;
public class Habit {
    public int Id { get; set; }
    public string Date { get; set; }
    public double Quantity { get; set; }

    public override string ToString() {
        return $"Id: {Id}, Date: {Date}, Quantity: {Quantity}";
    }

}

