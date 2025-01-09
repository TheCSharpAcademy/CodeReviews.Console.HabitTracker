namespace HabitLoggerLibrary.Ui.Habits;

public static class HabitsRenderer
{
    public static void Render(List<Habit> habits)
    {
        foreach (var habit in habits)
            Console.WriteLine($"{habit.Id}: {habit.HabitName}; {habit.UnitOfMeasure};");
    }
}