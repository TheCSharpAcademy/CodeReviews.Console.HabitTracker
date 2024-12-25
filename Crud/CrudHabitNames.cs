using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Data;
using System.Globalization;

internal class CrudHabitNames
{
    internal static string CreateNewHabitName()
    {
        var newHabitName = AnsiConsole.Ask<string>("Enter a new habit's name:");
        newHabitName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newHabitName.ToLower());
        var newMeasurementUnit = AnsiConsole.Ask<string>("Enter a new habit's measurement unit (i.e. minutes):");
        newMeasurementUnit = newMeasurementUnit.ToLower();

        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $@"INSERT OR IGNORE INTO habit_names (name, measurement_unit)
                VALUES(@habitName, @measurementUnit)";
            tableCmd.Parameters.Add("@habitName", (SqliteType)DbType.String).Value = newHabitName;
            tableCmd.Parameters.Add("@measurementUnit", (SqliteType)DbType.String).Value = newMeasurementUnit;
            tableCmd.ExecuteNonQuery();

            AnsiConsole.MarkupLine($"A new habit name [green]'{newHabitName}'[/] created successfully!\n");
        }

        var useIt = HelpersGeneral.GetYesNoAnswer("Do you want to use it in your new record?");
        return useIt ? newHabitName : Database.BackWord;
    }

    internal static void DeleteHabitName()
    {
        var habitNamesList = HelpersSql.RetrieveHabitNamesFromDb();
        var habitNames = HelpersSql.MakeListOfHabitNames(habitNamesList);
        var namesMap = new Dictionary<string, HabitName>();
        for (int i = 0; i < habitNames.Count; i++)
        {
            namesMap.Add(habitNames[i], habitNamesList[i]);
        }

        if (habitNames.Count() == 0)
        {
            AnsiConsole.MarkupLine("[red]No habit names found.[/]");
            HelpersGeneral.PressAnyKeyToContinue();
            return;
        }

        var name = HelpersGeneral.GetChoiceFromSelectionPrompt("Choose habit name to delete:", namesMap.Keys);
        if (name == Database.BackWord) return;

        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            if (namesMap.TryGetValue(name, out HabitName? habitNameToDelete))
            {
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_names WHERE id = @id)";
                checkCmd.Parameters.Add("@id", (SqliteType)DbType.Int64).Value = habitNameToDelete.Id;
                var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar()) == 1;
                if (checkQuery)
                {
                    AnsiConsole.MarkupLine($"[red]WARNING!\nYou want to delete \"{name}\" habit![/]");
                    AnsiConsole.MarkupLine("You can make summary reports with that habit later if you don't delete all records with it.\n" +
                        "You just need to re-create that habit name with the same measurement unit while creating a new record.\n");
                    if (!HelpersSql.ConfirmDeletion())
                    {
                        Console.Clear();
                        return;
                    }

                    var deleteCmd = connection.CreateCommand();
                    deleteCmd.CommandText =
                        $"DELETE from habit_names WHERE id = @id";
                    deleteCmd.Parameters.Add("@id", (SqliteType)DbType.Int64).Value = habitNameToDelete.Id;
                    deleteCmd.ExecuteNonQuery();

                    AnsiConsole.MarkupLine($"Habit \"{name}\" deleted successfully.\n");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Habit {name} not found in the database.[/]");
                    HelpersGeneral.PressAnyKeyToContinue();
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Selected habit not found.[/]");
                HelpersGeneral.PressAnyKeyToContinue();
            }
        }

        var confirmDeleteAllNameRecords = HelpersGeneral.GetYesNoAnswer(
            $"[red]Do you want to also delete all records of \"{name}\" in database?[/]");
        if (confirmDeleteAllNameRecords)
        {
            var confirm = HelpersSql.ConfirmDeletion();
            if (confirm) CrudRecords.DeleteAllRecordsOfHabitName(name);
        }
        Console.Clear();
    }
}
