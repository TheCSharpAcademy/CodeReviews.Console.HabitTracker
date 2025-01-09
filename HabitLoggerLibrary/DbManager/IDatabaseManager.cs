using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary.DbManager;

public interface IDatabaseManager
{
    public void SetUp();

    public SqliteConnection GetConnection();
}