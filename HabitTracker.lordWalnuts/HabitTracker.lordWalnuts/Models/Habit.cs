namespace HabitTracker.lordWalnuts.Models
{
    internal class Habit
    {
        public int Id { get; set; }
        public string HabitName { get; set; }
        public DateTime Date { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
    }
}
