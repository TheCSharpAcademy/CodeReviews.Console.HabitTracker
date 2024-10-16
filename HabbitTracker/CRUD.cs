using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

internal class CRUD
{
    
    public bool Create(SqliteConnection connection, string name)
    {
        // query for creating table if it not exists
        var createTableQuery = @$"CREATE TABLE IF NOT EXISTS '{name}' (
    Id INTEGER PRIMARY KEY,
    Date TEXT,
    Quantity INTEGER
    )";

        using var command = new SqliteCommand(createTableQuery, connection);
        if (command.ExecuteNonQuery() >= 0)
            return true;
        return false;
    }

    public bool Update(SqliteConnection connection, string name, string date, int? repetition, bool action)
    {
        // query to check if table exists
        var query1 = $@"SELECT name FROM sqlite_master WHERE type='table' AND name=""{name}""";
        // query to select string from table by date
        var query2 = $@"SELECT Id, Date, Quantity from ""{name}"" WHERE Date = '{date}'";

        using var command1 = new SqliteCommand(query1, connection);
        using var command4 = new SqliteCommand(query2, connection);
        using var command3 = new SqliteCommand(query2, connection);
        // check if table exists
        try
        {
            var readerTest = command4.ExecuteReader();
        }
        catch (SqliteException)
        {
            // if not, propose to create one
            Console.Write(@"Habbit is not existed yet. Want to create one? 1 - yes, 2 - no: ");
            string? x = Console.ReadLine();
            Regex regex = new Regex(@"^[1-2]$");
            while (!regex.IsMatch(x))
            {
                Console.WriteLine("Once more: ");
                Console.Write(@"Habbit is not existed yet. Want to create one? 1 - yes, 2 - no: ");
                x = Console.ReadLine();
            }

            switch (x)
            {
                case "1":
                    Create(connection, name);
                    return true;
                    break;
                case "2":
                    return false;
                    break;
            }
        }
        var reader = command3.ExecuteReader();
        // if user has chosen delete action and there is no existing record
        if (!reader.HasRows)
        {
            if (!action)
            {
                Console.WriteLine("No data.");
                return false;
            }
            // No existing record for this date, insert a new row
            var insertTableQuery = $@"INSERT INTO [{name}] (
            Id,
            Date,
            Quantity
        )
        VALUES (
            (SELECT MAX(Id) + 1 FROM [{name}]),  
            '{date}',                         
            {repetition}
        )";

            using var command2 = new SqliteCommand(insertTableQuery, connection);
            command2.ExecuteNonQuery();
            return true;
        }

        // If a record exists, update it
        while (reader.Read())
        {
            try
            {
                // if user has chosen delete action - delete it
                if (!action)
                {
                    new SqliteCommand($@"DELETE FROM [{name}]
                WHERE Date = '{date}'", connection).ExecuteNonQuery();
                    return true;
                }
                int quantity = Convert.ToInt32(reader.GetString(2));
                new SqliteCommand($@"UPDATE [{name}]
                SET Quantity = {quantity + repetition}
                WHERE Date = '{date}'", connection).ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SQL error: {ex.Message}");
                return false;
            }
        }

        return true;
    }


    public bool Read(SqliteConnection connection, string name)
    {
        // selecting all strings from table
        var selectTableQuery = @$"SELECT Id, Date, Quantity from [{name}]";
        using var command = new SqliteCommand(selectTableQuery, connection);
        var reader = command.ExecuteReader();
        while (reader.Read()) Console.WriteLine($"{reader.GetString(1)}, {reader.GetString(2)}");
        if (reader.HasRows)
            return true;
        return false;
    }

    public bool Delete(SqliteConnection connection, string name)
    {
        // delete whole table
        var selectDeleteQuery = $"DROP TABLE [{name}]";
        using var command = new SqliteCommand(selectDeleteQuery, connection);
        if (command.ExecuteNonQuery() >= 0)
            return true;
        return false;
    }

   public bool DbExistence(string dbString)
    {
        // check if db exists
        // this path is local - consider to change it your pc path
        string databasePath = $@"C:\Users\Alex\source\repos\Math Game\CodeReviews.Console.HabitTracker\HabbitTracker\bin\Debug\net8.0\{dbString}";  // Full path to your SQLite database

        if (File.Exists(databasePath))
        {
            Console.WriteLine("Database exists.");
            return true;
        }
        Console.WriteLine("Database does not exist.");
        return false;
        

    }
}