// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Managers.SqliteDataManager.HabitLogReport
// --------------------------------------------------------------------------------------------------
// Partial class for data manager methods specific to the HabitLogReport entity.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Constants;
using HabitTracker.Data.Entities;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Data.Managers;

public partial class SqliteDataManager
{
    #region Constants

    internal static readonly string GetHabitLogReportQuery =
        @"
        SELECT
            *
        FROM
            vw_habit_log_report
        ;";

    internal static readonly string GetHabitLogReportByHabitIdQuery =
        @"
        SELECT
            *
        FROM
            vw_habit_log_report
        WHERE
            habit_id = $habit_id
        ;";

    internal static readonly string GetHabitLogSumReportQuery =
        @"
        SELECT
             name
            ,SUM(quantity) AS quantity
            ,measure
        FROM 
            vw_habit_log_report
        WHERE
            date > $date_from
            AND date < $date_to
        GROUP BY
             name
            ,measure
        ;";

    internal static readonly string GetHabitLogSumReportByHabitIdQuery =
        @"
        SELECT
             name
            ,SUM(quantity) AS quantity
            ,measure
        FROM 
            vw_habit_log_report
        WHERE
            habit_id = $habit_id
            AND date > $date_from
            AND date < $date_to
        GROUP BY
             name
            ,measure
        ;";
    
    #endregion
    #region Methods: Public - Read

    public IReadOnlyList<HabitLogReportEntity> GetHabitLogReport()
    {
        var output = new List<HabitLogReportEntity>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitLogReportQuery;

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new HabitLogReportEntity(reader));
        }

        connection.Close();
        return output;
    }

    public IReadOnlyList<HabitLogReportEntity> GetHabitLogReportByHabitId(int habitId)
    {
        var output = new List<HabitLogReportEntity>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitLogReportByHabitIdQuery;
        command.Parameters.Add("$habit_id", SqliteType.Integer).Value = habitId;

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new HabitLogReportEntity(reader));
        }

        connection.Close();
        return output;
    }

    public IReadOnlyList<HabitLogSumReportEntity> GetHabitLogSumReport(DateTime dateFrom, DateTime dateTo)
    {
        var output = new List<HabitLogSumReportEntity>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitLogSumReportQuery;
        command.Parameters.Add("$date_from", SqliteType.Text).Value = dateFrom.ToString(FormatString.ISO8601);
        command.Parameters.Add("$date_to", SqliteType.Text).Value = dateTo.ToString(FormatString.ISO8601);

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new HabitLogSumReportEntity(reader));
        }

        connection.Close();
        return output;
    }

    public IReadOnlyList<HabitLogSumReportEntity> GetHabitLogSumReportByHabitId(DateTime dateFrom, DateTime dateTo, int habitId)
    {
        var output = new List<HabitLogSumReportEntity>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitLogSumReportByHabitIdQuery;
        command.Parameters.Add("$date_from", SqliteType.Text).Value = dateFrom.ToString(FormatString.ISO8601);
        command.Parameters.Add("$date_to", SqliteType.Text).Value = dateTo.ToString(FormatString.ISO8601);
        command.Parameters.Add("$habit_id", SqliteType.Integer).Value = habitId;

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new HabitLogSumReportEntity(reader));
        }

        connection.Close();
        return output;
    }

    #endregion
}
