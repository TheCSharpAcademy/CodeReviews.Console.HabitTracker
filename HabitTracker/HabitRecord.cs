namespace HabitTracker;

public class HabitRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public int HabitId { get; set; } // Foreign key

    // Properties to hold joined data from Habits table
    public string? HabitName { get; set; }
    public string? UnitOfMeasurement { get; set; }

    public override string ToString() // Updated for better display
    {
        return $"Log Id: {Id}, Habit: {HabitName ?? "N/A"}, Date: {Date:yyyy-MM-dd}, Quantity: {Quantity} {UnitOfMeasurement ?? ""}";
    }
}