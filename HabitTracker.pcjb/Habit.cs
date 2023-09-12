namespace HabitTracker;

class Habit
{
    public long ID { get; }
    public string Name { get; set; }
    public string Uom { get; set; }

    public Habit(long id, string name, string uom)
    {
        ID = id;
        Name = name;
        Uom = uom;
    }

    public Habit(string name, string uom)
    {
        Name = name;
        Uom = uom;
    }
}