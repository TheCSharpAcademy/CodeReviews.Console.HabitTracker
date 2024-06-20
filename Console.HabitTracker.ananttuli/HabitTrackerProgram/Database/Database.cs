using Microsoft.Data.Sqlite;

namespace HabitTrackerProgram.Database
{
    public class Database
    {
        private static SqliteConnection? _connection;

        public static void Init()
        {
            try
            {
                _connection = new SqliteConnection("Data Source=database.db");

                HabitRepository.CreateTable();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"ERROR CODE X001: Could not initialise application. {ex.Message}");
                _connection = null;
                Environment.Exit(1);
            }

        }

        public static SqliteConnection GetConnection()
        {
            if (_connection == null)
            {
                throw new Exception("Database connection missing.");
            }

            return _connection;
        }
    }
}