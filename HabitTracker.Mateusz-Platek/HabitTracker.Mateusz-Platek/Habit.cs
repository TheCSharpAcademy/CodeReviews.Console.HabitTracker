namespace HabitTracker.Mateusz_Platek;

public class Habit
{
    public int id { get; set; }
    public string name { get; set; }
    public string unit { get; set; }

    public Habit(int id, string name, string unit)
    {
        this.id = id;
        this.name = name;
        this.unit = unit;
    }

    public override string ToString()
    {
        return $"{id}: {name} - {unit}";
    }
}