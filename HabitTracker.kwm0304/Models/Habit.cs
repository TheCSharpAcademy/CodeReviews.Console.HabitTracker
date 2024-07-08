namespace HabitTracker.kwm0304.Models;

public class Habit
{
  public int HabitId { get; set; }
  public string HabitName { get; set; }
  public string UnitOfMeasurement { get; set; }
  public int Repetitions { get; set; }
  public DateTime StartedOn { get; set; }
  public int DaysTracked => CalculateDaysTracked();
  public Habit(){}
  public Habit(string name, string unitOfMeasurement)
  {
    HabitName = name;
    UnitOfMeasurement = unitOfMeasurement;
    Repetitions = 0;
    StartedOn = DateTime.Today;
  }

  private int CalculateDaysTracked()
  {
    return (DateTime.Today - StartedOn).Days;
  }
}
