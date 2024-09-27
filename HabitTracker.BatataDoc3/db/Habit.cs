using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.BatataDoc3.db
{
    internal class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public Habit(int id, string name, DateTime date)
        {
            Id = id;
            Name = name;
            Date = date;
        }
    }
}
