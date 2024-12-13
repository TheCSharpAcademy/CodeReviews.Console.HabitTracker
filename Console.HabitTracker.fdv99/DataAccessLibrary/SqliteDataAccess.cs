using System.Data.SQLite;
using HabitData;

namespace DataAccessLibrary;

public class SqliteDataAccess
{
    private SQLiteConnection CreateConnection()
    {
        var cnn = new SQLiteConnection(DataAccessHelpers.connectionString);
        cnn.Open();
        return cnn;
    }

    private SQLiteParameter[] GetHabitParameters(HabitModel habit)
    {
        return new SQLiteParameter[]
        {
            new SQLiteParameter("@Habit", habit.Habit),
            new SQLiteParameter("@Quantity", habit.Quantity),
            new SQLiteParameter("@Date", habit.Date),
            new SQLiteParameter("@Id", habit.Id)
        };
    }

    public void ExecuteCommand(string sqlStatement, params object[] parameters)
    {
        using (var cnn = CreateConnection())
        {
            using (var command = new SQLiteCommand(sqlStatement, cnn))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }
    }

    public void InsertHabit(HabitModel habit)
    {
        string sqlStatement = "INSERT INTO Habits (Habit, Quantity, Date) VALUES (@Habit, @Quantity, @Date)";

        var parameters = GetHabitParameters(habit).Where(p => p.ParameterName != "@Id").ToArray();

        ExecuteCommand(sqlStatement, parameters);
    }

    public void UpdateHabit(HabitModel habit)
    {
        string sqlStatement = "UPDATE Habits SET Habit = @Habit, Quantity = @Quantity, Date = @Date WHERE Id = @Id";
        
        var parameters = GetHabitParameters(habit);

        ExecuteCommand(sqlStatement, parameters);
    }

    public List<HabitModel> LoadData(string sqlStatement, params object[] parameters)
    {
        List<HabitModel> habits = new();
        using (var cnn = CreateConnection())
        {
            using (var command = new SQLiteCommand(sqlStatement, cnn))
            {
                command.Parameters.AddRange(parameters);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HabitModel habit = new()
                        {
                            Id = Convert.ToInt32(reader.GetInt64(0)),
                            Habit = reader.GetString(1),
                            Quantity = Convert.ToInt32(reader.GetInt64(3)),
                            Date = reader.GetString(2)
                        };
                        habits.Add(habit);
                    }
                }
            }
        }
        return habits;
    }

    public HabitModel GetHabitById(int id)
    {
        string sqlStatement = "SELECT Id, Habit, Quantity, Date FROM Habits WHERE Id = @Id";

        using (var cnn = CreateConnection())
        {
            using (var command = new SQLiteCommand(sqlStatement, cnn))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new HabitModel
                        {
                            Id = Convert.ToInt32(reader.GetInt64(0)),
                            Habit = reader.GetString(1),
                            Quantity = Convert.ToInt32(reader.GetInt64(2)),
                            Date = reader.GetString(3)
                        };
                    }
                    else
                    {
                        return null; // or throw an exception if preferred
                    }
                }
            }
        }
    }

    public void DeleteHabit(int id)
    {
        string sqlStatement = "DELETE FROM Habits WHERE Id = @Id";
        SQLiteParameter[] parameters = {
            new SQLiteParameter("@Id", id)
        };
        ExecuteCommand(sqlStatement, parameters);
    }
}
