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

    public void Insert(string sqlStatement, params object[] parameters)
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

        Insert(sqlStatement, parameters);
    }
}
