namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Represent Record unit used for database entries
    /// </summary>
    internal class DatabaseRecord
    {
        public int ID { get; set; } 
        public DateTime Date { get; set; }
        public int Volume { get; set; }

        /// <summary>
        /// Initializes new empty object of <see cref="DatabaseRecord"/> class
        /// </summary>
        public DatabaseRecord(){}
        /// <summary>
        /// Initializes new object of <see cref="DatabaseRecord"/> class
        /// </summary>
        /// <param name="id"><see cref="int"/> representing unique ID of the record</param>
        /// <param name="date"><see cref="DateTime"/> representing date of record</param>
        /// <param name="volume"><see cref="int"/> representing volume of activity done</param>
        public DatabaseRecord(int id, DateTime date, int volume)
        {
            ID = id;
            Date = date;
            Volume = volume;
        }
    }
}
