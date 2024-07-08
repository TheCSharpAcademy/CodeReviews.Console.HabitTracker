namespace csa_habit_logger
{
    public class NewHabitRecord
    {
        public Habit Habit { get; private set; }

        public int Amount { get; private set; }

        public DateTime DateTime { get; private set; }

        public NewHabitRecord(Habit habit, DateTime datetime, int amount)
        {
            Habit = habit;
            DateTime = datetime;
            Amount = amount;
        }

        public NewHabitRecord(Habit habit, int unixTime, int amount)
        {
            Habit = habit;
            DateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
            Amount = amount;
        }
    }
}