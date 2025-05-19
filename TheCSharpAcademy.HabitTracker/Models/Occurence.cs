using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCSharpAcademy.HabitTracker.Interfaces;

namespace TheCSharpAcademy.HabitTracker.Models
{
  internal class Occurence(string date, int HabitID, double value) : IOccurence
  {
    #region properties
    public int id { get; set; }
    public int HabitID { get; set; } = HabitID;
    public string Date { get; set; } = date;
    public double value { get; set; } = value;

    #endregion
    #region constructors
    #endregion
    #region methods
    public bool LogOccurence(Occurence occurence)
    {
      DatabaseHandler databaseHandler = new();
      databaseHandler.Connect();
      if (databaseHandler.LogOccurence(occurence))
      {
        
        return true;
      }
      else
      {
        Console.WriteLine("Failed to log occurence");
        return false;
      }
    }
    #endregion
  }
}
