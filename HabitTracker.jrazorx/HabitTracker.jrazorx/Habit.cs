public class Habit
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public int HabitTypeId { get; set; }
    public string HabitTypeName { get; set; }
    public string Unit { get; set; }
}