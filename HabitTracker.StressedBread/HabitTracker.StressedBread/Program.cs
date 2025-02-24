using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=HabitTracker.db";

using (SqliteConnection connection = new SqliteConnection(connectionString))
{
    connection.Open();

    SqliteCommand tableCommand = connection.CreateCommand();

    tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water ( 
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                                )";

    tableCommand.ExecuteNonQuery();
    connection.Close();
}