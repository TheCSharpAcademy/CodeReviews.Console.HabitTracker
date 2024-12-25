using Microsoft.Data.Sqlite;
using Spectre.Console;

internal class HelpersReports
{
    internal static void ShowReport()
    {
        var habitNamesList = HelpersSql.RetrieveHabitNamesFromDb();
        var habitNames = HelpersSql.MakeListOfHabitNames(habitNamesList);
        var habitName = HelpersGeneral.GetChoiceFromSelectionPrompt("Choose habit:", habitNames);
        if (habitName == Database.BackWord) return;

        AnsiConsole.MarkupLine($"Report for [green]\"{habitName}\"[/] habit.");
        AnsiConsole.MarkupLine("[yellow]Start date input:[/]");
        DateTime start = HelpersGeneral.GetDateInput();

        var (isValid, minDate) = GetMinDateFromDb();
        if (!isValid) return;

        while (start < minDate)
        {
            AnsiConsole.MarkupLine("[red]Entered starting date exceeds earliest date in the database.[/]\n" +
                $"First record's date in database is [yellow]{minDate.ToString(HelpersGeneral.DateFormat)}[/].");
            var answer = HelpersGeneral.GetYesNoAnswer("Do you want to use it as a starting date for report?");
            if (answer)
            {
                start = minDate;
                break;
            }
            else start = HelpersGeneral.GetDateInput();
        }
        AnsiConsole.MarkupLine($"Start date: {start.ToString(HelpersGeneral.DateFormat)}\n");

        AnsiConsole.MarkupLine("[yellow]End date input:[/]");
        DateTime end = HelpersGeneral.GetDateInput();
        while (end < start)
        {
            AnsiConsole.MarkupLine($"[red]Entered ending date \"{end.ToString(HelpersGeneral.DateFormat)}\" " +
                $"can not be earlier than starting date.[/]");
            end = HelpersGeneral.GetDateInput();
        }
        AnsiConsole.MarkupLine($"End date: {end.ToString(HelpersGeneral.DateFormat)}\n");

        ShowTotal(habitName, start.ToString(HelpersGeneral.DateFormat), end.ToString(HelpersGeneral.DateFormat));
    }

    internal static (bool, DateTime) GetMinDateFromDb()
    {
        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var reportCmd = connection.CreateCommand();
            reportCmd.CommandText = @"
                SELECT * FROM habit_records
                WHERE date = (SELECT MIN(date) FROM habit_records)";
            var reader = reportCmd.ExecuteReader();
            if (reader.Read())
            {
                return (true, Convert.ToDateTime(reader["date"]));
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No data found.[/]");
                HelpersGeneral.PressAnyKeyToContinue();
                return (false, DateTime.MinValue);
            }
        }
    }

    internal static void ShowSummarizedReport()
    {
        var reports = new List<HabitReport>();
        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var reportCmd = connection.CreateCommand();
            reportCmd.CommandText = @"
                SELECT habit_name,
                SUM(value) AS total_value,
                measurement_unit FROM habit_records
                GROUP BY habit_name, measurement_unit";
            var result = reportCmd.ExecuteScalar();
            if (result == DBNull.Value) return;

            var reader = reportCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var report = new HabitReport
                    {
                        HabitName = reader["habit_name"].ToString() ?? "",
                        MeasurementUnit = reader["measurement_unit"].ToString() ?? "",
                        TotalValue = reader.IsDBNull(reader.GetOrdinal("total_value")) ? 0 :
                            Convert.ToInt32(reader["total_value"])
                    };
                    reports.Add(report);
                }
            }
            else return;
        }

        var (isValid, minDate) = GetMinDateFromDb();
        if (!isValid) return;

        AnsiConsole.MarkupLine($"\nAll records summarized from {minDate.ToString(HelpersGeneral.DateFormat)}:");
        var table = new Table();
        table.AddColumn("Habit");
        table.AddColumn("Total value");
        foreach (var report in reports)
        {
            string total = GetTotalMessage(report.TotalValue, report.MeasurementUnit ?? "");
            table.AddRow(
                new Markup($"[yellow]{report.HabitName}[/]"),
                new Markup($"[green]{total}[/]"));
        }
        AnsiConsole.Write(table);
    }

    internal static void ShowTotal(string habitName, string startDate, string endDate)
    {
        int totalValue = 0;
        string? measurementUnit = "";

        using (var connection = new SqliteConnection(Database.ConnectionString))
        {
            connection.Open();
            var reportCmd = connection.CreateCommand();
            reportCmd.CommandText = @"
                SELECT SUM(value) AS total_value, measurement_unit
                FROM habit_records
                WHERE habit_name = @habit_name
                AND DATE(date) BETWEEN @start_date AND @end_date";
            reportCmd.Parameters.AddWithValue("@habit_name", habitName);
            reportCmd.Parameters.AddWithValue("@start_date", startDate);
            reportCmd.Parameters.AddWithValue("@end_date", endDate);
            var result = reportCmd.ExecuteScalar();
            if (result == DBNull.Value)
            {
                AnsiConsole.MarkupLine("[red]No data found.[/]");
                HelpersGeneral.PressAnyKeyToContinue();
                return;
            }

            var reader = reportCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    totalValue = Convert.ToInt32(reader["total_value"]);
                    measurementUnit = reader["measurement_unit"].ToString() ?? "";
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No data found.[/]");
                HelpersGeneral.PressAnyKeyToContinue();
                return;
            }
        }

        string total = GetTotalMessage(totalValue, measurementUnit);
        AnsiConsole.MarkupLine($"[green]Total {measurementUnit} of \"{habitName}\" habit " +
        $"for the period from {startDate} to {endDate}:[/]\n[yellow]=> {total}.[/]");
        HelpersGeneral.PressAnyKeyToContinue();
    }

    internal static string GetTotalMessage(int inputValue, string measurementUnit)
    {
        string detailsMesage = "";

        if (measurementUnit == "hours")
        {
            int totalDays = inputValue / 24;
            int totalHours = inputValue % 24;
            string hours = (totalHours == 1) ? "hour" : "hours";
            string days = (totalDays == 1) ? "day" : "days";
            if (inputValue < 24) detailsMesage = $"{inputValue} {hours}";
            else detailsMesage = $"{totalDays} {days} {totalHours} {hours}";
        }
        else if (measurementUnit == "minutes")
        {
            int totalDays = inputValue / 1440;
            int remainingMinutes = inputValue % 1440;
            int totalHours = remainingMinutes / 60;
            int totalMinutes = remainingMinutes % 60;
            string minutes = (totalMinutes == 1) ? "minute" : "minutes";
            string hours = (totalHours == 1) ? "hour" : "hours";
            string days = (totalDays == 1) ? "day" : "days";
            if (inputValue < 60) detailsMesage = $"{inputValue} {minutes}";
            else if (inputValue < 1440) detailsMesage = $"{totalHours} {hours} {totalMinutes} {minutes}";
            else detailsMesage = $"{totalDays} {days} {totalHours} {hours} {totalMinutes} {minutes}";
        }
        else detailsMesage = $"{inputValue} {measurementUnit}";

        return detailsMesage;
    }
}
