namespace DatabaseHandler
{
    public sealed class Record
    {
        public int record_id { get; set; }
        public string habit_name { get; set; }
        public string stat_name { get; set; }
        public int stat_value { get; set; }
        public string entry_timestamp { get; set; }
    }
}
