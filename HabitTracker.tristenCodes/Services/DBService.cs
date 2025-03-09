namespace HabitTracker.tristenCodes.Services;

using Microsoft.Data.Sqlite;
using HabitTracker.tristenCodes.Models;
using HabitTracker.tristenCodes.Helpers;

public class DBService
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

    public void AddEntry(Habit entry)
    {
        var command = _connection.CreateCommand();

        command.CommandText =
        @"INSERT INTO habits (habit_name, occurrences, date)
        VALUES 
        (@habit, @occurrences, @date)";

        // Add parameters safely
        command.Parameters.AddWithValue("@habit", entry.Name);
        command.Parameters.AddWithValue("@occurrences", entry.Occurences);
        command.Parameters.AddWithValue("@date", entry.Date);

        command.ExecuteNonQuery();
    }

    public void UpdateEntry(Habit oldEntry, Habit newEntry)
    {
        var command = _connection.CreateCommand();

        command.CommandText =
        @"SELECT *
        FROM habits
        WHERE habit_name = @habit";

        command.Parameters.AddWithValue("@habit", oldEntry.Name);

        var reader = command.ExecuteReader();
        if (!reader.HasRows)
        {
            Console.WriteLine("Row Not Found");
        }

        else
        {
            command = _connection.CreateCommand();
            command.CommandText =
            @"
            UPDATE habits
            SET habit_name = @newHabitName, occurrences = @newOccurrences, date = @newDate
            WHERE habit_name = @oldHabitName AND occurrences = @oldOccurrences AND date = @oldDate
            ";
            command.Parameters.AddWithValue("@newHabitName", newEntry.Name);
            command.Parameters.AddWithValue("@newOccurrences", newEntry.Occurences);
            command.Parameters.AddWithValue("@newDate", newEntry.Date);

            command.Parameters.AddWithValue("@oldHabitName", oldEntry.Name);
            command.Parameters.AddWithValue("@oldOccurrences", oldEntry.Occurences);
            command.Parameters.AddWithValue("@oldDate", oldEntry.Date);

            command.ExecuteNonQuery();
            Console.WriteLine("Success");
        }
    }

    public void DeleteEntry(int habitId)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"DELETE FROM habits WHERE id = @habitId";
        command.Parameters.AddWithValue("@habitId", habitId);
        command.ExecuteNonQuery();
    }

    public SqliteDataReader GetAllEntries()
    {
        var command = _connection.CreateCommand();

        command.CommandText = @"
        SELECT * FROM habits
        ORDER BY date DESC
        ";

        return command.ExecuteReader();
    }

    public Habit GetEntryById(int id)
    {
        var command = _connection.CreateCommand();

        command.CommandText = @"
        SELECT * FROM habits WHERE id = @id";
        command.Parameters.AddWithValue("id", id);

        var reader = command.ExecuteReader();
        if (!reader.HasRows)
        {
            throw new Exception($"No habit with id {id} found.");
        }

        return EntryHelper.GetHabitFromRow(reader);
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
