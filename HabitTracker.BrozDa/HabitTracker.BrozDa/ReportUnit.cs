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

        public override string ToString()
        {
            StringBuilder report = new StringBuilder();
            report.Append($"Here is your report for {Table}\n");
            report.Append("\n");
            report.Append($"You have {Count} records\n");
            report.Append($"Your total volume was: {TotalVolume} {Unit}\n");
            report.Append($"Your Average volume was {AverageVolume} {Unit}\n");
            report.Append($"Your minimum volume was {MinVolume} {Unit} which was done on {MinVolumeDate.ToString("dd-MM-yyyy")}\n");
            report.Append($"Your maximum volume was {MaxVolume} {Unit} which was done on {MaxVolumeDate.ToString("dd-MM-yyyy")}\n");

            return report.ToString();
        }

    }
}
