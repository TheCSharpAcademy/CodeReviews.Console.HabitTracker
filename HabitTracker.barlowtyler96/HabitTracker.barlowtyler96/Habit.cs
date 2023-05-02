namespace HabitTracker
{
    internal class Habit
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Activity { get; set; }

        public string Unit { get; set; }

        public int Amount { get; set; }
    }
}