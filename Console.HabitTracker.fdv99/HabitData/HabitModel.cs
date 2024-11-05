using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitData
{
    public class HabitModel
    {
        public int Id { get; set; }
        public string Habit { get; set; }
        public double Quantity { get; set; }
        public DateTime Date { get; set; }

    }
}
