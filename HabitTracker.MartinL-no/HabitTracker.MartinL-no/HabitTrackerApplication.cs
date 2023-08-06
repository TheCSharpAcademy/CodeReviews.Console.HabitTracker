namespace HabitTracker.MartinL_no;

internal class HabitTrackerApplication
{
    private readonly HabitService _service;

    public HabitTrackerApplication(HabitService service)
    {
        _service = service;
    }

    internal void Execute()
    {
        ShowMenuOptions();

        Console.Write("Your choice: ");
        var op = Console.ReadLine();

        switch (op)
        {
            case "v":
                ViewAllHabits();
                break;
            case "a":
                AddHabit();
                break;
        }
    }

    private void ViewAllHabits()
    {
        var habits = _service.GetAllHabits();

        Console.WriteLine("| Habit           | Date     | Count |");
        foreach (var habit in habits)
        {
            foreach (var date in habit.Dates)
            {
                Console.WriteLine($"  {habit.Name.PadRight(18)}{date.Date}{date.Count.ToString().PadLeft(4)}");

            }
        }
    }

    private void AddHabit()
    {
        throw new NotImplementedException();
    }

    private void ShowMenuOptions()
    {
        Console.WriteLine("Welcome to the Habit Tracker app!");
        Console.WriteLine("---------------------------------\n");

        Console.WriteLine("""
            Select an option:
            v - View all habits
            a - add habit

            """);
    }
}