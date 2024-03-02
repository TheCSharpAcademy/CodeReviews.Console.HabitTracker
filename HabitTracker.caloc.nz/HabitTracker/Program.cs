using Microsoft.Data.Sqlite;

namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(Helpers.connectionString))
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
        Menu.GetUserInput();
    }
}