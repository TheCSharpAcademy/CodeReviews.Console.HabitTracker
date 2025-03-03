namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Represent report unit, object containing current report data for the habit
    /// </summary>
    internal class ReportUnit
    {
        public string HabitName { get; init; }
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
        /// <param name="habit"><see cref="Habit"/> for which report should be generated</param>
        /// <param name="records"><see cref="List{T}"/> of records for which report should be generated</param>
        public ReportUnit(Habit habit, List<HabitRecord> records)
        {
            HabitName = habit.Name;
            Unit = habit.Unit;
            Count = records.Count;
            TotalVolume = records.Sum(x => x.Volume);
            AverageVolume = TotalVolume/Count;
            MinVolume = records.Min(x => x.Volume);
            MinVolumeDate = records.Find(x => x.Volume == MinVolume)!.Date;
            MaxVolume = records.Max(x => x.Volume);
            MaxVolumeDate = records.Find(x => x.Volume == MaxVolume)!.Date;
        }
        /// <summary>
        /// Created <see cref="string"/> value contaning report text
        /// </summary>
        /// <returns><see cref="string"/> value contaning report text</returns>
        public string GenerateReport()
        {
            return $"Here is your report for {HabitName}\n" +
                   $"\n" +
                   $"You have {Count} records\n" +
                   $"Your total volume was: {TotalVolume} {Unit}\n" +
                   $"Your Average volume was {AverageVolume} {Unit}\n" +
                   $"Your minimum volume was {MinVolume} {Unit} which was done on {FormatDate(MinVolumeDate)}\n" +
                   $"Your maximum volume was {MaxVolume} {Unit} which was done on {FormatDate(MaxVolumeDate)}\n";
        }
        /// <summary>
        /// Formats the Date of the record to desired form
        /// </summary>
        /// <param name="date"><see cref="DateTime"/> from the record</param>
        /// <returns>Formated input</returns>
        private string FormatDate(DateTime date) => date.ToString("dd-MM-yyyy");
    }
}
