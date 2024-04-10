namespace DatabaseLibrary.Models;

internal class Habit
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Unit { get; set; }

  public Habit(int id, string name, string unit)
  {
    Id = id;
    Name = name;
    Unit = unit;
  }
}