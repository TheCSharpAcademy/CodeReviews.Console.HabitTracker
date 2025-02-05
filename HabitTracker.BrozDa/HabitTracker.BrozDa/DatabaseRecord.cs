namespace HabitTracker.BrozDa
{
    internal class DatabaseRecord
    {
        public int ID { get; set; } 
        public string? Date { get; set; }
        public string? Volume { get; set; }

        public DatabaseRecord()
        {
            
        }
        public DatabaseRecord(int id, string date, string volume)
        {
            ID = id;
            Date = date;
            Volume = volume;
        }
    }
}
