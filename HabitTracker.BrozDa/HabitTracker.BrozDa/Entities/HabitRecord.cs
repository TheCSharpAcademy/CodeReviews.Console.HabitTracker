namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Class representing objects for <see cref="HabitRecordRepository"/>
    /// </summary>
    internal class HabitRecord
    {
        public int Id { get; set; } 
        public DateTime Date { get; set; }
        public int Volume { get; set; }
        public int HabitId { get; set; }
    }
}
