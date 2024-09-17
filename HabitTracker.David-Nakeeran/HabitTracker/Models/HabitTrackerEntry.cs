namespace HabitTracker.Models;
internal class HabitTrackerEntry
{
    internal int Id { get; set; }
    internal DateTime Date { get; set; }
    internal int Quantity { get; set; }

    internal HabitTrackerEntry(int id, DateTime date, int quantity)
    {
        Id = id;
        Date = date;
        Quantity = quantity;
    }
}