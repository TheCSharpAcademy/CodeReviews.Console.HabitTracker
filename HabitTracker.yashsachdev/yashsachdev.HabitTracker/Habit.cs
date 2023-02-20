namespace yashsachdev.HabitTracker;

public class Habit
{
    public Habit() { }
    public Habit(int habit_Id)
    {
        Habit_Id = habit_Id; habitEnrolls = new List<HabitEnroll>();
    }
    public int Habit_Id { get; set; }
    public string Habit_Name { get; set; }
    public string Unit { get; set; }
    public List<HabitEnroll> habitEnrolls { get; set; }
}
