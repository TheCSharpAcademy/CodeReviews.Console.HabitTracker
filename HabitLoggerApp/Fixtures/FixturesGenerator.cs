using HabitLoggerLibrary;
using HabitLoggerLibrary.Repository;

namespace HabitLoggerApp.Fixtures;

public class FixturesGenerator(IHabitsRepository habitsRepository, IHabitLogsRepository habitLogsRepository)
{
    private static readonly HabitDraft[] Drafts =
    [
        new("Running", "kilometers"),
        new("Climbing", "meters"),
        new("Push-ups", "repetitions"),
        new("Drinking water", "glasses")
    ];

    private static readonly DateTime Start = new(2020, 1, 1);
    private static readonly int Range = (DateTime.Today - Start).Days;

    public void Populate()
    {
        if (habitsRepository.GetHabitsCount() > 0) return;

        foreach (var habitDraft in Drafts)
        {
            var habit = habitsRepository.AddHabit(habitDraft);
            var i = 0;
            do
            {
                var draft = new HabitLogDraft(habit.Id, Random.Shared.Next(100), GetRandomDateOnly());
                if (habitLogsRepository.HasHabitLogByHabitIdAndDate(habit.Id, draft.HabitDate)) continue;
                habitLogsRepository.AddHabitLog(draft);
                i++;
            } while (i++ < 99);
        }
    }

    private DateOnly GetRandomDateOnly()
    {
        return DateOnly.FromDateTime(Start.AddDays(Random.Shared.Next(1000)));
    }
}