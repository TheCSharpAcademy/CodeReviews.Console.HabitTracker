// Represents a habit definition (name and unit)
namespace HabitTracker;

public class Habit
{
    public int Id { get; set; }
    public required string Name { get; set; } // Use 'required' or ensure non-null in constructor
    public required string UnitOfMeasurement { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Unit: {UnitOfMeasurement}";
    }
}