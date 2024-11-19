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

        SQLiteParameter[] parameters = {
            new SQLiteParameter("@Habit", habit.Habit),
            new SQLiteParameter("@Quantity", habit.Quantity),
            new SQLiteParameter("@Date", habit.Date)
        };

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
}
