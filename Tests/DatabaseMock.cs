using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data;

internal class DatabaseMock
{
    internal static void CreateRandomRecords()
    {
        Console.Clear();
        var numberOfRecords = HelpersGeneral.GetPositiveNumberInput("Enter a number of random records to generate:");
        var mockList = MakeMockListOfHabitRecords(numberOfRecords);

        if (mockList.Count > 0)
        {
            PopulateDatabase(mockList);
            string record = (numberOfRecords == 1) ? "record" : "records";
            AnsiConsole.MarkupLine($"[green]New {numberOfRecords} {record} created successfully![/]");
            HelpersGeneral.PressAnyKeyToContinue();
        }
    }

    internal static void DeleteAllRecords()
    {
        AnsiConsole.MarkupLine($"[red]WARNING!\nYou want to delete ALL records permanently!\n" +
            $"That operation can not be undone![/]");
        if (!HelpersSql.ConfirmDeletion())
        {
            Console.Clear();
            return;
        }

        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = @"
                DELETE FROM habit_records;
                UPDATE sqlite_sequence SET seq = 0 WHERE name = 'habit_records';";
            deleteCmd.ExecuteNonQuery();
        }

        AnsiConsole.MarkupLine("\nAll records deleted successfully.");
        HelpersGeneral.PressAnyKeyToContinue();
    }

    internal static List<HabitRecord> MakeMockListOfHabitRecords(int numberOfRecords)
    {
        var habitNames = HelpersSql.RetrieveHabitNamesFromDb();
        if (habitNames.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No habit names found. Make one while creating a new record.[/]");
            HelpersGeneral.PressAnyKeyToContinue();
            return new List<HabitRecord>();
        }

        var habits = new List<HabitRecord>();
        for (int i = 0; i < numberOfRecords; i++)
        {
            var randomHabitName = habitNames[Random.Shared.Next(habitNames.Count)];
            var randomDate = DateTime.Now.AddDays(Random.Shared.Next(-365, 1));
            var randomValue = GetRandomValue(randomHabitName.MeasurementUnit ?? "");

            habits.Add(new HabitRecord
            {
                Date = randomDate,
                HabitName = randomHabitName.Name,
                Value = randomValue,
                MeasurementUnit = randomHabitName.MeasurementUnit
            });
        }
        habits = habits.OrderBy(h => h.Date).ToList();
        return habits;
    }

    internal static int GetRandomValue(string unit)
    {
        return unit switch
        {
            "minutes" => Random.Shared.Next(1, 61),
            "hours" => Random.Shared.Next(1, 13),
            "times" => Random.Shared.Next(1, 3),
            "pages" => Random.Shared.Next(1, 101),
            "kilometers" => Random.Shared.Next(1, 6),
            "glasses" => Random.Shared.Next(1, 11),
            _ => Random.Shared.Next(1, 11)
        };
    }

    internal static void PopulateDatabase(List<HabitRecord> records)
    {
        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $@"
                        INSERT INTO habit_records (date, habit_name, value, measurement_unit)
                        VALUES(@date, @habitName, @value, @measurementUnit)";
                    tableCmd.Parameters.Add("@date", (SqliteType)DbType.DateTime);
                    tableCmd.Parameters.Add("@habitName", (SqliteType)DbType.String);
                    tableCmd.Parameters.Add("@value", (SqliteType)DbType.Int64);
                    tableCmd.Parameters.Add("@measurementUnit", (SqliteType)DbType.String);

                    foreach (var record in records)
                    {
                        tableCmd.Parameters["@date"].Value = record.Date;
                        tableCmd.Parameters["@habitName"].Value = record.HabitName ?? string.Empty;
                        tableCmd.Parameters["@value"].Value = record.Value;
                        tableCmd.Parameters["@measurementUnit"].Value = record.MeasurementUnit ?? string.Empty;
                        tableCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    AnsiConsole.MarkupLine("[red]Failed to populate DB with mock records![/]");
                    AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
                    HelpersGeneral.PressAnyKeyToContinue();
                }
            }
        }
    }
}
