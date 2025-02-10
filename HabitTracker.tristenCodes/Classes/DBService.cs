namespace Classes;

using Microsoft.Data.Sqlite;


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
            var result = CheckIfTableExists();
            CreateTable();
            Console.WriteLine("Success");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }

    public void CreateTable()
    {
        var command = _connection!.CreateCommand();

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

    public void AddEntry(string userEntry)
    {
        // TODO: Add split on delimiters for entry 
        // habit name, occurrences, date

        // went jogging, 2, 02/06/2025
    }

    public void UpdateEntry(int habitId)
    {

    }

    public void UpdateEntry(int habitId, string newUserEntry)
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
            if (reader.HasRows) Console.WriteLine("Has rows");
        }
        return false;
    }
}
