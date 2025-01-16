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
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER    
                    );
                ";
                command.ExecuteNonQuery();
            }
        }

        
    }
}
