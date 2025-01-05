using Microsoft.Data.Sqlite;

string connectionString = "Data Source=habbit-Tracker.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText =
        @"CREATE TABLE IF NOT EXISTS water_tracker (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";
}