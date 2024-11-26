namespace HabitLoggerApp;

public record struct Habit(int Id, string Date, string Body, int Quantity)
{
    public override string ToString()
    {
        return $"{Id} - {Body} - {Quantity} - {Date}";
    }
}