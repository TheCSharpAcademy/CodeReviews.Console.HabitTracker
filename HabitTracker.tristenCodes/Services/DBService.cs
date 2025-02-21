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
            _connection.Open();

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
        CREATE TABLE habits (
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
        var command = _connection.CreateCommand();

        command.CommandText =
        @"INSERT INTO habits (habit_name, occurrences, date)
        VALUES 
        (@habit, @occurrences, @date)";

        // Add parameters safely
        command.Parameters.AddWithValue("@habit", entry.Habit);
        command.Parameters.AddWithValue("@occurrences", entry.Occurences);
        command.Parameters.AddWithValue("@date", entry.Date);

        command.ExecuteNonQuery();
    }

    public void UpdateEntry(HabitEntry oldEntry, HabitEntry newEntry)
    {

    }

    public SqliteDataReader ViewAllEntries()
    {
        var command = _connection.CreateCommand();

        command.CommandText = @"
        SELECT * FROM habits
        ORDER BY date DESC
        ";

        return command.ExecuteReader();
    }


    private bool CheckIfTableExists()
    {
        var command = _connection.CreateCommand();

        command.CommandText =
        @"
        SELECT name
        FROM sqlite_master
        WHERE type = 'table' AND name = 'habits'
        ";

        using (var reader = command.ExecuteReader())
        {
            Console.WriteLine($"Has rows: {reader.HasRows}");
            return reader.HasRows;
        }
    }
}
