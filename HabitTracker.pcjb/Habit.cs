namespace HabitTracker;

class Habit
{
    public string Name { get; set; }
    public string UOM { get; set; }

    public Habit(string name, string uom)
    {
        Name = name;
        UOM = uom;
    }
}