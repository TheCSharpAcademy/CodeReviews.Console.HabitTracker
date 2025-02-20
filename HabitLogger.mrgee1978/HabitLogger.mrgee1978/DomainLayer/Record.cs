namespace HabitLogger.mrgee1978.DomainLayer;

public class Record
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string HabitName { get; set; }
    public string HabitMeasurement { get; set; }
    public int HabitId { get; set; }

    public Record(int id, DateTime date, int quantity, int habitId, string habitName, string habitMeasurement)
    {
        Id = id;
        Date = date;
        Quantity = quantity;
        HabitId = habitId;
        HabitName = habitName;
        HabitMeasurement = habitMeasurement;
    }
}
