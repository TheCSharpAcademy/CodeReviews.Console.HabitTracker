using Microsoft.Data.Sqlite;

// TODO: Abstract all of this logic into a DatabaseManager.cs class

string dbPath = "habits.db";
string connectionString = $"Data Source={dbPath}";

bool dbExists = File.Exists(dbPath);

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();

    if (!dbExists)
    {
        Console.WriteLine("Database not found. Creating a new one...");

        var createTable = @"
                    CREATE TABLE IF NOT EXISTS habits (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT NOT NULL,
                        completed INTEGER DEFAULT 0,
                        created_at TEXT DEFAULT CURRENT_TIMESTAMP
                    );";

        using (var command = new SqliteCommand(createTable, connection))
        {
            command.ExecuteNonQuery();
        }

        Console.WriteLine("Database created successfully.");
    }
    else
    {
        Console.WriteLine("Database found. Proceeding with normal operation.");
    }
}