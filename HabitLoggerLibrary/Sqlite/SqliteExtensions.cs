using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace HabitLoggerLibrary.Sqlite;

public static class SqliteExtensions
{
    public static long GetLastInsertRowId(this SqliteConnection connection)
    {
        var handle = connection.Handle ?? throw new NullReferenceException("The connection is not open");

        return raw.sqlite3_last_insert_rowid(handle);
    }
}