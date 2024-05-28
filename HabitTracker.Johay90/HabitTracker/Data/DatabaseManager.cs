using Microsoft.Data.Sqlite;

public class DatabaseManager
{
    private readonly string _connectionString;
    private readonly string _databasePath;
    private const string TableName = "habits";

    public DatabaseManager(string databasePath)
    {
        _databasePath = databasePath;
        _connectionString = $"Data Source={databasePath};";
    }

    public SqliteConnection GetConnection() => new(_connectionString);

    public void InitializeDatabase()
    {
        if (!File.Exists(_databasePath))
        {
            Console.WriteLine("Database does not exist. Creating database and table...");
            CreateDatabaseAndTable();
        }
        else
        {
            Console.WriteLine("Database exists. Checking table...");

            if (!TableExists())
            {
                Console.WriteLine("Database Table 'habits' does not exist. Creating table...");
                CreateTable();
            }
            else
            {
                Console.WriteLine("Database and table 'habits' are OK! Continuing...");
                Console.WriteLine(new string('-', 50));
            }
        }
    }

    private void CreateDatabaseAndTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            CreateTable();
        }
    }

    private void CreateTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS habits (
                    id INTEGER NOT NULL UNIQUE,
                    name TEXT NOT NULL,
                    measurement TEXT NOT NULL,
                    quantity INTEGER NOT NULL,
                    frequency TEXT NOT NULL,
                    date_created TEXT NOT NULL,
                    date_updated TEXT,
                    notes TEXT,
                    status TEXT NOT NULL,
                    PRIMARY KEY(id AUTOINCREMENT)
                );";

            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Table created successfully.");
        }
    }

    private bool TableExists()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            string tableExistsQuery = @"SELECT count(name) FROM sqlite_master WHERE type='table' AND name=@tableName;";

            using (var command = new SqliteCommand(tableExistsQuery, connection))
            {
                command.Parameters.AddWithValue("@tableName", TableName);

                var result = command.ExecuteScalar();
                return result != null && Convert.ToInt32(result) > 0;
            }
        }
    }
}
