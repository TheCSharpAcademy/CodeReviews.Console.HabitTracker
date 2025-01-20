using System.Data;
using System.Data.SQLite;

namespace FunRun.HabitTracker.Data;

public class SQLiteConnectionFactory
{
    private readonly string _connectionString;

    public SQLiteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        EnsureTableExists(connection);

        return connection;
    }

    private void EnsureTableExists(IDbConnection connection)
    {
        //TODO: Use a DataModel
        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );
        ";
        command.ExecuteNonQuery(); 
    }
}
