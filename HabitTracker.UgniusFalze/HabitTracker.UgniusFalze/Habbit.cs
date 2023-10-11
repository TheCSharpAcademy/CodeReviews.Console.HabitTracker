namespace HabitTracker.UgniusFalze
{
    internal class Habbit
    {
        public long Id { get; set;}
        public long Quantity { get; set; }
        public DateOnly Date { get; set; }

        public Habbit(long id, long quantity, DateOnly date)
        {
            this.Id = id;
            this.Quantity = quantity;
            this.Date = date;
        }
    }
}
