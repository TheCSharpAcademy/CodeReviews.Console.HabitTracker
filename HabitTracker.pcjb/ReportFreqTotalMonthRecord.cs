namespace HabitTracker;

class ReportFreqTotalMonthRecord
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Frequency { get; set; }
    public int Total { get; set; }

    public ReportFreqTotalMonthRecord(int year, int month, int frequency, int total)
    {
        Year = year;
        Month = month;
        Frequency = frequency;
        Total = total;
    }
}