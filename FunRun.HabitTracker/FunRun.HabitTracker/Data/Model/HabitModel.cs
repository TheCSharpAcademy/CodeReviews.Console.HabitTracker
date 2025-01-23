namespace FunRun.HabitTracker.Data.Model;

public class HabitModel
{
    public int Id { get; set; }
    public string HabitName { get; set; }
    public string HabitDescription { get; set; } 
    public int HabitCounter { get; set; }

    public HabitModel(int id, string habitName, string habitDescription, int habitCounter)
    {
        Id = id;
        HabitName = habitName;
        HabitDescription = habitDescription;
        HabitCounter = habitCounter;
    }
}
