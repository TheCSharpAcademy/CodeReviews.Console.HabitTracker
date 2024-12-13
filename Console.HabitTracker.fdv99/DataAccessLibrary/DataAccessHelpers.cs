// DataBaseCreate.cs
using HabitData;
using System.Data.SQLite;

namespace DataAccessLibrary
{
    public class DataAccessHelpers
    {
        private static readonly string _databaseFile = "HabitDataBase.db";
        public static readonly string connectionString = $"Data Source ={_databaseFile};Version=3";

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
                        CREATE TABLE IF NOT EXISTS Habits (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Habit TEXT NOT NULL,
                            Date TEXT,
                            Quantity INTEGER NOT NULL
                        );";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table 'Habit' created.");
                    }
                }
                SeedDatabase();
            }
            else
            {
                Console.WriteLine("Database file already exists.");
            }
        }

        public static void SeedDatabase()
        {
            // Seed the database with 50 random samples of Habit, Date, and Quantity
            HabitModel habit = new HabitModel();
            Random random = new Random();
            string[] habits = { "walk", "run", "read", "write", "code", "relax" };
            // make a random date in MM/DD/YYYY format
            int count = 0;

            while (count < 50)
            {
                int habitIndex = random.Next(habits.Length);
                habit.Habit = habits[habitIndex];
                habit.Quantity = random.Next(1, 10);
                habit.Date = $"{random.Next(1, 12)}/{random.Next(1, 28)}/{random.Next(2000, 2024)}";

                SqliteDataAccess dataAccess = new SqliteDataAccess();
                dataAccess.InsertHabit(habit);

                count++;
            }
        }
    }
}
