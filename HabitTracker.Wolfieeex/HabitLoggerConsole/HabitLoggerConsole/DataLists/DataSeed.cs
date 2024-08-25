using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitLoggerConsole.Models;

internal enum TrackerName
{
    going_to_gym,
    drinking_water,
    power_walking,
    sleeping,
    programming
};

internal enum TrackerHabitNamePrefix
{
    per_day,
    per_week,
    per_month,
};

class DataSeed()
{
    internal static void CreateTestRecord()
    {
        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();

            var cmd = connection.CreateCommand();

            cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE 'test%'";

            SqliteDataReader reader = cmd.ExecuteReader();

            Random random = new Random();

            if (!reader.HasRows)
            {
                string? habitName;
                string? trackedName;
                string? date;
                string? valueAchieved;

                int sequence = random.Next(0, Enum.GetNames(typeof(TrackerName)).Length - 1);
                habitName = "test_" + (TrackerName)sequence;
                
                sequence = random.Next(0, Enum.GetNames(typeof(MeasurementType)).Length - 1);
                MeasurementType measurementType = (MeasurementType)sequence;
                string measurementFullName = MeasurementUnits.MeasurementFullName[measurementType];
                sequence = random.Next(0, Enum.GetNames(typeof(TrackerHabitNamePrefix)).Length - 1);
                trackedName = $"{measurementFullName}_{(TrackerHabitNamePrefix)sequence}";

                int day = random.Next(1, 28);
                int month = random.Next(1, 12);
                int preYearValue = random.Next(0, 3);
                int year = 2024 - preYearValue;
                date = $"{day.ToString()}//{month.ToString()}//{year.ToString()}";
                DateTime datetime = DateTime.Parse(date, new CultureInfo("en-GB"));
                date = datetime.ToString("dddd, dd MMMM, yyyy");

                //sequence = random.Next()
            }
            else
            {
                Console.WriteLine($"Developer message: <Seed table could not be created: test table already exists. Application will continue as normal.");
            }
        }
    }
}