using Microsoft.Data.Sqlite;

namespace DotNETConsole.HabitTracker.DB;

public class DbConnection
{
    public SqliteConnection Connection { get; } = new SqliteConnection($"Data Source=data.sqlite");
    
    public void ConnectionStatus()
    {
        try
        {
            Connection.Open();
            Console.WriteLine("Database connection successful");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to open database");
            Console.WriteLine(e.Message);
        }
    }
}