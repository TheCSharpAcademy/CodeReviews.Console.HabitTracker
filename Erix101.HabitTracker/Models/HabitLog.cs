namespace HabitTracker.Models
{
    internal class HabitLog
    {
        public int Id { get; set; }
        public DateTime HabitDate { get; set; }
        public string? HabitName { get; set; }
        public int HabitQuantity { get; set; }
        public string? HabitUnit { get; set; }

    }

}
