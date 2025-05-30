namespace Habit_Logger.Data
{
    internal class Data
    {
        internal record Habit(int Id, string Name, string UnitOfMeasurement);

        internal record ProgressWithHabit(int Id, DateTime Date, int Quantity, string HabitName, string MeasurementUnit);

    }
}
