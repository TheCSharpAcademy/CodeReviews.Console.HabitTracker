using Microsoft.Data.Sqlite;

namespace HabitTracker.joshluca98;

public class Database
{
    private readonly string _connectionString;

    public Database(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateDatabaseTable(string source)
    {
        using (var connection = new SqliteConnection(source))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

}
