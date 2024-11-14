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

internal enum TrackerHabitDescription
{
    per_day,
    per_week,
    per_month,
    per_year,
    per_hour,
};

class DataSeed()
{
    internal static void CreateTestRecord()
    {
        int numberOfTestTables = 3;

        int habitsCreatedCounter = 0;

        using (var connection = new SqliteConnection(Program.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(*) name FROM sqlite_master WHERE type = 'table' AND name LIKE 'test%'";
            long testHabitCount = (long)cmd.ExecuteScalar();

            while (testHabitCount < numberOfTestTables)
            {
                Random random = new Random();
                int sequence = 0;

                string habitName = "";
                string trackedName = "";
                string? measurementType;

                bool habitNameSelected = false;
                while (!habitNameSelected)
                {
                    sequence = random.Next(0, Enum.GetNames(typeof(TrackerName)).Length);
                    habitName = "test_" + (TrackerName)sequence;

                    cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name LIKE '{habitName}'";

                    var reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        habitNameSelected = true;
                    }
                    else
                    {
                        reader.Close();
                        continue;
                    }

                    trackedName = Enum.GetName(typeof(TrackerHabitDescription), (TrackerHabitDescription)sequence);

                    reader.Close();
                }

                sequence = random.Next(0, Enum.GetNames(typeof(MeasurementType)).Length);
                measurementType = Enum.GetName(typeof(MeasurementType), (MeasurementType)sequence);

                habitsCreatedCounter++;
                cmd.CommandText = $"CREATE TABLE IF NOT EXISTS '{habitName}' (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, '{trackedName}' REAL, '{measurementType}' TEXT)";

                cmd.ExecuteNonQuery();

                sequence = random.Next(1, 2001);

                for (int i = 0; i < sequence; i++)
                {
                    string? date;
                    int? valueAchieved;

                    int day = random.Next(1, 29);
                    int month = random.Next(1, 13);
                    int preYearValue = random.Next(0, 6);
                    int year = 2024 - preYearValue;
                    date = $"{day.ToString()}/{month.ToString()}/{year.ToString()}";
                    DateTime datetime = DateTime.Parse(date, new CultureInfo("en-GB"));
                    date = datetime.ToString("dddd, dd MMMM, yyyy");

                    valueAchieved = random.Next(10, 1001);

                    cmd.CommandText = $"INSERT INTO '{habitName}'(Date, '{trackedName}') VALUES('{date}', '{valueAchieved}')";

                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = $"SELECT COUNT(*) name FROM sqlite_master WHERE type = 'table' AND name LIKE 'test%'";
                testHabitCount = (long)cmd.ExecuteScalar();
            }
            
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            switch (habitsCreatedCounter)
            {
                case 0:
                    Console.WriteLine($"Developer message: <No seed table could be created: 3 test tables already exists.>\n\n");
                    break;
                case 1:
                    Console.WriteLine($"Developer message: <1 test table have been created. To disable this option, assign false to /runDeveloperSeedTest/ variable in Program.cs class.>\n\n");
                    break;
                default:
                    Console.WriteLine($"Developer message: <{habitsCreatedCounter} additional test tables have been created. To disable this option, assign false to /runDeveloperSeedTest/ variable in Program.cs class.>\n\n");
                    break;
                
            }
            Console.ResetColor();
            connection.Close();
        }
    }
}