namespace DatabaseLibrary.Models;

internal class Record
{
  public int Id { get; set; }
  public int HabitId { get; set; }
  public DateTime Date { get; set; }
  public int Quantity { get; set; }

  public Record(int id, int habitId, DateTime date, int quantity)
  {
    Id = id;
    HabitId = habitId;
    Date = date;
    Quantity = quantity;
  }
}