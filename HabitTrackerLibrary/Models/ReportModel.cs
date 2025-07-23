namespace HabitTrackerLibrary.Models
{
    public class ReportModel
    {
        private List<RecordModel> sortedRecords = new List<RecordModel>();
        private DateTime startDate = new DateTime(); 
        private DateTime endDate = new DateTime();
        private int startIndex;
        private int endIndex;

        public int RecordCount { get; private set; }
        public int DayCount { get; private set; }
        public double Sum { get; private set; }
        public double DailyAverage { get; private set; }
        public int StreakDuration { get; private set; }
        public double StreakQuantity { get; private set; }
        public DateTime StreakStartDate { get; private set; }



        public ReportModel(List<RecordModel> sortedRecords, DateTime startDate, DateTime endDate)
        {
            this.sortedRecords = sortedRecords;
            this.startDate = startDate;
            this.endDate = endDate;

            GetIndexForStartDate();
            GetIndexForEndDate();
            GenerateReportData();
        }


        private void GetIndexForStartDate()
        {
            startIndex = sortedRecords.FindIndex(a => a.Date == startDate);
            if (startIndex == -1) { startIndex = 0; }
        }
        private void GetIndexForEndDate()
        {
            endIndex = sortedRecords.FindLastIndex(a => a.Date == endDate);
            if (endIndex == -1) { endIndex = sortedRecords.Count - 1; }
        }
        private void GenerateReportData()
        {
            int previousStreakDuration = 0;
            double previousStreakQuantity = 0;
            DateTime previousStreakStartDate = startDate;

            DateTime previousDate = startDate.AddDays(-1);
            StreakStartDate = sortedRecords.First<RecordModel>().Date;


            if (startDate.Date < sortedRecords.First<RecordModel>().Date.Date)
            {
                previousDate = sortedRecords.First<RecordModel>().Date.AddDays(-1);
            }
            else
            {
                StreakStartDate = startDate.Date;
            }

            foreach (var record in sortedRecords)
            {
                if (record.Date >= startDate && record.Date <= endDate)
                {
                    RecordCount++;
                    Sum += record.Quantity;

                    if (record.Date.Date != previousDate.Date)
                    {
                        DayCount++;
                    }

                    if (record.Date.Date == previousDate.Date.AddDays(1))
                    {
                        StreakDuration++;
                        StreakQuantity += record.Quantity;
                    }
                    else if (record.Date.Date == previousDate.Date)
                    {
                        StreakQuantity += record.Quantity;
                    }
                    else
                    {
                        if (StreakDuration > previousStreakDuration)
                        {
                            previousStreakDuration = StreakDuration;
                            previousStreakQuantity = StreakQuantity;
                            previousStreakStartDate = StreakStartDate;
                        }

                        StreakDuration = 1;
                        StreakQuantity = record.Quantity;
                        StreakStartDate = record.Date;
                    }
                }
                previousDate = record.Date;
            }

            if (previousStreakDuration > StreakDuration)
            {
                StreakQuantity = previousStreakQuantity;
                StreakDuration = previousStreakDuration;
                StreakStartDate = previousStreakStartDate;
            }

            DailyAverage = Sum / DayCount;
        }
    }
}
