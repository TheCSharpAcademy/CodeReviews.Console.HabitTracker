using System.Data.SQLite;

namespace Habit_Logger.Kakurokan
{
    internal class DataAcces
    {
        public SQLiteConnection MyConnection { get; private set; }

        public DataAcces()
        {
            MyConnection = new("Data Source=habit_logger.db;Version=3;");
        }

    }
}
