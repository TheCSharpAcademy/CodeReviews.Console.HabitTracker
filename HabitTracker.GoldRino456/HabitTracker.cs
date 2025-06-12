namespace HabitTracker.GoldRino456
{
    class HabitTracker
    {
        static void Main()
        {
            Habit testHabit1 = new(new DateTime(2025, 6, 11), "Sleep", 5.72f, "Hours");
            Habit testHabit2 = new(new DateTime(2025, 6, 11), "Sleep", 5.72f, "Hours");
            Habit testHabit3 = new(new DateTime(2025, 6, 11), "Sleep", 5.72f, "Hours");

            DBManager dbManager = DBManager.Instance;

            dbManager.AddHabitToDB(testHabit1);
            dbManager.AddHabitToDB(testHabit2);
            dbManager.AddHabitToDB(testHabit3);

            var habits = dbManager.GetAllExistingHabitEntries();

            foreach (var habit in habits)
            {
                Console.WriteLine(habit);
            }
        }
    }
}