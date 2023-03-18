
using LucianoNicolasArrieta.HabitTracker;
using System.Data.SQLite;

namespace habit_tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database();

            database.myConnection.Open();

            string createTableQuery = 
                @"CREATE TABLE IF NOT EXISTS habits (
                    Habit_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Description TEXT NOT NULL, 
                    Date TEXT,
                    Quantity INTEGER
                    )";

            SQLiteCommand command = new SQLiteCommand(createTableQuery, database.myConnection);
            command.ExecuteNonQuery();

            database.myConnection.Close();

        }
    }
}