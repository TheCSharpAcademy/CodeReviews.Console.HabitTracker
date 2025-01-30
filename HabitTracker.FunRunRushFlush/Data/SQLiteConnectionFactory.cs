using HabitTracker.FunRunRushFlush.Data.Model;
using System.Data;
using System.Data.SQLite;

namespace HabitTracker.FunRunRushFlush.Data;

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
        using var command = connection.CreateCommand();
        command.CommandText = HabitTable.SqlCreateTable();
        command.ExecuteNonQuery(); 
    }
}
