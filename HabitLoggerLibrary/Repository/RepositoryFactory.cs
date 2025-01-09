using HabitLoggerLibrary.DbManager;

namespace HabitLoggerLibrary.Repository;

public class RepositoryFactory(IDatabaseManager databaseManager)
{
    public IHabitsRepository CreateHabitsRepository()
    {
        return new HabitsRepository(databaseManager.GetConnection());
    }

    public IHabitLogsRepository CreateHabitLogsRepository()
    {
        return new HabitLogsRepository(databaseManager.GetConnection());
    }
}