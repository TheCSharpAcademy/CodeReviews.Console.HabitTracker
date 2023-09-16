using Microsoft.Data.Sqlite;

namespace MiroiuDev.HabitTracker;
internal class Reader
{
    private readonly SqliteDataReader _reader;
    internal Reader(SqliteDataReader reader)
    {
        _reader = reader;
    }

    internal int GetInt32(int ordinal) => _reader.GetInt32(ordinal);
    internal string GetString(int ordinal) => _reader.GetString(ordinal);
}
