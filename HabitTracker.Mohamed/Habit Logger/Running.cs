using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Logger
{
    public class Running
    {
        private int id;
        private DateTime date;
        private int kilometers;
        public Running(int id ,string date, int kilometers)
        {
            this.date =  DateTime.Parse(date);
            this.kilometers = kilometers;
            this.id = id;
        }

        public override string ToString()
        {
            return $"{this.id} - {this.date} - Kilometers: {this.kilometers}";
        }
    }
}
