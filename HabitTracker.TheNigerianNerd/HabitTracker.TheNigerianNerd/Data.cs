using Microsoft.Data.Sqlite;

namespace HabitTracker.TheNigerianNerd;

internal class Data
{
    internal string ConnectionString { get; set; } = "";
    private readonly HabitFunctions _habitFunctions;

    public Data(string connectionString, HabitFunctions habitFunctions)
    {
        ConnectionString = connectionString;
        _habitFunctions = habitFunctions;
    }

    internal void CreateDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER,
                    HabitId INTEGER,
                    FOREIGN KEY (HabitId) REFERENCES habits(Id) ON DELETE CASCADE
                )";
                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    MeasurementUnit Text
                )";
                tableCmd.ExecuteNonQuery();
            }
        }
        SeedData();
    }

    void SeedData()
    {
        bool recorsTableEmpty = _habitFunctions.IsTableEmplty("records");
        bool habitsTableEmpty = _habitFunctions.IsTableEmplty("habits");

        if (!recorsTableEmpty || !habitsTableEmpty)
            return;

        string[] habitnames = { "Reading", "Running", "Chocolate", "Drinking Water", "Glasses of Wine" };
        string[] habitUnits = { "Pages", "Meters", "Grams", "Mililiters", "Mililiters" };
        string[] dates = _habitFunctions.GenerateRandomDates(100);
        int[] quantities = _habitFunctions.GenerateRandomQuantities(100, 0, 2000);

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            for (int i = 0; i < habitnames.Length; i++)
            {
                var insertSql = $"INSERT INTO habits (Name, MeasurementUnit) VALUES ('{habitnames[i]}', '{habitUnits[i]}');";
                var command = new SqliteCommand(insertSql, connection);

                command.ExecuteNonQuery();
            }

            for (int i = 0; i < 100; i++)
            {
                var insertSql = $"INSERT INTO records (Date, Quantity, HabitId) VALUES ('{dates[i]}', {quantities[i]}, {_habitFunctions.GetRandomHabitId()});";
                var command = new SqliteCommand(insertSql, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
