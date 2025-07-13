using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    internal class HabitRecord
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public string HabitName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
