namespace HabitTracker.MartinL_no.Models;

internal class Habit
{
    internal int Id { get; }
    internal string Name { get; }
    internal List<HabitDate> Dates { get; }

    internal Habit(string name, List<HabitDate> dates)
    {
        Name = name;
        Dates = dates;
    }

    internal Habit(int id, string name, List<HabitDate> dates) : this(name, dates)
    {
        Id = id;
    }
}