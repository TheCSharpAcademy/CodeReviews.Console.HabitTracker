using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHabitTracker.kraven88.Models;
internal class DailyProgress
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int Quantity { get; set; }
}
