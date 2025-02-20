namespace HabitLogger.mrgee1978.DomainLayer;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UnitOfMeasurement { get; set; }

    public Habit(int id, string name, string unitOfMeasurement)
    {
        Id = id;
        Name = name;
        UnitOfMeasurement = unitOfMeasurement;
    }
}
