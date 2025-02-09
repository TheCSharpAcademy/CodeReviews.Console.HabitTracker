namespace habbitTracker.fatihskalemci.Models
{
    internal class Habbit
    {
        public int Id { get; set; }
        public required string HabbitName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public required string Unit { get; set; }
    }
}
