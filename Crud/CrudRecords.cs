using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data;

internal class CrudRecords
{
    internal static void CreateNewRecord()
    {
        Console.Clear();
        var (shouldExit, date, habitName) = HelpersGeneral.GetDateAndHabitName();
        if (shouldExit) return;

        try
        {
            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();
                var (value, measurementUnit) = HelpersSql.GetValueAndMeasurementUnit(connection, habitName);

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $@"INSERT INTO habit_records (date, habit_name, value, measurement_unit)
                    VALUES(@date, @habitName, @value, @measurementUnit)";
                tableCmd.Parameters.Add("@date", (SqliteType)DbType.Date).Value = date;
                tableCmd.Parameters.Add("@habitName", (SqliteType)DbType.String).Value = habitName;
                tableCmd.Parameters.Add("@value", (SqliteType)DbType.Int64).Value = value;
                tableCmd.Parameters.Add("@measurementUnit", (SqliteType)DbType.String).Value = measurementUnit;
                tableCmd.ExecuteNonQuery();

                AnsiConsole.MarkupLine($"[green]A new record created successfully![/]");
                HelpersGeneral.DisplayOneRecord(date, habitName, value, measurementUnit);
            }
        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine("[red]Failed to add a record to DB.[/]");
            AnsiConsole.MarkupLine($"Details: [yellow]{ex.Message}[/]");
            HelpersGeneral.PressAnyKeyToContinue();
        }
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        var dictionaryOfHabits = HelpersSql.MakeRecordsMap();
        if (Database.CheckIfNoRecordsAvailable(dictionaryOfHabits.Keys)) return;

        var choice = HelpersGeneral.GetChoiceFromSelectionPrompt("Choose record to update", dictionaryOfHabits.Keys);
        if (choice == Database.BackWord) return;

        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();

            if (dictionaryOfHabits.TryGetValue(choice, out HabitRecord? habitToUpdate))
            {
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_records WHERE id = {habitToUpdate.Id})";
                var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar()) == 1;
                if (checkQuery)
                {
                    AnsiConsole.MarkupLine($"[darkcyan]Updating record:[/]\n{choice}\n");

                    var (shouldExit, date, habitName) = HelpersGeneral.GetDateAndHabitName();
                    if (shouldExit) return;
                    var (value, measurementUnit) = HelpersSql.GetValueAndMeasurementUnit(connection, habitName);

                    var updateCmd = connection.CreateCommand();
                    updateCmd.CommandText = $@"
                        UPDATE habit_records
                        SET date = @date,
                            habit_name = @habitName,
                            value = @value,
                            measurement_unit = @measurementUnit
                        WHERE id = @id";
                    updateCmd.Parameters.Add("@date", (SqliteType)DbType.Date).Value = date;
                    updateCmd.Parameters.Add("@habitName", (SqliteType)DbType.String).Value = habitName;
                    updateCmd.Parameters.Add("@value", (SqliteType)DbType.Int64).Value = value;
                    updateCmd.Parameters.Add("@measurementUnit", (SqliteType)DbType.String).Value = measurementUnit;
                    updateCmd.Parameters.Add("@id", (SqliteType)DbType.Int64).Value = habitToUpdate.Id;
                    updateCmd.ExecuteNonQuery();

                    AnsiConsole.MarkupLine("[green]Record updated successfully![/]");
                    HelpersGeneral.DisplayOneRecord(date, habitName, value, measurementUnit);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Record not found in the database.[/]");
                    HelpersGeneral.PressAnyKeyToContinue();
                }
            }
            else
            {
                Console.WriteLine("Selected habit not found.");
                HelpersGeneral.PressAnyKeyToContinue();
            }
        }
    }

    internal static void DeleteRecord()
    {
        Console.Clear();
        var dictionaryOfHabits = HelpersSql.MakeRecordsMap();
        if (Database.CheckIfNoRecordsAvailable(dictionaryOfHabits.Keys)) return;

        var choice = HelpersGeneral.GetChoiceFromSelectionPrompt("Choose record to delete", dictionaryOfHabits.Keys);
        if (choice == Database.BackWord) return;

        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();

            if (dictionaryOfHabits.TryGetValue(choice, out HabitRecord? habitToDelete))
            {
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_records WHERE id = @id)";
                checkCmd.Parameters.Add("@id", (SqliteType)DbType.Int64).Value = habitToDelete.Id;
                var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar()) == 1;
                if (checkQuery)
                {
                    AnsiConsole.MarkupLine($"[red]WARNING!\nYou want to delete that record permanently![/]");
                    AnsiConsole.MarkupLine($"{choice}\n");
                    if (!HelpersSql.ConfirmDeletion())
                    {
                        Console.Clear();
                        return;
                    }

                    var deleteCmd = connection.CreateCommand();
                    deleteCmd.CommandText =
                        $"DELETE from habit_records WHERE id = @id";
                    deleteCmd.Parameters.Add("@id", (SqliteType)DbType.Int64).Value = habitToDelete.Id;
                    deleteCmd.ExecuteNonQuery();

                    AnsiConsole.MarkupLine("Record deleted successfully.");
                    HelpersGeneral.PressAnyKeyToContinue();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Record not found in the database.[/]");
                    HelpersGeneral.PressAnyKeyToContinue();
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Selected record not found.[/]");
                HelpersGeneral.PressAnyKeyToContinue();
            }
        }
    }

    internal static void DeleteAllRecordsOfHabitName(string habitName)
    {
        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = $@"DELETE FROM habit_records WHERE habit_name = @habitName";
            deleteCmd.Parameters.Add("@habitName", (SqliteType)DbType.String).Value = habitName;
            deleteCmd.ExecuteNonQuery();

            AnsiConsole.MarkupLine($"All records of habit '{habitName}' deleted successfully.");
            HelpersGeneral.PressAnyKeyToContinue();
        }
    }
}
