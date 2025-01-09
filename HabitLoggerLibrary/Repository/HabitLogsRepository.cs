using HabitLoggerLibrary.Sqlite;
using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary.Repository;

public sealed class HabitLogsRepository(SqliteConnection connection) : IHabitLogsRepository
{
    public List<HabitLog> GetHabitLogs()
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $@"SELECT hl.id, hl.habit_id, hl.quantity, hl.habit_date, h.habit, h.unit_of_measure 
                FROM {IHabitLogsRepository.TableName} hl
                INNER JOIN {IHabitsRepository.TableName} h ON habit_id = h.id 
                ORDER BY hl.id";

        using var reader = command.ExecuteReader();
        var results = new List<HabitLog>();
        while (reader.Read())
            results.Add(new HabitLog(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt32(2),
                DateOnly.FromDateTime(reader.GetDateTime(3)), reader.GetString(4), reader.GetString(5)));

        return results;
    }

    public HabitLog AddHabitLog(HabitLogDraft draft)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"INSERT INTO {IHabitLogsRepository.TableName} (habit_id, quantity, habit_date) VALUES (@HabitId, @Quantity, @Date);";
        command.Parameters.AddWithValue("@HabitId", draft.HabitId);
        command.Parameters.AddWithValue("@Quantity", draft.Quantity);
        command.Parameters.AddWithValue("@Date", draft.HabitDate);
        command.ExecuteNonQuery();

        return GetHabitLogById(connection.GetLastInsertRowId());
    }

    public void UpdateHabitLog(HabitLog habitLog)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"UPDATE {IHabitLogsRepository.TableName} SET habit_id = @HabitId, quantity = @UnitOfMeasure, habit_date = @Date WHERE id = @Id";

        command.Parameters.AddWithValue("@HabitId", habitLog.HabitId);
        command.Parameters.AddWithValue("@UnitOfMeasure", habitLog.Quantity);
        command.Parameters.AddWithValue("@Date", habitLog.HabitDate);
        command.Parameters.AddWithValue("@Id", habitLog.Id);
        var updatedCount = command.ExecuteNonQuery();
        if (updatedCount == 0) throw new HabitLogNotFoundException(habitLog.Id);
    }

    public void DeleteHabitLogById(long id)
    {
        if (!HasHabitLogById(id)) throw new HabitLogNotFoundException(id);

        var command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM {IHabitLogsRepository.TableName} WHERE id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }

    public HabitLog GetHabitLogById(long id)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $@"SELECT hl.id, hl.habit_id, hl.quantity, hl.habit_date, h.habit, h.unit_of_measure 
                FROM {IHabitLogsRepository.TableName} hl
                INNER JOIN {IHabitsRepository.TableName} h ON habit_id = h.id 
                WHERE hl.id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (!reader.HasRows) throw new HabitLogNotFoundException(id);

        reader.Read();

        return new HabitLog(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt32(2),
            DateOnly.FromDateTime(reader.GetDateTime(3)), reader.GetString(4), reader.GetString(5));
    }

    public bool HasHabitLogById(long id)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT id FROM {IHabitLogsRepository.TableName} WHERE id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        return reader.HasRows;
    }

    public bool HasHabitLogByHabitIdAndDate(long habitId, DateOnly date)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT id FROM {IHabitLogsRepository.TableName} WHERE habit_id = @HabitId AND habit_date = @Date";
        command.Parameters.AddWithValue("@HabitId", habitId);
        command.Parameters.AddWithValue("@Date", date);

        using var reader = command.ExecuteReader();

        return reader.HasRows;
    }

    public List<Statistic> GetStatistics(string period)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $@"SELECT h.habit, strftime(@Period, hl.habit_date) AS period, SUM(hl.quantity), h.unit_of_measure 
FROM {IHabitsRepository.TableName} AS h 
INNER JOIN {IHabitLogsRepository.TableName} AS hl ON hl.habit_id = h.id
GROUP BY h.id, period
ORDER BY h.id, hl.habit_date";

        command.Parameters.AddWithValue("@Period", period);

        using var reader = command.ExecuteReader();

        var results = new List<Statistic>();
        while (reader.Read())
            results.Add(
                new Statistic(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3)));

        return results;
    }
}