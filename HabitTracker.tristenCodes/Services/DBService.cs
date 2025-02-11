namespace Services;

using Microsoft.Data.Sqlite;
using DTO;


class DBService
{
    private string _connectionString = "";
    private SqliteConnection? _connection;
    public DBService(string connectonString)
    {
        try
        {
            _connectionString = connectonString;
            _connection = new SqliteConnection(connectonString);
            OpenSqliteConnection();

            var tableExists = CheckIfTableExists();
            if (!tableExists)
            {
                CreateTable();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

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

    public void AddEntry(HabitEntry entry)
    {

    }

    public void UpdateEntry(HabitEntry oldEntry, HabitEntry newEntry)
    {

    }

    public void ViewAllEntries()
    {

    }

    private void OpenSqliteConnection()
    {
        if (_connection == null) throw new NullReferenceException("Sqlite connection is null.");
        _connection.Open();
    }

    private bool CheckIfTableExists()
    {
        var command = _connection.CreateCommand();

        command.CommandText =
        @"
        SELECT name
        FROM sqlite_master
        WHERE type = 'table' AND name = 'occurrences'
        ";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine($"Has rows: {reader.HasRows}");
            return reader.HasRows;
        }
    }
}
