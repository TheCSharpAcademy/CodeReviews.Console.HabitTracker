using Microsoft.Data.Sqlite;

const string dbPath = "habit_tracker.db";
const string tableName = "habits";
const string checkTableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName;";
const string createTableCommand = """
                                  CREATE TABLE habits(
                                      id INTEGER PRIMARY KEY,
                                      date DATE NOT NULL,
                                      time TIME NOT NULL,
                                      habit TEXT NOT NULL,
                                      Quantity INT NOT NULL
                                  )
                                  """;


try
{
    await using var connection = new SqliteConnection($"Data Source={dbPath}");
    connection.Open();

    Console.WriteLine("Connected to SQLite database");

    if (!CheckTableExists(connection, tableName))
    {
        CreateTable(connection, tableName);
    }
    else
    {
        Console.WriteLine($"{tableName} table found.");
    }
}
catch (SqliteException ex)
{
    Console.WriteLine(ex.Message);
}

return;

static bool CheckTableExists(SqliteConnection connection, string tableName)
{
    
    using var command = new SqliteCommand(checkTableExistsQuery, connection);
    command.Parameters.AddWithValue("@tableName", tableName);
    using var reader = command.ExecuteReader();

    return reader.HasRows;
}

static void CreateTable(SqliteConnection connection, string tableName)
{
    using var command = new SqliteCommand(createTableCommand, connection);
    command.Parameters.AddWithValue("@tableName", tableName);

    command.ExecuteNonQuery();
    Console.WriteLine($"{tableName} table successfully created.");
}