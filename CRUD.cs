using System.Diagnostics.Metrics;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

internal class Crud
{

    public bool Create(SqliteConnection connection, string name)
    {
        // query for creating table if it not exists
        var createTableQuery = @$"CREATE TABLE IF NOT EXISTS '{name}' (
    Id INTEGER PRIMARY KEY,
    Date TEXT,
    Quantity INTEGER,
    Measurement TEXT
    )";
        
        using var command = new SqliteCommand(createTableQuery, connection);
        if (command.ExecuteNonQuery() >= 0)
        {
            SetMeasurement(connection, name);
            return true;
        }
        return false;
    }

    public bool Update(SqliteConnection connection, string name, string date, int? repetition, bool action)
    {
        // query to select string from table by date
        var query2 = $@"SELECT Id, Date, Quantity, Measurement from ""{name}"" WHERE Date = @Date";

        using var commandTest = new SqliteCommand(query2, connection);
        using var commandSelect = new SqliteCommand(query2, connection);

        commandSelect.Parameters.AddWithValue("@Date", date);
        commandTest.Parameters.AddWithValue("@Date", date);
        // check if table exists
        try
        {
            commandTest.ExecuteReader();
        }
        catch (SqliteException)
        {
            // if not, propose to create one
            Console.Write(@"Habit is not existed yet. Want to create one? 1 - yes, 2 - no: ");
            string? inputUserCreateHabit = Console.ReadLine();
            Regex regex = new Regex(@"^[1-2]$");
            while (!regex.IsMatch(inputUserCreateHabit))
            {
                Console.WriteLine("Once more: ");
                Console.Write(@"Habit is not existed yet. Want to create one? 1 - yes, 2 - no: ");
                inputUserCreateHabit = Console.ReadLine();
            }

            switch (inputUserCreateHabit)
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
        // get measurement from id 0 row
        var queryMeasurement = $@"SELECT Measurement from ""{name}"" WHERE Id = 0";
        using var commandMeasurement = new SqliteCommand(queryMeasurement, connection);
        var measurement = "";
        var readerMeasurement = commandMeasurement.ExecuteReader();
        while (readerMeasurement.Read())
        {
            measurement = readerMeasurement["Measurement"].ToString();
        }


        var reader = commandSelect.ExecuteReader();
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
            Quantity,
            Measurement
        )
        VALUES (
            (SELECT MAX(Id) + 1 FROM [{name}]),  
            @Date,                         
            @Repetition,
            @Measurement
        )";

            using var commandInsert = new SqliteCommand(insertTableQuery, connection);
            commandInsert.Parameters.AddWithValue("@Date", date);
            commandInsert.Parameters.AddWithValue("@Repetition", repetition);
            commandInsert.Parameters.AddWithValue("@Measurement", measurement);
            commandInsert.ExecuteNonQuery();
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
        var selectTableQuery = @$"SELECT Id, Date, Quantity, Measurement from [{name}]
                                WHERE Id != 0";
        using var command = new SqliteCommand(selectTableQuery, connection);
        var reader = command.ExecuteReader();
        while (reader.Read()) Console.WriteLine($"{reader.GetString(1)}, {reader.GetString(2)}, {reader.GetString(3)}");
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
        // this path is local - consider to change it to your pc path
        string databasePath = $@"C:\Users\Alex\source\repos\projects\CodeReviews.Console.HabitTracker\bin\Debug\net8.0\{dbString}"; 

        if (File.Exists(databasePath)) return true;
        Console.WriteLine("Database does not exist.");
        Console.WriteLine("Created seed tables - test1, test2, test3");
        return false;
    }

    public bool SetMeasurement(SqliteConnection connection, string name)
    {
        // function to create table with measurement 
        // insert one row on id 0 with empty data except measurement, from where update method will take info
        Console.Write("Please, insert measurement for your habit or leave empty if one is not necessary: ");
        string? measurement = Console.ReadLine();

        
        var commandAddMeasurement = new SqliteCommand($"INSERT INTO {name} (Id, Date, Quantity, Measurement) VALUES (@Id, @Date, @Quantity, @Measurement)", connection);

        commandAddMeasurement.Parameters.AddWithValue("@Id", 0);
        commandAddMeasurement.Parameters.AddWithValue("@Date", DBNull.Value);
        commandAddMeasurement.Parameters.AddWithValue("@Quantity", DBNull.Value);
        commandAddMeasurement.Parameters.AddWithValue("@Measurement", string.IsNullOrEmpty(measurement) ? DBNull.Value : measurement);

        try
        {
            commandAddMeasurement.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite Error: {ex.Message}");
            return false;
        }

        return true;
    }

    public bool Report(SqliteConnection connection, string name, string year)
    {
        // function to get year habit report
        var selectTableQuery = @$"SELECT Id, Date, Quantity, Measurement from [{name}]
                                WHERE Id != 0 
                                AND
                                Date LIKE '%{year}%'";
        using var command = new SqliteCommand(selectTableQuery, connection);
        var reader = command.ExecuteReader();
        int quantityAmount = 0;
        int iterator = 0;
        string measurement = " ";
        while (reader.Read())
        {
            iterator++;
            quantityAmount += Convert.ToInt32(reader.GetString(2));
            measurement = reader.GetString(3);
        }
        Console.WriteLine($" You have done {name} {iterator} times this year and achieved {quantityAmount} {measurement}");
        if (reader.HasRows)
            return true;
        return false;
    }
}