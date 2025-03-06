namespace HabitTracker.DataHelpers
{
    internal class InitialStarterData
    {
        internal class ExampleHabit
        {
            public string Habit { get; set; }
            public string Unit { get; set; }
            public ExampleHabit(string habit, string unit)
            {
                this.Habit = habit;
                this.Unit = unit;
            }
        }
        internal static List<ExampleHabit> ExampleHabits = new()
        {
             new ExampleHabit("Drink Water","Glasses"),
             new ExampleHabit("Yoga", "45 minute Session"),
             new ExampleHabit("Drink Beer", "Pint"),
             new ExampleHabit("Meditate", "30 minute Session"),
             new ExampleHabit("Code", "App completed")
        };

        internal class ExampleHabitLog
        {
            public string Date { get; set; }
            public int Quantity { get; set; }
            public ExampleHabitLog(string date, int quantity)
            {
                this.Date = date;
                this.Quantity = quantity;
            }
        }

        internal static List<ExampleHabitLog> ExampleHabitLogs = new()
        {
            new ExampleHabitLog("22-02-25", 5),
            new ExampleHabitLog("23-02-25", 2),
            new ExampleHabitLog("24-02-25", 2),
            new ExampleHabitLog("01-03-25", 4),
            new ExampleHabitLog("02-03-25", 2)
        };
    }
}
