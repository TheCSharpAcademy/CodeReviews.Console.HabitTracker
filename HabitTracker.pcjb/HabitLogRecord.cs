namespace HabitTracker;

class HabitLogRecord : ICloneable
{
    public long ID { get; }
    public long HabitID { get; set; }
    public DateOnly Date { get; set; }
    public int Quantity { get; set; }

    public HabitLogRecord(long id, long habitID, DateOnly date, int quantity)
    {
        ID = id;
        HabitID = habitID;
        Date = date;
        Quantity = quantity;
    }

    public HabitLogRecord(long habitID, DateOnly date, int quantity)
    {
        HabitID = habitID;
        Date = date;
        Quantity = quantity;
    }

    public object Clone()
    {
        return new HabitLogRecord(ID, HabitID, Date, Quantity);
    }
}