using Spectre.Console;

namespace CodingTracker;

public class HabitLogger
{
    private int id;
    private DateTime date;
    private int kilometers;

    public int Id { get => id; set => id = value; }
    public DateTime Date { get => date.Date; set => date = value; }
    public int Kilometers { get => kilometers; set => kilometers = value; }

    public HabitLogger(int id, DateTime date, int kilometers)
    {
        this.id = id;
        this.date = date;
        this.kilometers = kilometers;
    }
}
