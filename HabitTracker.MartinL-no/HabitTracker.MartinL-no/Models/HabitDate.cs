namespace HabitTracker.MartinL_no.Models;

internal class HabitDate
{
    internal int Id { get; }
    internal DateOnly Date { get; }
    internal int Count { get; set; }

    internal HabitDate(int id, DateOnly date, int count = 1)
    {
        if (count == 0) throw new ArgumentException();

        Id = id;
        Date = date;
        Count = count;
    }
}