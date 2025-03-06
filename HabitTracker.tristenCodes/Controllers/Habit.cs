namespace Controllers;

public class Habit
{
    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime Date { get; set; }
    public int Occurences { get; set; } = 1;

}
