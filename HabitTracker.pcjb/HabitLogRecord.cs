namespace HabitTracker;

class HabitLogRecord
{
    public long ID { get; }
    public DateOnly Date { get; set; }
    public int Quantity { get; set; }

    public HabitLogRecord(Int64 id, DateOnly date, int quantity)
    {
        ID = id;
        Date = date;
        Quantity = quantity;
    }

    public HabitLogRecord(DateOnly date, int quantity)
    {
        Date = date;
        Quantity = quantity;
    }
}