class HabitEntry
{
    private string _habit = "";
    private DateTime _date;

    public string Habit
    {
        get
        {
            return _habit;
        }
        set
        {
            _habit = value;
        }
    }

    public DateTime Date
    {
        get
        {
            return _date;
        }
        set
        {

            DateTime DT = DateTime.ParseExact(value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }
    }
}
