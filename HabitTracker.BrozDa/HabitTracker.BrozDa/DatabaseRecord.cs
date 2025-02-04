namespace HabitTracker.BrozDa
{
    internal class DatabaseRecord
    {
        public string Date { get; set; }
        public string Volume { get; set; }

        public DatabaseRecord(string date, string volume)
        {
            Date = date;
            Volume = volume;
        }
    }
}
