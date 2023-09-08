using Spectre.Console;
using System.Data.SQLite;

namespace Habit_Logger.Kakurokan
{
    internal class DataAcces
    {
        public SQLiteConnection MyConnection { get; private set; }

        public DataAcces()
        {
            MyConnection = new("Data Source=habit_logger.db;Version=3;");
            if (!File.Exists("./habit_logger.db"))
            {
                SQLiteConnection.CreateFile("habit_logger.db");
                AnsiConsole.Markup("[green]Database created[/]");
            }
            using (var conn = new SQLiteConnection(MyConnection))
            {
                conn.Open();
                var command = conn.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Habits (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Name TEXT NOT NULL,
	Date TEXT NOT NULL,
	Quantity INTEGER NOT NULL,
	Unit TEXT NOT NULL)";
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

    }
}
