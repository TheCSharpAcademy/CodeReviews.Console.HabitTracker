using System.Globalization;
using HabitTracker.wkktoria.Models;
using Microsoft.Data.Sqlite;

namespace HabitTracker.wkktoria;

public class Database
{
    private readonly string _connectionString;
    private readonly CultureInfo _cultureInfo;

    public Database(string connectionString, CultureInfo cultureInfo)
    {
        _connectionString = connectionString;
        _cultureInfo = cultureInfo;
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = """
                               CREATE TABLE IF NOT EXISTS habits (
                                   Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                   Date DATETIME,
                                   Name TEXT,
                                   Unit TEXT,
                                   Quantity INTEGER
                               )
                               """;
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    public void Insert()
    {
        var date = Helpers.GetDateInput(_cultureInfo);
        var name = Helpers.GetStringInput("Enter the habit: ");
        name = Helpers.PareString(name);
        var unit = Helpers.GetStringInput("Enter the unit: ");
        unit = Helpers.PareString(unit);
        var quantity = Helpers.GetNumberInput("Enter the quantity: ");

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $"INSERT INTO habits(date, name, unit, quantity) VALUES('{date}', '{name}', '{unit}', {quantity})";
        tableCmd.ExecuteNonQuery();

        Console.WriteLine("Record has been inserted.");

        connection.Close();
    }

    public void PrintAllRecords()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM habits ORDER BY Date ASC";

        List<Habit> tableData = new();
        var reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
            while (reader.Read())
                tableData.Add(new Habit
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", _cultureInfo),
                    Name = reader.GetString(2),
                    Unit = reader.GetString(3),
                    Quantity = reader.GetInt32(4)
                });

        Console.WriteLine("All records:");
        if (tableData.Any())
            foreach (var record in tableData)
                Console.WriteLine(
                    $"- id: {record.Id}; {record.Date:dd MMM yyyy}; {record.Name.ToUpper()}: {record.Quantity} {record.Unit}");
        else
            Console.WriteLine("No records found.");

        connection.Close();
    }

    public void PrintSelectedRecords()
    {
        var name = Helpers.GetStringInput("Enter the habit: ");
        name = Helpers.PareString(name);

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT * FROM habits WHERE Name='{name}' ORDER BY Date ASC";

        List<Habit> tableData = new();
        var reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
            while (reader.Read())
                tableData.Add(new Habit
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", _cultureInfo),
                    Name = reader.GetString(2),
                    Unit = reader.GetString(3),
                    Quantity = reader.GetInt32(4)
                });

        Console.WriteLine("Records:");
        if (tableData.Any())
            foreach (var record in tableData)
                Console.WriteLine(
                    $"- id: {record.Id}; {record.Date:dd MMM yyyy}; {record.Name.ToUpper()}: {record.Quantity} {record.Unit}");
        else
            Console.WriteLine("No records found.");

        connection.Close();
    }

    public void Update()
    {
        PrintAllRecords();

        var recordId = Helpers.GetNumberInput("Enter id: ");

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id={recordId})";

        var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"Record with id '{recordId}' doesn't exist.");
        }
        else
        {
            var date = Helpers.GetDateInput(_cultureInfo);
            var name = Helpers.GetStringInput("Enter the habit: ");
            name = Helpers.PareString(name);
            var unit = Helpers.GetStringInput("Enter the unit: ");
            unit = Helpers.PareString(unit);
            var quantity = Helpers.GetNumberInput("Enter the quantity: ");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"UPDATE habits SET date='{date}', name='{name}', unit='{unit}', quantity={quantity} WHERE Id={recordId}";
            tableCmd.ExecuteNonQuery();

            Console.WriteLine("Record has been updated.");
        }

        connection.Close();
    }

    public void Delete()
    {
        PrintAllRecords();

        var recordId = Helpers.GetNumberInput("Enter id: ");

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE FROM habits WHERE Id={recordId}";

        var rowCount = tableCmd.ExecuteNonQuery();

        Console.WriteLine(rowCount == 0 ? $"Record with id '{recordId}' doesn't exist." : "Record has been deleted.");

        connection.Close();
    }

    public void Report()
    {
        var year = Helpers.GetNumberInput("Enter a year: ");

        var name = Helpers.GetStringInput("Enter the habit: ");
        name = Helpers.PareString(name);

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText =
            $"SELECT EXISTS(SELECT 1 FROM habits WHERE Name='{name}' AND strftime('%Y', Date)='{year}')";

        var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"No habit '{name}' found in selected year.");
        }
        else
        {
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT Name, COUNT(Name), SUM(Quantity), Unit FROM habits WHERE Name='{name}' AND strftime('%Y', Date)='{year}'";

            var reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    Console.WriteLine(
                        $"You did '{reader.GetString(0)} {reader.GetString(1)}' times, with total amount of {reader.GetInt32(2)} {reader.GetString(3)}.");
        }

        connection.Close();
    }
}