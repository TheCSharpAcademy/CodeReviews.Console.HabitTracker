using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary.DbManager;

public sealed class DatabaseManagerFactory
{
    public IDatabaseManager Create(bool inMemory = false)
    {
        var connectionString = new SqliteConnectionStringBuilder();
        if (inMemory)
        {
            connectionString.Mode = SqliteOpenMode.Memory;
        }
        else
        {
            connectionString.Mode = SqliteOpenMode.ReadWriteCreate;
            connectionString.DataSource = "HabitLogger.db";
        }

        using var connection = new SqliteConnection(connectionString.ConnectionString);

        return new DatabaseManager(connection);
    }
}