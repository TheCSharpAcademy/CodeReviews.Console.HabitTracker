// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Managers.SqliteDataManager.HabitLog
// --------------------------------------------------------------------------------------------------
// Partial class for data manager methods specific to the HabitLog entity.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Constants;
using HabitTracker.Data.Entities;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Data.Managers;

public partial class SqliteDataManager
{
    #region Constants

    internal static readonly string AddHabitLogQuery =
        @"
        INSERT INTO habit_log
        (
             habit_id
            ,date
            ,quantity
        )
        VALUES
        (
             $habit_id
            ,$date
            ,$quantity
        )
        ;";

    internal static readonly string GetHabitLogQuery =
        @"
        SELECT
            * 
        FROM
            habit_log
        WHERE
            habit_log_id = $habit_log_id
        ;";

    internal static readonly string GetHabitLogByHabitIdAndDateQuery =
    @"
        SELECT
            * 
        FROM
            habit_log
        WHERE
            habit_id = $habit_id
            AND date = $date
        ;";

    internal static readonly string GetHabitLogsQuery =
        @"
        SELECT
            *
        FROM
            habit_log
        ;";

    internal static readonly string SetHabitLogQuery =
        @"
        UPDATE
            habit_log
        SET
             habit_id = $habit_id
            ,date = $date
            ,quantity = $quantity
        WHERE
            habit_log_id = $habit_log_id
        ;";

    internal static readonly string DeleteHabitLogQuery =
        @"
        DELETE FROM
            habit_log
        WHERE
            habit_log_id = $habit_log_id
        ;";

    #endregion
    #region Methods: Public - Create

    public void AddHabitLog(int habitId, DateTime date, int quantity)
    {
        // Validation.
        // If habit already has an entry for the date, then merge (increase the quantity).
        var habitLog = GetHabitLogByHabitIdAndDate(habitId, date);

        if (habitLog == null)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = AddHabitLogQuery;
            command.Parameters.Add("$habit_id", SqliteType.Integer).Value = habitId;
            command.Parameters.Add("$date", SqliteType.Text).Value = date.ToString(FormatString.ISO8601);
            command.Parameters.Add("$quantity", SqliteType.Integer).Value = quantity;
            command.ExecuteNonQuery();

            connection.Close();
        }
        else
        {
            // Additional date instance. Merge.
            quantity += habitLog.Quantity;
            SetHabitLog(habitLog.Id, habitId, date, quantity);
        }
    }

    #endregion
    #region Methods: Public - Read

    public HabitLogEntity? GetHabitLog(int id)
    {
        HabitLogEntity? output = null;

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitLogQuery;
        command.Parameters.Add("$habit_log_id", SqliteType.Integer).Value = id;

        using SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            output = new HabitLogEntity(reader);
        }

        connection.Close();
        return output;
    }

    public HabitLogEntity? GetHabitLogByHabitIdAndDate(int habitId, DateTime date)
    {
        HabitLogEntity? output = null;

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitLogByHabitIdAndDateQuery;
        command.Parameters.Add("$habit_id", SqliteType.Integer).Value = habitId;
        command.Parameters.Add("$date", SqliteType.Text).Value = date.ToString(FormatString.ISO8601);

        using SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            output = new HabitLogEntity(reader);
        }

        connection.Close();
        return output;
    }
    public IReadOnlyList<HabitLogEntity> GetHabitLogs()
    {
        var output = new List<HabitLogEntity>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitLogsQuery;

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new HabitLogEntity(reader));
        }

        connection.Close();
        return output;
    }

    #endregion
    #region Methods: Public - Update

    public void SetHabitLog(int id, int habitId, DateTime date, int quantity)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SetHabitLogQuery;
        command.Parameters.Add("$habit_log_id", SqliteType.Integer).Value = id;
        command.Parameters.Add("$habit_id", SqliteType.Integer).Value = habitId;
        command.Parameters.Add("$date", SqliteType.Text).Value = date.ToString(FormatString.ISO8601);
        command.Parameters.Add("$quantity", SqliteType.Integer).Value = quantity;
        command.ExecuteNonQuery();

        connection.Close();
    }

    #endregion
    #region Methods: Public - Delete

    public void DeleteHabitLog(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = DeleteHabitLogQuery;
        command.Parameters.Add("$habit_log_id", SqliteType.Integer).Value = id;
        command.ExecuteNonQuery();

        connection.Close();
    }

    #endregion
}
