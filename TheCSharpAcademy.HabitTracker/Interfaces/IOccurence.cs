using System.Linq;
using TheCSharpAcademy.HabitTracker.Models;

namespace TheCSharpAcademy.HabitTracker.Interfaces
{
   interface IOccurence
  {
    #region properties
    public int Id { get; set; }
    public int HabitID { get; set; }
    public string Date { get; set; }
    public double Value { get; set; }
    #endregion
    #region methods
    public bool LogOccurence(Occurence occurence);
    #endregion
  }
}
