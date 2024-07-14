using Microsoft.Data.Sqlite;

internal class DatabaseContext
{
    public static SqliteConnection Connection = default!;

    public static SqliteConnection GetConnection(string connectionString = "Data Source=habit-Tracker.db")
    {
        if (Connection == null)
            Connection = new SqliteConnection(connectionString);

        return Connection;
    }
    
}