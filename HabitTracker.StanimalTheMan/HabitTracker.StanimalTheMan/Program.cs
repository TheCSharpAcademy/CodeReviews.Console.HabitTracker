// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;

namespace HabitTracker.StanimalTheMan;

class Program
{
	static string connectionString = "Data Source=habit-tracker.db;";
	static void Main()
	{
		// Check if the database exists
		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();
			var command = connection.CreateCommand();

			command.CommandText =
				@"CREATE TABLE IF NOT EXISTS run_distance_log (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Date TEXT,
					Distance INTEGER
					)";

			command.ExecuteNonQuery();

			UserInterface.ShowMenu(connectionString);
		}
	}
}