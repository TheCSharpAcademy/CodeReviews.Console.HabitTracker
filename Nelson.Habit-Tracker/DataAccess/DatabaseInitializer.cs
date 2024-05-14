using System.Data.SQLite;

namespace Nelson.Habit_Tracker.DataAccess
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private const string DatabaseName = "habit_tracker.db";
        private const string ConnectionString = "Data Source=" + DatabaseName + ";Version=3;";

        public void InitializeDatabase()
        {
            // Check if the database is already initialized
            if (System.IO.File.Exists(DatabaseName))
            {
                return;
            }
            // Create a new SQLite database file if it doesn't exist    
            SQLiteConnection.CreateFile(DatabaseName);

            // Create tables
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Habits (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Name TEXT NOT NULL,
                            Quantity INTEGER
                        );";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public void InsertToDatabase(string date, string name, int quantity)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @$"
                INSERT INTO Habits(Date, Name, Quantity)
                VALUES ('{date}', '{name}', '{quantity}')";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }
    }
}