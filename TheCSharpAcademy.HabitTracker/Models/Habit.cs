using System.Data.SQLite;
using System.Linq;
using TheCSharpAcademy.HabitTracker.Interfaces;
namespace TheCSharpAcademy.HabitTracker.Models
{
  /// <summary>
  /// Constructor for the habit class
  /// </summary>
  /// <param name="habitname"></param>
  /// <param name="unit"></param>
  /// <param name="amount"></param>
  internal class Habit(string habitname, string unit, double amount) : IHabit
  {
    #region properties
    public int Id { get; set; }
    public string Habitname { get; set; } = habitname;
    public string MeasuringUnit { get; set; } = unit;
    public double Amount { get; set; } = amount;

    #endregion
    

    #region methods
    /// <summary>
    /// Adds a habit to the database
    /// </summary>
    /// <param name="habit"></param>
    /// <returns>True if succesfull, false if not</returns>
    public bool AddHabit(Habit habit)
    {
      DatabaseHandler databaseHandler = new();

      if (databaseHandler.AddHabit(habit))
      {
        Console.WriteLine("Habit created succesfully");
        return true;
      }
      return false;
    }
    /// <summary>
    /// Removes a habit from the database
    /// </summary>
    /// <param name="habit"></param>
    public bool RemoveHabit(Habit habit)
    {
      DatabaseHandler databaseHandler = new();
      databaseHandler.Connect();
      if (databaseHandler.RemoveHabit(habit))
      {
        return true;
      }
      else
      {
        Console.WriteLine("Habit not removed");
        return false;
      }
    }
    /// <summary>
    /// Updates a habit in the database
    /// </summary>
    /// <param name="oldHabit"></param>
    /// <param name="newHabit"></param>
    public bool UpdateHabit(Habit oldHabit, Habit newHabit)
    {
      DatabaseHandler databaseHandler = new();
      if (databaseHandler.UpdateHabit(oldHabit, newHabit))
      {
        return true;
      }
      else
      {
        Console.WriteLine("Habit not updated");
        return false;
      }
    }
    #endregion
  }
}
