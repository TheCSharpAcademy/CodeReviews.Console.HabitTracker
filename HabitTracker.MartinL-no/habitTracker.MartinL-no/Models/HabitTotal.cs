namespace HabitTracker.MartinL_no.Models;

internal class HabitTotal
{
    internal string Name { get; }
    internal int Total { get; }

    internal HabitTotal(string name, int total)
    {
        Name = name;
        Total = total;
    }
}