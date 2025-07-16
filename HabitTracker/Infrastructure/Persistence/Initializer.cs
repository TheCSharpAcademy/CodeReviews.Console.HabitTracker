using Microsoft.Data.Sqlite;

namespace HabitTracker.Infrastructure.Persistence;

public class Initializer(string connectionString)
{

    public void Initialize()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var enforceForeignKeys = new SqliteCommand("PRAGMA foreign_keys = ON", connection);
            enforceForeignKeys.ExecuteNonQuery();
            CreateHabitsTable(connection);
            CreateOccurrencesTable(connection);
        }
    }

    private int CreateHabitsTable(SqliteConnection connection)
    {
        string query = "CREATE TABLE IF NOT EXISTS habits" +
                       "(id INTEGER PRIMARY KEY," +
                       "name VARCHAR(64) NOT NULL," +
                       "unit VARCHAR(64) NOT NULL)";
        var command = new SqliteCommand(query, connection);
        return command.ExecuteNonQuery();
    }

    private int CreateOccurrencesTable(SqliteConnection connection)
    {
        string query = "CREATE TABLE IF NOT EXISTS occurrences" +
                       "(id INTEGER PRIMARY KEY," +
                       "date TEXT NOT NULL," +
                       "habit_id INTEGER NOT NULL," +
                       "FOREIGN KEY(habit_id) REFERENCES habits(id) ON DELETE CASCADE)";
        var command = new SqliteCommand(query, connection);
        return command.ExecuteNonQuery();
    }
}