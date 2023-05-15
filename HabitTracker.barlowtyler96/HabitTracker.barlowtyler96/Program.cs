using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    internal class Program
    {
        public static string ConnectionString { get; private set; } = @"Data Source=habit-Tracker.db";

        static void Main(string[] args)
        {
            //Using statement calls Dispose() after the using block is left.
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand(); //Creates a command to send to DB

                tableCmd.CommandText = //Defines the command string to create a table
                    @"CREATE TABLE IF NOT EXISTS habits
                        (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Activity TEXT,
                        Unit TEXT,
                        Amount INTEGER
                        )";

                tableCmd.ExecuteNonQuery();//Executes the command without returning a value. Only telling it to create a table.

                connection.Close(); //Closes the connection with the DB
            }
            MainMenu.GetUserInput();
        }
    }
}