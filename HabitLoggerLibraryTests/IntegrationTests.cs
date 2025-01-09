using HabitLoggerLibrary.DbManager;
using HabitLoggerLibrary.Sqlite;
using LibraryRepository = HabitLoggerLibrary.Repository;

namespace HabitLoggerLibraryTests;

public abstract class IntegrationTests : ConsoleTest
{
    protected IDatabaseManager DatabaseManager { get; private set; }

    [SetUp]
    public void SetUpDatabase()
    {
        DatabaseManager = new DatabaseManagerFactory().Create(true);
        DatabaseManager.GetConnection().Open();
        DatabaseManager.SetUp();
    }

    protected void PopulateDatabase()
    {
        var id = AddHabit("Running", "kilometers");
        AddHabitLog(id, 3, new DateOnly(2020, 1, 1));
        AddHabitLog(id, 4, new DateOnly(2020, 2, 1));
        AddHabitLog(id, 5, new DateOnly(2020, 3, 1));

        id = AddHabit("Swimming", "meters");
        AddHabitLog(id, 6, new DateOnly(2020, 4, 1));
        AddHabitLog(id, 7, new DateOnly(2020, 5, 1));
        AddHabitLog(id, 8, new DateOnly(2020, 6, 1));

        id = AddHabit("Drinking water", "glasses");
        AddHabitLog(id, 9, new DateOnly(2020, 7, 1));
        AddHabitLog(id, 10, new DateOnly(2020, 8, 1));
        AddHabitLog(id, 11, new DateOnly(2020, 9, 1));
    }

    private long AddHabit(string habit, string unitOfMeasure)
    {
        var command = DatabaseManager.GetConnection().CreateCommand();
        command.CommandText =
            $"INSERT INTO {LibraryRepository.IHabitsRepository.TableName} (habit, unit_of_measure) VALUES (@Name, @UnitOfMeasure)";

        command.Parameters.AddWithValue("@Name", habit);
        command.Parameters.AddWithValue("@UnitOfMeasure", unitOfMeasure);
        command.ExecuteNonQuery();

        return DatabaseManager.GetConnection().GetLastInsertRowId();
    }

    private void AddHabitLog(long habitId, int quantity, DateOnly date)
    {
        var command = DatabaseManager.GetConnection().CreateCommand();
        command.CommandText =
            $"INSERT INTO {LibraryRepository.IHabitLogsRepository.TableName} (habit_id, quantity, habit_date) VALUES (@HabitId, @Quantity, @Date)";

        command.Parameters.AddWithValue("@HabitId", habitId);
        command.Parameters.AddWithValue("@Quantity", quantity);
        command.Parameters.AddWithValue("@Date", date);
        command.ExecuteNonQuery();
    }

    protected LibraryRepository.IHabitsRepository CreateHabitsRepository()
    {
        return new LibraryRepository.HabitsRepository(DatabaseManager.GetConnection());
    }

    protected LibraryRepository.IHabitLogsRepository CreateHabitLogsRepository()
    {
        return new LibraryRepository.HabitLogsRepository(DatabaseManager.GetConnection());
    }
}