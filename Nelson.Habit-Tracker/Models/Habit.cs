namespace Nelson.Habit_Tracker.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Name { get; set; }
        public string? Measurement { get; set; }
        public int Quantity { get; set; }

    }
}