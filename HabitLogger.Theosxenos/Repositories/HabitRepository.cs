using System.Data;
using HabitLogger.Models;
using Microsoft.Data.Sqlite;

namespace HabitLogger.Repositories;

public class HabitRepository
{
    private readonly HabitLoggerDatabase db = new();

    public List<HabitModel> GetAllHabits()
    {
        var habits = new List<HabitModel>();
        using var connection = db.GetConnection();
        var selectAllHabitsCommand = new SqliteCommand("SELECT * FROM Habits", connection);

        using var reader = selectAllHabitsCommand.ExecuteReader();
        while (reader.Read())
        {
            var habit = CreateHabitFromReader(reader);

            habits.Add(habit);
        }

        return habits;
    }

    public HabitModel GetHabitById(int habitId)
    {
        HabitModel? habit = null;
        using var connection = db.GetConnection();
        var selectHabitCommand = new SqliteCommand("SELECT * FROM Habits WHERE ID = $id", connection);
        selectHabitCommand.Parameters.AddWithValue("$id", habitId);

        using var reader = selectHabitCommand.ExecuteReader();

        return !reader.Read() ? habit : CreateHabitFromReader(reader);
    }

    public void AddHabit(HabitModel habit)
    {
        using var connection = db.GetConnection();

        var insertHabitCommand =
            new SqliteCommand("INSERT INTO Habits (HabitName, Unit) VALUES ($name, $unit)", connection);
        insertHabitCommand.Parameters.AddWithValue("$name", habit.HabitName);
        insertHabitCommand.Parameters.AddWithValue("$unit", habit.Unit);
        insertHabitCommand.ExecuteNonQuery();
    }

    public void UpdateHabit(HabitModel habit, int toUpdateHabitId)
    {
        using var connection = db.GetConnection();

        var updateHabitCommand =
            new SqliteCommand("UPDATE Habits SET HabitName = $name, Unit = $unit WHERE ID = $id", connection);
        updateHabitCommand.Parameters.AddWithValue("$id", toUpdateHabitId);
        updateHabitCommand.Parameters.AddWithValue("$name", habit.HabitName);
        updateHabitCommand.Parameters.AddWithValue("$unit", habit.Unit);
        updateHabitCommand.ExecuteNonQuery();
    }

    private HabitModel CreateHabitFromReader(IDataRecord reader)
    {
        // Assuming reader is already positioned on the correct row
        var habit = new HabitModel
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            HabitName = reader.GetString(reader.GetOrdinal("HabitName")),
            Unit = reader.GetString(reader.GetOrdinal("Unit"))
        };

        return habit;
    }
}