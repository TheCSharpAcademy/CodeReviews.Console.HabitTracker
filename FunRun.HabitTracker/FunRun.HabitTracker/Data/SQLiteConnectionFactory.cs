using FunRun.HabitTracker.Data.Model;
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

    public IDbConnection OneTimeSetupConnection()
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
        command.CommandText = HabitTable.SQLCreateTable();
        command.ExecuteNonQuery(); 
    }
}
