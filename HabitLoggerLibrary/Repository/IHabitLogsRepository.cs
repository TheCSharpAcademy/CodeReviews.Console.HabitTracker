namespace HabitLoggerLibrary.Repository;

public interface IHabitLogsRepository
{
    public const string TableName = "habit_logs";

    public List<HabitLog> GetHabitLogs();

    public HabitLog AddHabitLog(HabitLogDraft draft);

    public void UpdateHabitLog(HabitLog habitLog);

    public void DeleteHabitLogById(long id);

    public HabitLog GetHabitLogById(long id);

    public bool HasHabitLogById(long id);

    public bool HasHabitLogByHabitIdAndDate(long habitId, DateOnly date);

    public List<Statistic> GetStatistics(string period);
}