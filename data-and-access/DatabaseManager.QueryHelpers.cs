using Microsoft.Data.Sqlite;

namespace HabitLogger.data_and_access;

/// <summary>
/// The <see cref="DatabaseManager"/> class provides methods to interact with a database.
/// </summary>
public partial class DatabaseManager
{
    /// <summary>
    /// Add parameters to a SqliteCommand object.
    /// </summary>
    /// <param name="command">The SqliteCommand object where the parameters will be added.</param>
    /// <param name="parameters">A dictionary of parameters where the key is the parameter name and the value is the parameter value.</param>
    private void AddParameters(SqliteCommand command, Dictionary<string, object>? parameters)
    {
        if (parameters == null)
        {
            return;
        }
        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
        }
    }

    /// <summary>
    /// Creates a new SqliteCommand object with the specified query, connection, and parameters.
    /// </summary>
    /// <param name="query">The SQL query string.</param>
    /// <param name="connection">The SqliteConnection object to use for the command.</param>
    /// <param name="parameters">A dictionary of parameter names and values to be added to the command.</param>
    /// <returns>A new SqliteCommand object.</returns>
    private SqliteCommand CreateCommand(string query, SqliteConnection connection, Dictionary<string, object>? parameters)
    {
        var command = new SqliteCommand(query, connection);

        if (parameters != null)
        {
            AddParameters(command, parameters);
        }

        return command;
    }
}