using System.Data.SQLite;

namespace LucianoNicolasArrieta.HabitTracker
{
    class Database
    {
        public SQLiteConnection myConnection;

        public Database()
        {
            myConnection = new SQLiteConnection("Data Source=habit-tracker.db");

            if (!File.Exists("./habit-tracker.db"))
            {
                SQLiteConnection.CreateFile("habit-tracker.db");
                Console.WriteLine("Database file created.");
            }


        }
    }
}
