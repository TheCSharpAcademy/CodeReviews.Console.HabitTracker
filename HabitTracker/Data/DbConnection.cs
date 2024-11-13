using Microsoft.Data.Sqlite;

namespace HabitTracker.Data
{
    public class DbConnection
    {
        private static SqliteConnection _connection;

        private DbConnection() { }

        public static SqliteConnection GetConnection()
        {
            if (_connection == null)
            {
                const string dbPath = "testDb.db";
                _connection = new SqliteConnection($"Data Source={dbPath}");
                _connection.Open();
                Console.WriteLine("The database has been connected.");
               
                DbConnection.CreateTable("Habits");
                DbConnection.CreateTable("Records");

                _connection.Close();
            }
            return _connection;
        }

        public static void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                Console.WriteLine("The database connection has been closed.");
            }
        }

        public static void CreateTable(string name)
        {
            try
            {
                var command = _connection.CreateCommand();
                if (name == "Habits")
                {
                    command.CommandText =
                    @"
                        CREATE TABLE IF NOT EXISTS Habits (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,                    
                            Frequency INTEGER NOT NULL,                    
                            TimesPerPeriod INTEGER NOT NULL,                    
                            StartDate TEXT NOT NULL                    
                        )
                    ";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText =
                    @"
                        CREATE TABLE IF NOT EXISTS Records (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,                    
                            HabitDate TEXT NOT NULL,
                            HabitId INTEGER,
                            FOREIGN KEY (HabitId) REFERENCES Habits(Id) ON DELETE CASCADE
                        )
                    ";
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }

        }
    }
}
