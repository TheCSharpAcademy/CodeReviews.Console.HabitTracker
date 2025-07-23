using HabitTrackerLibrary;
using HabitTrackerLibrary.DataAccess;

namespace HabitTracker
{
    internal class Program
    {
        private static string connectionString = @"Data Source=HabitTracker.db";
        private static SqliteDataAccess db = new SqliteDataAccess(connectionString);

        static void Main(string[] args)
        {
            var sqlData = new SqlData(db, connectionString);
            DBInitializationData.InitializeTables(db, sqlData);
            var consoleUI = new ConsoleUI(sqlData);
            consoleUI.RunApplication();
        }
    }
}