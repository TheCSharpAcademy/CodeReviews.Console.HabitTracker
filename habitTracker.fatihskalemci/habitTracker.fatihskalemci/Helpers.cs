using habitTracker.fatihskalemci.Models;

namespace habitTracker.fatihskalemci;

internal class Helpers
{
    static internal HabitEntry GenerateRandomEntry()
    {
        List<Habit> habits = new()
        {
            new Habit(){ HabitName = "Running", Unit = "KM" },
            new Habit(){ HabitName = "Water Drinking", Unit = "Glass" },
            new Habit(){ HabitName = "Reading", Unit = "Pages" },
        };

        Random rand = new();
        int habitIndex = rand.Next(habits.Count);
        Habit habit = habits[habitIndex];
        int quantity = rand.Next(12);
        DateTime date = GenerateRandomDay();

        return new HabitEntry
        {
            Quantity = quantity,
            Date = date,
            Unit = habit.Unit,
            HabitName = habit.HabitName
        };
    }

    static internal DateTime GenerateRandomDay()
    {
        DateTime start = DateTime.Now.AddYears(-2);
        Random gen = new();
        int range = ((TimeSpan)(DateTime.Today - start)).Days;
        return start.AddDays(gen.Next(range));
    }
}
