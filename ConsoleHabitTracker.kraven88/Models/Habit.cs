using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHabitTracker.kraven88.Models;
internal class Habit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UnitOfMeasurement { get; set; } = string.Empty;
    public List<DailyProgress> ProgressList { get; set; } = new();
    public int CurrectGoal { get; set; } = 0;
}
