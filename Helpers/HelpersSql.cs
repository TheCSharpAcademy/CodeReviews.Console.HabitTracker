using Microsoft.Data.Sqlite;
using Spectre.Console;

internal class HelpersSql
{
    internal static void ShowListOfAllRecords()
    {
        Console.Clear();
        var records = RetrieveAllHabitRecordsFromDb(orderBy: "ASC");
        if (Database.CheckIfNoRecordsAvailable(records)) return;

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Date");
        table.AddColumn("Habit");
        table.AddColumn("Value");

        foreach (var record in records)
        {
            table.AddRow(
                new Markup($"{record.Id}"),
                new Markup($"{record.Date.ToString(HelpersGeneral.DateFormat)}"),
                new Markup($"[yellow]{record.HabitName}[/]"),
                new Markup($"[green]{record.Value} {record.MeasurementUnit}[/]"));
        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"Total number of records: [green]{records.Count}[/]");

        HelpersReports.ShowSummarizedReport();
        AnsiConsole.Markup("[yellow]Press any key to continue: [/]");
        Console.ReadKey(true);
        Console.Clear();
    }

    internal static string GetHabitName()
    {
        var habitNamesList = RetrieveHabitNamesFromDb(ignoreCreateDelete: false);
        var habitNames = MakeListOfHabitNames(habitNamesList);
        var habitName = HelpersGeneral.GetChoiceFromSelectionPrompt("Choose habit:", habitNames);
        return habitName;
    }

    internal static Dictionary<string, HabitRecord> MakeRecordsMap()
    {
        var habits = RetrieveAllHabitRecordsFromDb();
        var records = MakeListOfAllRecordsWithRecordId(habits);
        var recordsMap = new Dictionary<string, HabitRecord>();
        for (int i = 0; i < records.Count; i++)
        {
            recordsMap.Add(records[i], habits[i]);
        }
        return recordsMap;
    }

    internal static List<string> MakeListOfAllRecordsWithRecordId(List<HabitRecord> data)
    {
        List<string> tableData = new();
        foreach (var record in data)
        {
            tableData.Add($"{record.Date.ToString(HelpersGeneral.DateFormat)}: [yellow]{record.HabitName}[/] " +
                $"- [green]{record.Value} {record.MeasurementUnit}[/] " +
                $"[{Console.BackgroundColor}]=> id:{record.Id}[/]");
        }
        return tableData;
    }

    internal static List<string> MakeListOfHabitNames(List<HabitName> data)
    {
        var tableData = new List<string>();
        foreach (var habitName in data)
        {
            tableData.Add(habitName.Name ?? string.Empty);
        }
        return tableData;
    }

    internal static List<HabitName> RetrieveHabitNamesFromDb(bool ignoreCreateDelete = true)
    {
        var tableData = new List<HabitName>();

        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM habit_names ORDER BY id DESC";
            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new HabitName
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            MeasurementUnit = reader.GetString(2),
                        });
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No habit names found.[/]");
                HelpersGeneral.PressAnyKeyToContinue();
            }
        }

        if (ignoreCreateDelete)
        {
            var removeCreate = tableData.FirstOrDefault(x => x.Name == Database.CreateHabitName);
            if (removeCreate != null) tableData.Remove(removeCreate);
            var removeDelete = tableData.FirstOrDefault(x => x.Name == Database.DeleteHabitName);
            if (removeDelete != null) tableData.Remove(removeDelete);
        }
        return tableData;
    }

    internal static List<HabitRecord> RetrieveAllHabitRecordsFromDb(string orderBy = "DESC")
    {
        var records = new List<HabitRecord>();
        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM habit_records ORDER BY date {orderBy}";

            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    records.Add(
                        new HabitRecord
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetDateTime(1).Date,
                            HabitName = reader.GetString(2),
                            Value = reader.GetInt32(3),
                            MeasurementUnit = reader.GetString(4),
                        });
                }
            }
        }
        return records;
    }

    internal static (int value, string measurementUnit) GetValueAndMeasurementUnit(SqliteConnection connection, string habitName)
    {
        var command = connection.CreateCommand();
        command.CommandText = "SELECT measurement_unit FROM habit_names WHERE name = @habitName";
        command.Parameters.AddWithValue("@habitName", habitName);
        var measurementUnit = command.ExecuteScalar() as string ?? "";
        var value = HelpersGeneral.GetPositiveNumberInput($"Enter a number of {measurementUnit}:");
        return (value, measurementUnit);
    }

    internal static bool ConfirmDeletion()
    {
        var confirm = HelpersGeneral.GetYesNoAnswer("[red]Are you sure?[/]");
        return confirm;
    }
}
