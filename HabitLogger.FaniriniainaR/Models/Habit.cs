
namespace HabitLogger.Models
{
    public class Habit
    {
        public int id { get; set; }
        public string Date { get; set; }
        public string quantity { get; set; }
        public int habitTypeId { get; set; }

    }
}
