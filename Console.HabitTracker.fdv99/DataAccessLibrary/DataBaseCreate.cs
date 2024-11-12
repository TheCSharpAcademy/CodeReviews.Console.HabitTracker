// DataBaseCreate.cs

using System.Data.SQLite;

namespace DataAccessLibrary
{
    public class DataBaseCreate
    {
        private static readonly string _databaseFile = "HabitDataBase.db";
        private readonly string connectionString = $"Data Source ={_databaseFile};Version=3";

        public void InitializeDatabase()
        {
            if (File.Exists(_databaseFile) == false)
            {
                SQLiteConnection.CreateFile(_databaseFile);
                Console.WriteLine("Database File Created");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Habit (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Date TEXT,
                            Quantity DOUBLE NOT NULL
                        );";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table 'Habit' created.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Database file already exists.");
            }
        }
    }
}
