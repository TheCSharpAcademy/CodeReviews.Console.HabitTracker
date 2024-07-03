// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Extensions.IDataReaderExtensions
// --------------------------------------------------------------------------------------------------
// Extensions for System.Data.IDataReader.
// Notably allowing retrieval of a value via column name (ordinal) rather than index.
// --------------------------------------------------------------------------------------------------
using System.Data;

namespace HabitTracker.Data.Extensions;

public static class IDataReaderExtensions
{
    public static bool GetBoolean(this IDataReader reader, string columnName)
    {
        return reader.GetBoolean(reader.GetOrdinal(columnName));
    }

    public static DateTime GetDateTime(this IDataReader reader, string columnName)
    {
        return reader.GetDateTime(reader.GetOrdinal(columnName));
    }

    public static int GetInt32(this IDataReader reader, string columnName)
    {
        return reader.GetInt32(reader.GetOrdinal(columnName));
    }

    public static string GetString(this IDataReader reader, string columnName)
    {
        return reader.GetString(reader.GetOrdinal(columnName));
    }
}
