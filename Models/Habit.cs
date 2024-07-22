namespace Models;

public record class Habit(int? Id, string Name, int Quantity, string Date)
{
    public override string ToString()
    {
        return $"{Id} - {Name} - {Quantity} - {Date}";
    }

    public string[] ToStringArray()
    {
        ArgumentNullException.ThrowIfNull(Id);
#pragma warning disable CS8601 // Possible null reference assignment.
        return [Id.ToString(), Name, Quantity.ToString(), Date];
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
