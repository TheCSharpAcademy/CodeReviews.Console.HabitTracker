public class WaterHabit
{
    public WaterHabit(int id, DateTime date, int quantity)
    {
        Id = id;
        Date = date;
        Quantity = quantity;
    }

    public WaterHabit() { }

    public int Id { set; get; }
    public DateTime Date { set; get; }
    public int Quantity { set; get; }
}