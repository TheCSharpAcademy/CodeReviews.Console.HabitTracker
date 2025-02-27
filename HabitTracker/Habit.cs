namespace HabitTracker
{
    internal class Habit
    {
        internal required int Id { get; set; }
        internal required string Title { get; set; }
        internal required bool IsCompleted {  get; set; }
        internal required string DateCreated {  get; set; }

    }
}
