namespace habit_logger.Data
{
    using Microsoft.Data.Sqlite;
    public static class Database
    {
        private const string ConnectionString = "Data Source=habit-logger.db";

        public static SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static void InitializeDatabase()
        {
            using (var connection = GetConnection())
            {
                var command = connection.CreateCommand();

                command.CommandText = 
                    @"CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Unit TEXT NOT NULL 
                    );
                ";
                command.ExecuteNonQuery();

                command.CommandText = 
                    @"CREATE TABLE IF NOT EXISTS habit_records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES habits(Id) ON DELETE CASCADE
                    );
                ";
                command.ExecuteNonQuery();
            }
        }

        
    }
}
