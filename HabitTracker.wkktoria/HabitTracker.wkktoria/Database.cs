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

        connection.Close();

        Console.WriteLine("Record has been inserted.");
    }

    private List<Habit> GetAllRecords()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM habits";

        List<Habit> tableData = new();
        var reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
            while (reader.Read())
                tableData.Add(new Habit
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", _cultureInfo),
                    Name = reader.GetString(2),
                    Unit = reader.GetString(3),
                    Quantity = reader.GetInt32(4)
                });


        connection.Close();

        return tableData;
    }

    public void PrintAllRecords()
    {
        var allRecords = GetAllRecords();

        Console.WriteLine("All records:");
        if (allRecords.Any())
            foreach (var record in allRecords)
                Console.WriteLine(
                    $"- id: {record.Id}; {record.Date:dd MMM yyyy}; {record.Name.ToUpper()}: {record.Quantity} {record.Unit}");
        else
            Console.WriteLine("No records found.");
    }

    public void PrintSelectedRecords()
    {
        var allRecords = GetAllRecords();
        var name = Helpers.GetStringInput("Enter the habit: ");
        name = Helpers.PareString(name);

        Console.WriteLine("Records:");
        if (allRecords.Any(record => record.Name == name))
            foreach (var record in allRecords.Where(record => record.Name == name))
                Console.WriteLine(
                    $"- id: {record.Id}; {record.Date:dd MMM yyyy}; {record.Name.ToUpper()}: {record.Quantity} {record.Unit}");
        else
            Console.WriteLine("No records found.");
    }

    public void Update()
    {
        GetAllRecords();

        var recordId = Helpers.GetNumberInput("Enter id: ");

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id={recordId})";

        var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"Record with id '{recordId}' doesn't exist.");
            connection.Close();
            Update();
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

            connection.Close();

            Console.WriteLine("Record has been updated.");
        }
    }

    public void Delete()
    {
        GetAllRecords();

        var recordId = Helpers.GetNumberInput("Enter id: ");

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE FROM habits WHERE Id={recordId}";

        var rowCount = tableCmd.ExecuteNonQuery();

        switch (rowCount)
        {
            case 0:
                Console.WriteLine($"Record with id '{recordId}' doesn't exist.");
                Delete();
                break;
            default:
                Console.WriteLine("Record has been deleted.");
                break;
        }

        connection.Close();
    }

    private List<Habit> GetRecordsData(List<Habit> records, string habit, string unit)
    {
        return records.Where(record => record.Name == habit && record.Unit.Contains(unit)).ToList();
    }

    public void Report()
    {
        var allRecords = GetAllRecords();

        var year = Helpers.GetNumberInput("Enter a year: ");
        var selectedYearRecords = allRecords.Where(record => record.Date.Year == year);

        var yearRecords = selectedYearRecords.ToList();
        if (yearRecords.Any())
        {
            var runningData = GetRecordsData(yearRecords, "running", "km");
            if (runningData.Any())
            {
                var totalKms = runningData.Sum(record => record.Quantity);
                Console.WriteLine($"You ran {runningData.Count} times, with total distance of {totalKms} kilometers.");
            }
            else
            {
                Console.WriteLine("Not found 'running' habit with valid unit in database.");
            }

            var drinkingWaterData = GetRecordsData(yearRecords, "drinking water", "glass");
            if (drinkingWaterData.Any())
            {
                var totalGlasses = drinkingWaterData.Sum(record => record.Quantity);

                Console.WriteLine($"You drank {totalGlasses} glasses of water.");
            }
            else
            {
                Console.WriteLine("Not found 'drinking water' habit with valid unit in database.");
            }

            var readingData = GetRecordsData(yearRecords, "reading", "page");
            if (readingData.Any())
            {
                var totalPages = readingData.Sum(record => record.Quantity);

                Console.WriteLine($"You read {totalPages} pages of books.");
            }
            else
            {
                Console.WriteLine("Not found 'reading' habit with valid unit in database.");
            }
        }
        else
        {
            Console.WriteLine("No records in selected year found.");
        }
    }
}