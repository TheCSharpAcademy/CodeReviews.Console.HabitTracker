using mrgee1978.HabitLogger.Models.Habits;

namespace mrgee1978.HabitLogger.Models.Records;

// This class keeps records of each individual habit
public class Record
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public int HabitId { get; set; }

    public Record(int id, DateTime date, int quantity, int habitId)
    {
        Id = id;
        Date = date;
        Quantity = quantity;
        HabitId = habitId;
    }

}
