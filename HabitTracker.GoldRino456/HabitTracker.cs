namespace HabitTracker.GoldRino456
{
    class HabitTracker
    {
        static void Main()
        {
            Habit testHabit = new(new DateTime(2025, 6, 11), "Sleep", 5.72f, "Hours");
            Console.WriteLine(testHabit);
        }
    }
}