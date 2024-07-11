namespace HabitTracker.kwm0304.Models;

public class Habit
{
  public int HabitId { get; set; }
  public string? HabitName { get; set; }
  public string? UnitOfMeasurement { get; set; }
  public int Repetitions { get; set; }
  public DateTime StartedOn { get; set; }
  public bool IsMock { get; set; }
  public int DaysTracked => CalculateDaysTracked();
  public Habit() { }
  public Habit(string name, string unitOfMeasurement)
  {
    HabitName = name;
    UnitOfMeasurement = unitOfMeasurement;
    Repetitions = 0;
    StartedOn = DateTime.Today;
    IsMock = false;
  }
  public Habit(string name, string unitOfMeasurement, DateTime startedOn, int repetitions)
  {
    HabitName = name;
    UnitOfMeasurement = unitOfMeasurement;
    StartedOn = startedOn;
    Repetitions = repetitions;
    IsMock = true;
  }

  public int CalculateDaysTracked()
  {
    return (DateTime.Today - StartedOn).Days;
  }
  public override string ToString()
  {
    return $"{HabitName} {Repetitions} {UnitOfMeasurement} {StartedOn:MM/dd/yyyy}";
  }
}
