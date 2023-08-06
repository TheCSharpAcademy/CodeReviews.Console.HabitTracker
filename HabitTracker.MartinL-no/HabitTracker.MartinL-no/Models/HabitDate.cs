namespace HabitTracker.MartinL_no.Models;

internal class HabitDate
{
    internal int Id { get; }
    internal DateOnly Date { get; }
    internal int Count { get; set; }

    internal HabitDate(int id, DateOnly date, int count)
    {
        Id = id;
        Date = date;
        Count = count;
    }
}