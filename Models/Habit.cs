namespace Models
{
    public abstract class Habit
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public HabitType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public TimeSpan Duration { get; set; }
        public int OwnerId { get; set; }

    }
}
