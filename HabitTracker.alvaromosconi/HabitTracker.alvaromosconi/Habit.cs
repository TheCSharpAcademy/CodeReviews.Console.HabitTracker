using System.Diagnostics.Metrics;

namespace HabitTracker.alvaromosconi
{
    internal class Habit
    { 
        public int Id { get; set; }
        public string Name { get; set; }
            
        public DateTime Date { get; set; }

        public int Quantity { get; set; }
    }
}
