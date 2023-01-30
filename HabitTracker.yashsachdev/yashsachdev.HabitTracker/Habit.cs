using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace yashsachdev.HabitTracker
{
    public class Habit
    {
        public Habit() { }  
        public Habit(int Habit_Id) { }

        public int Habit_Id { get; set; }
        public string Habit_Name { get; set; }
        public string unit { get; set; }

        public bool Validate()
        {
            bool isvalue = true;
            if (string.IsNullOrEmpty(Habit_Name) && string.IsNullOrEmpty(unit)) isvalue = false ;

            return isvalue;
        }
    }
}
