
namespace AnaClos.HabitTracker
{
    public class Habit
    {
        public Habit() { }
        
        public int Id { get; set; } 
        public string Date { get; set; }
        public int Quantity { get; set; } 

        public override string ToString()
        {
            return $"Id: {Id} Date: {Date} Quantity: {Quantity}";
        }
           
    }
}
