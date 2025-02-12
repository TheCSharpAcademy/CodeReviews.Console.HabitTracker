namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Represent report unit, object containing current report data for the table
    /// </summary>
    internal class ReportUnit
    {
        public string Table { get; init; }
        public string Unit { get; init; }
        public int Count { get; init; }
        public int TotalVolume { get; init; }
        public int AverageVolume { get; init; }
        public int MinVolume { get; init; }
        public DateTime MinVolumeDate { get; init; }
        public int MaxVolume { get; init; }
        public DateTime MaxVolumeDate { get; init; }

        /// <summary>
        /// Initializes new object of <see cref="ReportUnit"/> class
        /// </summary>
        /// <param name="table"><see cref="string"/> value representing name of the table</param>
        /// <param name="unit"><see cref="string"/> value representing unit of the table</param>
        /// <param name="count"><see cref="int"/> value representing number of records in the table</param>
        /// <param name="totalVolume"><see cref="int"/> value representing total volume of all records</param></param>
        /// <param name="averageVolume"><see cref="int"/> value representing average volume for the records</param></param>
        /// <param name="minVolume"><see cref="int"/> value representing minimum volume </param>
        /// <param name="minVolumeDate"><see cref="DateTime"/> value representing Date when minimum value was inserted to the database</param>
        /// <param name="maxVolume"><see cref="int"/> value representing maximum volume </param>
        /// <param name="maxVolumeDate"><see cref="DateTime"/> value representing Date when maximu value was inserted to the database</param>
        public ReportUnit(string table, string unit, int count, int totalVolume, int averageVolume, int minVolume, DateTime minVolumeDate, int maxVolume, DateTime maxVolumeDate)
        {
            Table = table;
            Unit = unit;
            Count = count;
            TotalVolume = totalVolume;
            AverageVolume = averageVolume;
            MinVolume = minVolume;
            MinVolumeDate = minVolumeDate;
            MaxVolume = maxVolume;
            MaxVolumeDate = maxVolumeDate;
        }
        /// <summary>
        /// Created <see cref="string"/> value contaning report text
        /// </summary>
        /// <returns><see cref="string"/> value contaning report text</returns>
        public string GenerateReport()
        {
            return $"Here is your report for {Table}\n" +
                   $"\n" +
                   $"You have {Count} records\n" +
                   $"Your total volume was: {TotalVolume} {Unit}\n" +
                   "Your Average volume was {AverageVolume} {Unit}\n" +
                   $"Your minimum volume was {MinVolume} {Unit} which was done on {FormatDate(MinVolumeDate)}\n" +
                   $"Your maximum volume was {MaxVolume} {Unit} which was done on {FormatDate(MaxVolumeDate)}\n";
        }
        private string FormatDate(DateTime date) => date.ToString("dd-MM-yyyy");
    }
}
