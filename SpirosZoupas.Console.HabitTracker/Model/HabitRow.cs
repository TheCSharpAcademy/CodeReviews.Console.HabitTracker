namespace SpirosZoupas.Console.HabitTracker.Model
{
    public class HabitRow
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string HabitName { get; set; }
        public string MeasurementUnit { get; set; }

    }
}