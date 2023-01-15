using MapDataReader;

namespace HabitTracker.edvaudin;

[GenerateDataReaderMapper]
public class Entry
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Measurement { get; set; } = string.Empty;

    public string GetString()
    {
        if (DateTime.TryParse(Date, out DateTime parsedDate))
        {
            return $"[#{Id}] {Quantity} {Measurement} on {parsedDate.ToLongDateString()}";
        }
        else
        {
            return $"[#{Id}] {Quantity} {Measurement} on {Date}";
        }
    }
}
