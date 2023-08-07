namespace HabitTracker.MartinL_no.Models;

internal class HabitDate
{
    internal int Id { get; }
    internal DateOnly Date { get; }
    internal int Count { get; }
    internal int HabitId { get; }

    internal HabitDate(DateOnly date, int count, int habitId)
    {
        if (count < 1) throw new ArgumentException();

        Date = date;
        Count = count;
        HabitId = habitId;
    }

    internal HabitDate(int id, DateOnly date, int count, int habitId) : this(date, count, habitId)
    {
        Id = id;
    }
}