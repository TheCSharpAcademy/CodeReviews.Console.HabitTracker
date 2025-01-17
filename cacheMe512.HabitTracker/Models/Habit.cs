namespace habit_logger.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }

    public class HabitRecord
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
