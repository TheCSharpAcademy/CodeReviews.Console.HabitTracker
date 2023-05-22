using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=habitTracker.db";
using (var connection = new SqliteConnection(connectionString))
{
	connection.Open();
	var command = connection.CreateCommand();

	command.CommandText = "";
	command.ExecuteNonQuery();
	connection.Close();
}