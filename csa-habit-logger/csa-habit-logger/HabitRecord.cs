
using Microsoft.VisualBasic;

namespace csa_habit_logger
{
    public class HabitRecord
    {
        public int ID { get; private set; }

        public Habit Habit { get; set; }

        public int Amount { get; set; }

        public DateTime DateTime { get; set; }

        public HabitRecord(int id, Habit habit, DateTime datetime, int amount)
        {
            ID = id;
            Habit = habit;
            DateTime = datetime;
            Amount = amount;
        }

        public HabitRecord(int id, Habit habit, int unixTime, int amount)
        {
            ID = id;
            Habit = habit;
            DateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Habit.Name} : {Amount} {Habit.Unit} @ {DateTime.ToString()}";
        }
    }
}
