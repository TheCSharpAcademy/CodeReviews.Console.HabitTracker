using FunRun.HabitTracker.Data.Model;
using FunRun.HabitTracker.Services.Interfaces;
using System.Data;
using System.Data.SQLite;

namespace FunRun.HabitTracker.Services;

public class SqlOperations : ISqlOperations
{
    private readonly IDbConnection _connection;

    public SqlOperations(IDbConnection connection)
    {
        _connection = connection;
    }


    public List<HabitModel> SqlReadAllHabits()
    {
        List<HabitModel> habits = new List<HabitModel>();
        string query = $"SELECT * FROM {HabitTable.TableName}";


        using (var command = _connection.CreateCommand())
        {
            command.CommandText = query;

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var habit = new HabitModel(
                        Convert.ToInt32(reader["Id"]),
                        reader.IsDBNull(reader.GetOrdinal("HabitName")) ? string.Empty : reader["HabitName"].ToString(),
                        reader.IsDBNull(reader.GetOrdinal("HabitDescription")) ? string.Empty : reader["HabitDescription"].ToString(),
                        Convert.ToInt32(reader["HabitCounter"])
                    );
                    habits.Add(habit);
                }
            }
        }


        return habits;
    }

    public void SqlCreateHabit(HabitModel newHabit)
    {
        string query = $@"
            INSERT INTO {HabitTable.TableName} ({HabitTable.HabitName}, {HabitTable.HabitDescription}, {HabitTable.HabitCounter})
            VALUES (@{HabitTable.HabitName}, @{HabitTable.HabitDescription}, @{HabitTable.HabitCounter});
        ";

        using (var command = _connection.CreateCommand())
        {
            command.CommandText = query;

            var habitNameParameter = CreateParameter($"@{HabitTable.HabitName}", DbType.String, newHabit.HabitName);
            var habitDescriptionParameter = CreateParameter($"@{HabitTable.HabitDescription}", DbType.String, newHabit.HabitDescription);
            var habitCounterParameter = CreateParameter($"@{HabitTable.HabitCounter}", DbType.Int32, newHabit.HabitCounter);

            command.Parameters.Add(habitNameParameter);
            command.Parameters.Add(habitDescriptionParameter);
            command.Parameters.Add(habitCounterParameter);

            command.ExecuteNonQuery();
        }
    }

    public void SqlUpdateHabit(HabitModel newHabit)
    {
        string query = $@"
            UPDATE {HabitTable.TableName}
            SET {HabitTable.HabitName} = @{HabitTable.HabitName},
                {HabitTable.HabitDescription} = @{HabitTable.HabitDescription},
                {HabitTable.HabitCounter} = @{HabitTable.HabitCounter}
            WHERE {HabitTable.Id} = @{HabitTable.Id};
        ";

        using (var command = _connection.CreateCommand())
        {
            command.CommandText = query;

            var habitIdParameter = CreateParameter($"@{HabitTable.Id}", DbType.Int32, newHabit.Id);
            var habitNameParameter = CreateParameter($"@{HabitTable.HabitName}", DbType.String, newHabit.HabitName);
            var habitDescriptionParameter = CreateParameter($"@{HabitTable.HabitDescription}", DbType.String, newHabit.HabitDescription);
            var habitCounterParameter = CreateParameter($"@{HabitTable.HabitCounter}", DbType.Int32, newHabit.HabitCounter);

            command.Parameters.Add(habitIdParameter);
            command.Parameters.Add(habitNameParameter);
            command.Parameters.Add(habitDescriptionParameter);
            command.Parameters.Add(habitCounterParameter);

            command.ExecuteNonQuery();
        }

    }

    public void SqlDeleteHabit(HabitModel newHabit)
    {

        string query = $@"
            Delete From {HabitTable.TableName}
            WHERE {HabitTable.Id} = @{HabitTable.Id};
        ";

        using (var command = _connection.CreateCommand())
        {
            command.CommandText = query;

            var habitIdParameter = CreateParameter($"@{HabitTable.Id}", DbType.Int32, newHabit.Id);

            command.Parameters.Add(habitIdParameter);

            command.ExecuteNonQuery();
        }
    }

    private IDbDataParameter CreateParameter(string name, DbType type, object value)
    {
        var parameter = _connection.CreateCommand().CreateParameter();
        parameter.ParameterName = name;
        parameter.DbType = type;
        parameter.Value = value;
        return parameter;
    }

}
