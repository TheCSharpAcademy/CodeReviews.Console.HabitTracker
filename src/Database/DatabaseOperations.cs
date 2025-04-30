using Microsoft.Data.Sqlite;
using Spectre.Console;
using Golvi1124.HabitLogger.src.Helpers;

namespace Golvi1124.HabitLogger.src.Database;

public class DatabaseOperations
{

    private readonly string _connectionString;

    public DatabaseOperations()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }

    public bool IsTableEmpty(string tableName)
    {
        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand command = connection.CreateCommand())
        {
            connection.Open();
            command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
            object? result = command.ExecuteScalar();
            long count = result is not null ? (long)result : 0; // Safely handle null
            return count == 0; // This means it will be true if no rows are found and false otherwise.
        }
    }


    public void WipeData()
    {
        Console.Clear();
        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand wipeCmd = connection.CreateCommand())
        {
            connection.Open();
            wipeCmd.CommandText = "DELETE FROM records; DELETE FROM habits; DELETE FROM sqlite_sequence WHERE name = 'records' OR name = 'habits';";
            wipeCmd.ExecuteNonQuery();
        }
        Console.WriteLine("All data wiped and IDs reset.");
    }


    public void AddRandomData()
    {
        HelperMethods helper = new(); // Create an instance of HelperMethods to access its methods in this class

        if (!IsTableEmpty("habits") || !IsTableEmpty("records")) // check if the tables are empty before seeding data
        {
            var wipeData = AnsiConsole.Confirm("Tables should be emptied first. Do you want to wipe all data?");
            if (wipeData)
                WipeData();
            else
                return;
        }

        // Ensuring that habits are seeded before records
        if (IsTableEmpty("habits")) SeedHabits();

        int numberOfRecords = AnsiConsole.Ask<int>("How many random records do you want to add?");

        List<string> dates = helper.GenerateRandomDates(numberOfRecords);
        List<int> quantities = helper.GenerateRandomQuantities(numberOfRecords, 0, 2000);

        using (SqliteConnection connection = new(_connectionString))
        {
            connection.Open();

            for (int i = 0; i < numberOfRecords; i++)
            {
                var insertSql = $"INSERT INTO records (Date, Quantity, HabitId) VALUES ('{dates[i]}', {quantities[i]}, {helper.GetRandomHabitId()});";
                var command = new SqliteCommand(insertSql, connection);
                command.ExecuteNonQuery();
            }
        }
        Console.Clear();
        Console.WriteLine($"{numberOfRecords} random records added.");
    }

    public void SeedHabits()
    {
        string[] habitNames = { "Reading", "Running", "Chocolate", "Drinking Water", "Glasses of Wine" };
        string[] habitUnits = { "Pages", "Meters", "Grams", "Mililiters", "Mililiters" };

        using (SqliteConnection connection = new(_connectionString)) 
        {
            connection.Open();

            for (int i = 0; i < habitNames.Length; i++)
            {
                var insertSql = $"INSERT INTO habits (Name, MeasurementUnit) VALUES ('{habitNames[i]}', '{habitUnits[i]}');";
                var command = new SqliteCommand(insertSql, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
