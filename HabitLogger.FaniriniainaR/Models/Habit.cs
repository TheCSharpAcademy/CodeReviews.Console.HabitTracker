
namespace HabitLogger.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Quantity { get; set; }
        public int HabitTypeId { get; set; }

    }
}
