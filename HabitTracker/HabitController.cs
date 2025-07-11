using Spectre.Console;

namespace HabitTracker;

public class HabitController(string connectionString, string userName)
{
    private readonly Queries _queries = new(connectionString);

    public void InsertHabit()
    {
        var habit = AnsiConsole.Prompt(new TextPrompt<string>("What habit do you want to log?"));
        var count = AnsiConsole.Prompt(new TextPrompt<int>("How often did you do the habit?").Validate((n) => n switch
        {
            < 1 => ValidationResult.Error("Please enter a number between 0 and 100."),
            <= 100 => ValidationResult.Success(),
            > 100 => ValidationResult.Error("Please enter a number between 0 and 100."),
        }));
        var day = AnsiConsole.Prompt(
            new SelectionPrompt<DateOnly>().Title("What day do you want to log?")
                .AddChoices(new DateOnly[]
                {
                    DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-4)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                    DateOnly.FromDateTime(DateTime.Now.AddDays(-6)),
                }) // Considered making this programmatic, but the amount of complexity did not feel worth it.
        );

        _queries.InsertNewHabit(userName, habit, count, day);
        AnsiConsole.MarkupLine($"[blue]Added new habit: {habit}[/]");
        AnsiConsole.WriteLine($"You did {habit}, {count} times on day {day}");
    }

    public void SeeHabits()
    {
        var habits = _queries.RetrieveHabits(userName);
        foreach (var habit in habits)
        {
            Console.WriteLine(
                $"Id: {habit["id"]}, Habit: {habit["habit"]}, Count: {habit["count"]}, Date: {habit["date"]}");
        }
    }

    public void UpdateHabit()
    {
        var habits = _queries.RetrieveHabits(userName).ToList();
        Console.WriteLine(habits);
        var habitChoice = new SelectionPrompt<string>();
        habitChoice.Title = "What habit do you want to change?";
        foreach (var habit in habits)
        {
            habitChoice.AddChoice(
                $"Id: {habit["id"]}, Habit: {habit["habit"]}, Count: {habit["count"]}, Date: {habit["date"]}");
        }

        var selectedHabit = AnsiConsole.Prompt(habitChoice);
        var habitIndex = selectedHabit.Split(",")[0];
        var habitInt = Int32.Parse(habitIndex);

        var countChange = AnsiConsole.Prompt(new TextPrompt<int>("What new count do you want to give this habit?"));

        _queries.UpdateHabit(habitInt, countChange);
    }

    public void RemoveHabit()
    {
        var habits = _queries.RetrieveHabits(userName).ToList();
        Console.WriteLine(habits);
        var habitChoice = new SelectionPrompt<string>();
        habitChoice.Title = "What habit do you want to change?";
        foreach (var habit in habits)
        {
            habitChoice.AddChoice(
                $"{habit["id"]}, Habit: {habit["habit"]}, Count: {habit["count"]}, Date: {habit["date"]}");
        }

        var selectedHabit = AnsiConsole.Prompt(habitChoice);
        var habitIndex = selectedHabit.Split(",")[0];
        var habitInt = Int32.Parse(habitIndex);
        _queries.DeleteHabitById(habitInt);
    }
}