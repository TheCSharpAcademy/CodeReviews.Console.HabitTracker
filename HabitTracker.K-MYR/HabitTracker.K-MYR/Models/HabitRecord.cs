namespace HabitTracker.K_MYR.Models
{
    internal class HabitRecord
    {
        public int Id { get; set; }

        public string Habit { get; set; }

        public string Measurement { get; set; }

        public DateTime Date { get; set; }

        public int Quantity { get; set; }
    }

    internal class Habit
    {
        public string Name { get; set; }

        public string Measurement { get; set; }
    }
}
