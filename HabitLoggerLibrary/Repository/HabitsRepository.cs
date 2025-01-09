using HabitLoggerLibrary.Sqlite;
using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary.Repository;

internal sealed class HabitsRepository(SqliteConnection connection) : IHabitsRepository
{
    public List<Habit> GetHabits()
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT id, habit, unit_of_measure FROM {IHabitsRepository.TableName} ORDER BY id";

        using var reader = command.ExecuteReader();
        var results = new List<Habit>();
        while (reader.Read())
            results.Add(new Habit(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));

        return results;
    }

    public Habit AddHabit(HabitDraft draft)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"INSERT INTO {IHabitsRepository.TableName} (habit, unit_of_measure) VALUES (@HabitName, @UnitOfMeasure);";
        command.Parameters.AddWithValue("@HabitName", draft.HabitName);
        command.Parameters.AddWithValue("@UnitOfMeasure", draft.UnitOfMeasure);
        command.ExecuteNonQuery();

        return GetHabitById(connection.GetLastInsertRowId());
    }

    public void UpdateHabit(Habit habit)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"UPDATE {IHabitsRepository.TableName} SET habit = @HabitName, unit_of_measure = @UnitOfMeasure WHERE id = @Id";

        command.Parameters.AddWithValue("@HabitName", habit.HabitName);
        command.Parameters.AddWithValue("@UnitOfMeasure", habit.UnitOfMeasure);
        command.Parameters.AddWithValue("@Id", habit.Id);
        var updatedCount = command.ExecuteNonQuery();
        if (updatedCount == 0) throw new HabitNotFoundException(habit.Id);
    }

    public void DeleteHabitById(long id)
    {
        if (!HasHabitById(id)) throw new HabitNotFoundException(id);

        var command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM {IHabitsRepository.TableName} WHERE id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();
    }

    public Habit GetHabitById(long id)
    {
        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT id, habit, unit_of_measure FROM {IHabitsRepository.TableName} WHERE id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (!reader.HasRows) throw new HabitNotFoundException(id);

        reader.Read();

        return new Habit(reader.GetInt64(0), reader.GetString(1), reader.GetString(2));
    }

    public bool HasHabitById(long id)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT id FROM {IHabitsRepository.TableName} WHERE id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        return reader.HasRows;
    }

    public long GetHabitsCount()
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(id) FROM {IHabitsRepository.TableName}";

        using var reader = command.ExecuteReader();
        reader.Read();

        return reader.GetInt64(0);
    }
}