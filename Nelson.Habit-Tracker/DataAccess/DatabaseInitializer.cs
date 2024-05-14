using System.Data.SQLite;
using Nelson.Habit_Tracker.Models;

namespace Nelson.Habit_Tracker.DataAccess
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private const string DatabaseName = "habit_tracker.db";
        private const string ConnectionString = "Data Source=" + DatabaseName + ";Version=3;";
        readonly List<Habit> databases = [];

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
                            Name TEXT,
                            Measurement TEXT,
                            Quantity INTEGER
                        );";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public void GetFromDatabase()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @"SELECT * FROM Habits";

            using var command = new SQLiteCommand(createTableQuery, connection);

            // Read the rows from the database
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var habit = new Habit
                {
                    Id = reader.GetInt32(0),
                    Date = reader.GetDateTime(1),
                    Name = reader.GetString(2),
                    Measurement = reader.GetString(3),
                    Quantity = reader.GetInt32(4)
                };
                databases.Add(habit);
            }
        }

        public void InsertToDatabase(DateTime date, string name, string measure, int quantity)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @$"
                INSERT INTO Habits(Date, Name, Measurement, Quantity)
                VALUES ('{date}', '{name}', '{measure}', {quantity})";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public void UpdateToDatabase(DateTime date, string name, string measure, int quantity)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromDatabase(DateTime date, string name, string measure, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}