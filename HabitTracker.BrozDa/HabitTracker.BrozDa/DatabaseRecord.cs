namespace HabitTracker.BrozDa
{
    internal class DatabaseRecord
    {
        public int ID { get; set; } 
        public DateTime Date { get; set; }
        public int Volume { get; set; }

        public DatabaseRecord(){}
        public DatabaseRecord(int id, DateTime date, int volume)
        {
            ID = id;
            Date = date;
            Volume = volume;
        }
    }
}
