using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=habit-Tracker.db";

CreateDatabase();

void CreateDatabase()
{
	using (SqliteConnection connection = new(connectionString))
	{
		using (SqliteCommand tableCmd = connection.CreateCommand())
		{
			connection.Open();
			tableCmd.CommandText =
				@"CREATE TABLE IF NOT EXISTS walkingHabit (
					Id INTEGER PRIMARY KEY AUTOINCREMENT,
					Date TEXT,
					Quantity INTEGER
					)";
			tableCmd.ExecuteNonQuery();
		}
	}
}