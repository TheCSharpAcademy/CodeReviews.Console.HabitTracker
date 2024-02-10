namespace HabitLogger.data_and_access;

/// <summary>
/// Class for managing database operations.
/// </summary>
public partial class DatabaseManager(string connectionString)
{
    /// The `_connectionString` variable represents the connection string used to establish a connection with a database.
    /// It is used in the following classes:
    /// - `DatabaseManager.Base.cs`: In the `CreateDatabase()` method, the `_connectionString` variable is used to open a connection to the database.
    /// - `DatabaseManager.ConnectionHandler.cs`: In the `OpenConnection()` method, the `_connectionString` variable is passed as a parameter when creating a new `SqliteConnection`.
    /// @type {string}
    /// /
    private readonly string _connectionString = connectionString;

    /// <summary>
    /// Creates the database and necessary tables if they don't already exist.
    /// </summary>
    /// <remarks>
    /// This method should be called when the application starts for the first time to ensure that the database and tables are created.
    /// </remarks>
    public void CreateDatabase()
    {
        using var connection = OpenConnection();
        try
        {
            using var tableCommand = connection.CreateCommand();
            tableCommand.CommandText =
                """
                CREATE TABLE IF NOT EXISTS records (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Date DATE,
                                    Quantity INTEGER,
                                    HabitId INTEGER,
                                    FOREIGN KEY (HabitId) REFERENCES habits(Id) ON DELETE CASCADE
                                    )
                """;
            tableCommand.ExecuteNonQuery();
            
            tableCommand.CommandText = 
                """
                CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Unit TEXT
                )
                """;
            tableCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to create database: ");
            ErrorMessagePrinter(e);
        }
        finally
        {
            CloseConnection(connection);
        }
        
        #if DEBUG
        SeedData();
        #endif
    }
}