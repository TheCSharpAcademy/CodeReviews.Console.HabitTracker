namespace HabitLogger
{
    public class CSharpLog
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public int Hours { get; set; }
    }
}
