namespace yashsachdev.HabitTracker;

public class HabitEnroll
{
    public User User { get; set; }
    public int User_Id { get; set; }
    public Habit habit { get; set; }
    public int Habit_Id { get; set; }
    public DateTime Date { get; set; } 
}
