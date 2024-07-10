using HabitTracker.kwm0304.Models;

namespace HabitTracker.kwm0304.Data;

public class Mocks
{
  private readonly DbActions _actions;
  public static Random random = new();
  public static string EntryName = "Name";
  public static string EntryUnitofMeasurement = "um";
  public Mocks(DbActions actions)
  {
    _actions = actions;
  }
  public static DateTime RandomDate()
  {
    DateTime today = DateTime.Today;
    DateTime startDate = today.AddYears(-5);
    int range = (today - startDate).Days;
    return startDate.AddDays(random.Next(range));
  }
  public static int RandomRepetition()
  {
    return random.Next(5, 10000);
  }
  public void GenerateMocks()
  {
    for (int i = 0; i < 100; i++)
    {
      string entryNum = i.ToString();
      Habit habit = new()
      {
        HabitName = EntryName + entryNum,
        UnitOfMeasurement = EntryUnitofMeasurement + i.ToString(),
        StartedOn = RandomDate(),
        Repetitions = RandomRepetition()
      };
      _actions.InsertHabit(habit);
      Console.WriteLine($"Creating mock habit {i}");
    }
  }
}
