namespace yashsachdev.HabitTracker;

internal class Report
{
    public Report(string username, string habitname, DateTime date, string unit)
    {
        UserName = username;
        HabitName = habitname;
        Date = date;
        Unit = unit;
    }
    public string UserName { get; set; }
    public string HabitName { get; set; }
    public DateTime Date { get; set; }
    public string Unit { get; set; }
}
