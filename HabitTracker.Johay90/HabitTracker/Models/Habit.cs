public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Measurement { get; set; }
    public int Quantity { get; set; }
    public string Frequency { get; set; }
    public string DateCreated { get; set; }
    public string DateUpdated { get; set; }
    public string Notes { get; set; }
    public string Status { get; set; }

    public override string ToString()
    {
        return $"{Id,-5} {Name,-20} {Measurement,-15} {Quantity,-10} {Frequency,-10} {DateCreated,-20} {DateUpdated,-20} {Notes,-30} {Status,-10}";
    }
}