using System.ComponentModel;

namespace HabitLogger.Model
{
    public class Habit
    {
        public int Id {  get; set; }
        public DateTime Date { get; set; }

        public int MetricValue { get; set; }
        
        public HabitType Type { get; set; }
        
        
    }
}