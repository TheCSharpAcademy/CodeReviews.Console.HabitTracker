using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCSharpAcademy.HabitTracker.Models;

namespace TheCSharpAcademy.HabitTracker.Interfaces
{
   interface IOccurence
  {
    #region properties
    public int id { get; set; }
    public int HabitID { get; set; }
    public string Date { get; set; }
    public double value { get; set; }
    #endregion
    #region methods
    public bool LogOccurence(Occurence occurence);
    #endregion
  }
}
