using Microsoft.Data.Sqlite;

namespace HabitLogger.data_and_access;

/// <summary>
/// Represents a class that manages database operations for HabitLogger.
/// </summary>
public partial class DatabaseManager
{
    /// <summary>
    /// Opens a connection to the SQLite database.
    /// </summary>
    /// <returns>An instance of the <see cref="SqliteConnection"/> class.</returns>
    private SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        return connection;
    }

    /// <summary>
    /// Closes the database connection.
    /// </summary>
    /// <param name="connection">The database connection to be closed.</param>
    private void CloseConnection(SqliteConnection? connection)
    {
        if (connection == null)
        {
            return;
        }

        try
        {
            connection.Close();
            connection.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to close connection: ");
            ErrorMessagePrinter(e);
        }
    }
}