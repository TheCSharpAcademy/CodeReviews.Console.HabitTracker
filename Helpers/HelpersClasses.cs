public class HabitName
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? MeasurementUnit { get; set; }
}

public class HabitRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? HabitName { get; set; }
    public int Value { get; set; }
    public string? MeasurementUnit { get; set; }
}

public class HabitReport
{
    public string? HabitName { get; set; }
    public string? MeasurementUnit { get; set; }
    public int TotalValue { get; set; }
}
