using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HabitLoggerApp.Models
{
    public class HabitReport
    {
        public int HabitId { get; set; }
        public string HabitName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int EntryCount { get; set; }
        public int TotalQuantity { get; set; }
        public double AverageQuantityPerEntry => EntryCount > 0 ? (double)TotalQuantity / EntryCount : 0;
    }
}

