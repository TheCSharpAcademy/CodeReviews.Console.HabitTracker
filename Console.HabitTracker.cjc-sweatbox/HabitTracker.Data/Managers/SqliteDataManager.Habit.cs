// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Managers.SqliteDataManager.Habit
// --------------------------------------------------------------------------------------------------
// Partial class for data manager methods specific to the Habit entity.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Entities;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Data.Managers;

public partial class SqliteDataManager
{
    #region Constants

    private static readonly string AddHabitQuery =
        @"
        INSERT INTO habit
        (
             name
            ,measure
        )
        VALUES
        (
             $name
            ,$measure
        )
        ;";

    private static readonly string GetHabitQuery =
        @"
        SELECT
            * 
        FROM
            habit
        WHERE
            habit_id = $habit_id
        ;";

    private static readonly string GetHabitsQuery =
        @"
        SELECT
            * 
        FROM
            habit
        ;";

    private static readonly string GetHabitsByIsActiveQuery =
        @"
        SELECT 
            * 
        FROM
            habit
        WHERE
            is_active = $is_active
        ;";

    private static readonly string SetHabitQuery =
        @"
        UPDATE
            habit
        SET
             name = $name
            ,measure = $measure
        WHERE
            habit_id = $habit_id
        ;";

    private static readonly string SetHabitIsActiveQuery =
        @"
        UPDATE
            habit
        SET
            is_active = $is_active
        WHERE
            habit_id = $habit_id
        ;";

    #endregion
    #region Methods: Public - Create

    public void AddHabit(string name, string measure)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = AddHabitQuery;
        command.Parameters.Add("$name", SqliteType.Text).Value = name;
        command.Parameters.Add("$measure", SqliteType.Text).Value = measure;
        command.ExecuteNonQuery();

        connection.Close();
    }

    #endregion
    #region Methods: Public - Read

    public HabitEntity? GetHabit(int id)
    {
        HabitEntity? output = null;

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitQuery;
        command.Parameters.Add("$habit_id", SqliteType.Integer).Value = id;

        using SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            output = new HabitEntity(reader);
        }

        connection.Close();
        return output;
    }

    public IReadOnlyList<HabitEntity> GetHabits()
    {
        var output = new List<HabitEntity>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitsQuery;

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new HabitEntity(reader));
        }

        connection.Close();
        return output;
    }

    public IReadOnlyList<HabitEntity> GetHabitsByIsActive(bool isActive)
    {
        var output = new List<HabitEntity>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = GetHabitsByIsActiveQuery;
        command.Parameters.Add(parameterName: "$is_active", SqliteType.Integer).Value = isActive;

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            output.Add(new HabitEntity(reader));
        }

        connection.Close();
        return output;
    }

    #endregion
    #region Methods: Public - Update

    public void SetHabit(int id, string name, string measure)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SetHabitQuery;
        command.Parameters.Add("$habit_id", SqliteType.Integer).Value = id;
        command.Parameters.Add("$name", SqliteType.Text).Value = name;
        command.Parameters.Add("$measure", SqliteType.Text).Value = measure;
        command.ExecuteNonQuery();

        connection.Close();
    }

    public void SetHabitIsActive(int id, bool isActive)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SetHabitIsActiveQuery;
        command.Parameters.Add("$habit_id", SqliteType.Integer).Value = id;
        command.Parameters.Add("$is_active", SqliteType.Integer).Value = isActive;
        command.ExecuteNonQuery();

        connection.Close();
    }

    #endregion
}
