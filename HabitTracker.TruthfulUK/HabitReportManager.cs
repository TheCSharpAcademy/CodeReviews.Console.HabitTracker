using HabitTracker.TruthfulUK.Helpers;
using Spectre.Console;

namespace HabitTracker.TruthfulUK;
internal class HabitReportManager
{
    internal static void GenerateDayReport()
    {
        AnsiConsole.MarkupLine("This report will show [underline]all habits[/] logged for the requested day.");

        var reportDate = AnsiConsole.Prompt(
            new TextPrompt<DateOnly>("Enter the requested date (YYYY-MM-DD) or leave blank for today:").AllowEmpty());

        List<(string, double, string)> dayReport = DbHelpers.HabitDayReport(reportDate);

        var dayReportTable = new Table();
        dayReportTable
            .AddColumn("[white on blue] Habit [/]")
            .AddColumn("[white on blue] Amount [/]");

        foreach ((string name, double amount, string measurement) row in dayReport) {             
            dayReportTable.AddRow(
                $"{row.name}",
                $"{InputHelpers.FormatDouble(row.amount)} {row.measurement}"
            );
        }

        dayReportTable
            .ShowRowSeparators()
            .Border(TableBorder.Horizontal)
            .Expand();

        AnsiConsole.Write(dayReportTable);
    }

    internal static void GenerateTotalLogged()
    {
        AnsiConsole.MarkupLine("This report shows [underline]all habits[/] and the [underline]total amount[/] you have logged.");

        List<(string, double, string)> totalLoggedReport = DbHelpers.TotalLoggedReport();

        var totalLoggedTable = new Table();
        totalLoggedTable
            .AddColumn("[white on blue] Habit [/]")
            .AddColumn("[white on blue] Amount [/]");

        foreach ((string name, double amount, string measurement) row in totalLoggedReport)
        {
            totalLoggedTable.AddRow(
                $"{row.name}",
                $"{InputHelpers.FormatDouble(row.amount)} {row.measurement}"
            );
        }

        totalLoggedTable
            .ShowRowSeparators()
            .Border(TableBorder.Horizontal)
            .Expand();

        AnsiConsole.Write(totalLoggedTable);
    }
}
