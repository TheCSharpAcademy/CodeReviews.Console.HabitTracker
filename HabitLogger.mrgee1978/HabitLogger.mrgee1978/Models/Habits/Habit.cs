namespace mrgee1978.HabitLogger.Models.Habits;

// This class represents a habit allowing you to
// enter both the habit's name and description
public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Habit(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
