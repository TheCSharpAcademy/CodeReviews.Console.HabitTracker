namespace HabitTracker.Mateusz_Platek;

public class HabitLog
{
    public int id { get; set; }
    public DateTime date { get; set; }
    public int quantity { get; set; }
    public string name { get; set; }
    public string unit { get; set; }

    public HabitLog(int id, DateTime date, int quantity, string name, string unit)
    {
        this.id = id;
        this.date = date;
        this.quantity = quantity;
        this.name = name;
        this.unit = unit;
    }

    public override string ToString()
    {
        string day = date.Day.ToString("00");
        string month = date.Month.ToString("00");
        string year = date.Year.ToString("0000");
        return $"{id}: {name} - {quantity} {unit} - {day}-{month}-{year}";
    }
}