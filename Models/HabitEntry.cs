using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLoggerApp.Models
{
    public class HabitEntry
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
