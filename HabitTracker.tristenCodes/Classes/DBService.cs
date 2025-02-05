using Microsoft.Data.Sqlite;

class DBService
{
    private string _connectionString;
    private SqliteConnection? _connection;
    public DBService(string connectonString)
    {
        this._connectionString = connectonString;
        _connection = new SqliteConnection(connectonString);
    }

    public void CreateTable()
    {
        var command = _connection.CreateCommand();
        command.CommandText =
        @"
        CREATE TABLE occurrences (
            id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            habit_name TEXT NOT NULL,
            occurrences INTEGER NOT NULL,
            date INTEGER NOT NULL
            ); 
    ";

        command.ExecuteNonQuery();
    }
}