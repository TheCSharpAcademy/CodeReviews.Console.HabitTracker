using Microsoft.Data.Sqlite;
using UserInput;

namespace habit_tracker2;

class Program
{    static void Main(string[] args)
    {
        string connectionString = @"Data Source=habit-Tracker2.db";
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER

                )
                ";
            tableCommand.ExecuteNonQuery();
            connection.Close();
        }
        ClassUserInput getInput = new ClassUserInput();
        getInput.GetUserInput();
    }
}
