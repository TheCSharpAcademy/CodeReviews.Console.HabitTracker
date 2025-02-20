using System.Runtime.CompilerServices;

namespace HabitTracker.S1m0n32002.Models
{
    public class Habit
    {
        public string Name { get; set; }
        public DateTime LastOccurrance { get; set; }
        public Habit()
        {
            this.Name = "";
            this.LastOccurrance = new DateTime();
        }

        public Habit(string Name, DateTime LastOccurrance)
        {
            this.Name = Name;
            this.LastOccurrance = LastOccurrance;
        }
    }
}
