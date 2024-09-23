using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AnaClos.HabitTracker
{
    public class Habit
    {
        public Habit() { }
        
        public int Id { get; set; } = 0;
        public string Date { get; set; }
        public int Quantity { get; set; } = 0;

        public override string ToString()
        {
            return $"Id: {Id} Date: {Date} Quantity: {Quantity}";
        }
           
    }
}
