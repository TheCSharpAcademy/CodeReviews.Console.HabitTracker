namespace habit_tracker
{
    class Program
    {
        static void Main(string[] args)
        {

            Repository.CreateHabit(true);
            Repository.GetUserInput();
        }
    }
}

public class HabitTable
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
}

public class TablesList
{
    public string? TableName { get; set; }
}