namespace habit_tracker
{
    class Program
    {
        static void Main(string[] args)
        {

            Repository.CreateTable();
            Repository.GetUserInput();
        }

    }
}

public class CsharpLessons
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}