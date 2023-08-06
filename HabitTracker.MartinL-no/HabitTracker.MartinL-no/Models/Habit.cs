namespace HabitTracker.MartinL_no.Models;

internal class Habit
{
    internal readonly int _id;
    internal readonly string Name;
    internal List<HabitDate> Dates { get; private set; }

    internal Habit(int id, string name, List<HabitDate> dates)
    {
        _id = id;
        Name = name;
        Dates = dates;
    }
}