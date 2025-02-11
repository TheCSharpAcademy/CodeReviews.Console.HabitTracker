using System.Text;


namespace HabitTracker.BrozDa
{
    
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
