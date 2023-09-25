using HabbitLogger.AndreasGuy54;
using Microsoft.Data.Sqlite;

Console.WriteLine("Hello, World!");

string connectionString = @"Data Source=habit_tracker.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();

    var tableCmd = connection.CreateCommand();
    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER)";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}

Helpers.GetUserInput();