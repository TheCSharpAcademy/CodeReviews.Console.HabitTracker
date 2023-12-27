using HabitTracker;
using Microsoft.Data.Sqlite;

using (var connection = new SqliteConnection(Helpers.connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS running (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                                )";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}

MenuFunctions.UserMenu();
