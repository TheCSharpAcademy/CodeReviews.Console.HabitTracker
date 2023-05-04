namespace HabitTracker
{
    internal struct HabitRecord
    {
        public int Id;
        public DateTime Date;
        public string Value;

        public HabitRecord(int id, DateTime date, string value)
        {
            Id = id;
            Date = date;
            Value = value;
        }
    }
}
