using HabbitLogger.AndreasGuy54;
using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=habit_tracker.db";

using (SqliteConnection connection = new SqliteConnection(connectionString))
{
    connection.Open();

    SqliteCommand tableCmd = connection.CreateCommand();
    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER)";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}

Helpers.GetUserInput();