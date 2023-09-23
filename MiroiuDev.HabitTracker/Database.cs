using Microsoft.Data.Sqlite;

namespace MiroiuDev.HabitTracker;

internal class Database : IDisposable
{
    private readonly SqliteConnection _connection;

    internal Database(string connectionString)
    {
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
    }

    internal int Execute(string commandText)
    {
        var tableCommand = _connection.CreateCommand();

        tableCommand.CommandText = commandText;

        int rowCount = tableCommand.ExecuteNonQuery();

        return rowCount;
    }

    internal int ExecuteScalar(string commandText)
    {
        var tableCommand = _connection.CreateCommand();

        tableCommand.CommandText = commandText;

        var result = tableCommand.ExecuteScalar();

        if (result is DBNull) return 0;

        return Convert.ToInt32(result);
    }

    internal void Select(string commandText, Action<Reader> getReader)
    {
        var tableCommand = _connection.CreateCommand();

        tableCommand.CommandText = commandText;

        var reader = tableCommand.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read()){
                getReader(new Reader(reader));
            }
        }
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
