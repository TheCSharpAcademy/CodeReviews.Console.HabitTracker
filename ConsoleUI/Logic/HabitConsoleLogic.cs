using ConsoleTableExt;
using Models;
using Services;

namespace ConsoleUI.Logic;


static class HabitConsoleLogic
{
    public static int UserId { get; set; }
    public static string? UserName { get; set; }


    private static HabitService _habitService = new();


    public static void DeleteHabit()
    {
        ViewAllHabits();
        int habitId = ConsoleDataReader.GetIntFromConsole("Please Enter The Id For the Habit You want to Delete");
        _habitService.DeleteHabit(habitId, UserId);
        ViewAllHabits();
    }
    public static void UpdateHabit()
    {
        ViewAllHabits();
        int habitId = ConsoleDataReader.GetIntFromConsole("Please Enter The Id For the Habit You want to Update");
        Console.Clear();
        Habit habit = _habitService.GetHabitById(habitId, UserId);
        if (habit != null)
        {
            if (habit is UnitHabit uHabit)
            {
                var uHabits = new List<UnitHabit>();
                uHabits.Add(uHabit);
                var reorderedData = uHabits
                    .Select(x => new { x.Id, x.Name, x.Type, x.Quantity, x.Measure, x.CreatedAt, x.ModifiedAt }) // Project and reorder columns
                    .ToList();
                ConsoleTableBuilder
                    .From(reorderedData)
                    .WithFormat(ConsoleTableBuilderFormat.Default)
                    .WithColumn("ID", "NAME", "TYPE", "QUANTITY", "MEASURE", "CREATED AT", "MODIFIED AT")
                    .ExportAndWriteLine();
                uHabit.Name = ConsoleDataReader.GetStringFromConsole("Enter Habit Name");
                uHabit.Type = HabitType.Unit;
                uHabit.Quantity = ConsoleDataReader.ConsoleReadRangedInt($"Enter {uHabit.Name} Quantity", 1, 100);
                uHabit.Measure = ConsoleDataReader.GetStringFromConsole("Enter Habit Subject (eg. Cup,Litter,Apple, or Cigarret)");
                if (_habitService.UpdateHabit(uHabit, UserId))
                {
                    Console.WriteLine("Habit Successfully Updated");
                    Thread.Sleep(1000);
                }
            }
            if (habit is DurationHabbit duHabit)
            {

            }
            ViewAllHabits();
        }


    }
    public static void InsertHabit()
    {
        Console.WriteLine("Habit Types");
        Console.WriteLine("[1] - Units.");
        Console.WriteLine("[2] - Duration.");
        int typeNo = ConsoleDataReader.ConsoleReadRangedInt("Enter The Number Corresponds to Desigired Type", 1, 3);
        switch (typeNo)
        {
            case 1:
                UnitHabit unihabit;

                unihabit = new UnitHabit();
                unihabit.Name = ConsoleDataReader.GetStringFromConsole("Enter Habit Name");
                unihabit.Type = HabitType.Unit;
                unihabit.Quantity = ConsoleDataReader.ConsoleReadRangedInt($"Enter {unihabit.Name} Quantity", 1, 100);
                unihabit.Measure = ConsoleDataReader.GetStringFromConsole("Enter Habit Subject (eg. Cup,Litter,Apple, or Cigarret)");
                unihabit.OwnerId = UserId;
                _habitService.AddNewHabit(unihabit, UserId);

                break;
            case 2:
                DurationHabbit DurHabit = new DurationHabbit();
                DurHabit.Name = ConsoleDataReader.GetStringFromConsole("Enter Habit Name");
                DurHabit.Type = HabitType.Duration;
                DurHabit.StartedAt = ConsoleDataReader.ConsoleReadDate("What Date You Habit Starts at (e.g., 12/31/2024 11:59 PM)");
                DurHabit.EndedAt = ConsoleDataReader.ConsoleReadDate("What Date You Habit Ends at (e.g., 12/31/2024 11:59 PM)");
                DurHabit.OwnerId = UserId;
                DurHabit.Duration = DurHabit.StartedAt - DurHabit.EndedAt;
                _habitService.AddNewHabit(DurHabit, UserId);
                break;
        }
    }

    public static void ViewAllHabits()
    {

        List<Habit>? habits = _habitService.GetAllHabit(UserId);
        List<UnitHabit> uniHabits = new();
        List<FormatedDuration> formatedDurationHabits = new();
        foreach (var habit in habits)
        {
            if (habit is UnitHabit uHabit)
            {
                uniHabits.Add(uHabit);
            }
            if (habit is DurationHabbit uDurationHabbit)
            {
                var formatedDuration = new FormatedDuration
                {
                    CreatedAt = uDurationHabbit.CreatedAt,
                    DeletedAt = uDurationHabbit.DeletedAt,
                    Duration = uDurationHabbit.Duration,
                    EndedAt = uDurationHabbit.EndedAt,
                    Id = uDurationHabbit.Id,
                    IsDeleted = uDurationHabbit.IsDeleted,
                    ModifiedAt = uDurationHabbit.ModifiedAt,
                    Name = uDurationHabbit.Name,
                    OwnerId = uDurationHabbit.OwnerId,
                    StartedAt = uDurationHabbit.StartedAt,
                };
                formatedDuration.TotalDuration = $"{(formatedDuration.Duration.TotalDays >= 1 ? (int)formatedDuration.Duration.TotalDays:0)} Days";
                formatedDuration.TotalDuration += $" {(formatedDuration.Duration.TotalHours>= 24 ? (int)formatedDuration.Duration.TotalHours%24:0)} Hours";
                formatedDurationHabits.Add(formatedDuration);
            }
        }

        var reorderedData = uniHabits
                            .Select(x => new { x.Id, x.Name, x.Type,x.Quantity,x.Measure,x.CreatedAt,x.ModifiedAt }) // Project and reorder columns
                            .ToList();
        ConsoleTableBuilder
            .From(reorderedData)
            .WithFormat(ConsoleTableBuilderFormat.Default)
            .WithColumn("ID", "NAME", "TYPE", "QUANTITY", "MEASURE", "CREATED AT", "MODIFIED AT")
            .ExportAndWriteLine();

        var reorderedData2 = formatedDurationHabits
                            .Select(x => new { x.Id, x.Name, x.Type, x.StartedAt, x.EndedAt,x.TotalDuration, x.CreatedAt, x.ModifiedAt }) // Project and reorder columns
                            .ToList();

        ConsoleTableBuilder
            .From(reorderedData2)
            .WithFormat(ConsoleTableBuilderFormat.Default)
            .WithColumn("ID", "NAME", "TYPE", "STARTED AT", "ENDED AT","DURATION", "CREATED AT", "MODIFIED AT")
            .ExportAndWriteLine(); 
    }
}
