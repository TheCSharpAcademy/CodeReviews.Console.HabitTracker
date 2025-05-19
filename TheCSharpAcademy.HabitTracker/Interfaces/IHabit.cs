using System.Linq;
using TheCSharpAcademy.HabitTracker.Models;

namespace TheCSharpAcademy.HabitTracker.Interfaces
{
  interface IHabit
  {
    #region properties
    public int Id { get; set; }
    public string Habitname { get; set; }
    public string MeasuringUnit { get; set; }
    public double Amount { get; set; }
    #endregion
    #region methods
    public bool AddHabit(Habit habit);
    public bool RemoveHabit(Habit habit);
    public bool UpdateHabit(Habit oldHabit,Habit newHabit);
    #endregion
  }
}
