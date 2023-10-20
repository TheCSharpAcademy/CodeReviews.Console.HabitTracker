namespace HabitTracker.iGoodw1n
{
    public class WrittenCode
    {
        public int Id { get; set; }
        public string? Language { get; set; }
        public int Lines { get; set; }

        public DateTime Date { get; set; }

        public override string ToString() => $"{Id} {Language} {Lines} {Date:d}";
    }
}