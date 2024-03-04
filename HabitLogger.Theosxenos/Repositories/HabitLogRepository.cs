using HabitLogger.Models;
using Microsoft.Data.Sqlite;

namespace HabitLogger.Repositories;

public class HabitLogRepository
{
    private readonly HabitLoggerDatabase db = new();

    public List<HabitLogModel> GetAllLogsForHabitId(int habitId)
    {
        var logs = new List<HabitLogModel>();
        using var connection = db.GetConnection();
        var selectLogsCommand = new SqliteCommand("SELECT * FROM HabitLogs WHERE HabitID = $habitid", connection);
        selectLogsCommand.Parameters.AddWithValue("$habitid", habitId);

        using var reader = selectLogsCommand.ExecuteReader();
        while (reader.Read())
        {
            var habitLog = new HabitLogModel
            {
                LogId = reader.GetInt32(reader.GetOrdinal("LogId")),
                HabitId = reader.GetInt32(reader.GetOrdinal("HabitId")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                Date = reader.GetDateTime(reader.GetOrdinal("Date"))
            };

            logs.Add(habitLog);
        }

        return logs;
    }

    public void UpdateHabitLog(HabitLogModel habitLog)
    {
        var connection = db.GetConnection();
        var updateLogCommand =
            new SqliteCommand("UPDATE HabitLogs SET Quantity = $quantity WHERE HabitID = $habitid AND Date = $date",
                connection);
        updateLogCommand.Parameters.AddWithValue("$quantity", habitLog.Quantity);
        updateLogCommand.Parameters.AddWithValue("$habitid", habitLog.HabitId);
        updateLogCommand.Parameters.AddWithValue("$date", habitLog.Date.ToString("O"));

        var updateResult = updateLogCommand.ExecuteNonQuery();

        if (updateResult == 1) return;

        updateLogCommand.CommandText =
            "INSERT INTO HabitLogs (HabitID, Quantity, Date) VALUES ($habitid, $quantity, $date)";
        updateLogCommand.ExecuteNonQuery();
    }
}