using System.Data.SQLite;

namespace HabitTrackerConsole.Database;

/// <summary>
/// DatabaseContext manages the database connection settings and provides a method to establish a new database connection.
/// It encapsulates the connection logic and ensures that any connection opened through this class is correctly configured.
/// </summary>
public class DatabaseContext
{
    private readonly string dbConnectionString;

    /// <summary>
    /// Initializes a new instance of the DatabaseContext with the specified database path.
    /// </summary>
    /// <param name="dbPath">The file path of the SQLite database.</param>
    public DatabaseContext(string dbPath)
    {
        dbConnectionString = $"Data Source={dbPath};Version=3;";
    }

    /// <summary>
    /// Creates and opens a new database connection using the configured connection string.
    /// Handles exceptions by logging errors and rethrowing them to be handled by the caller.
    /// </summary>
    /// <returns>An opened SQLiteConnection that can be used to perform database operations.</returns>
    public SQLiteConnection GetNewDatabaseConnection()
    {
        var connection = new SQLiteConnection(dbConnectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening database connection: {ex.Message}");
            connection.Dispose();
            throw;
        }
    }
}
