namespace HabitTracker.Mateusz_Platek;

public class Unit
{
    public int id { get; set; }
    public string unit { get; set; }

    public Unit(int id, string unit)
    {
        this.id = id;
        this.unit = unit;
    }

    public override string ToString()
    {
        return $"{id}: {unit}";
    }
}