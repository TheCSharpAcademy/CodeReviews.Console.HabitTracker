using HabitTracker.lordWalnuts.Models;
using Microsoft.Data.Sqlite;

namespace HabitTracker.lordWalnuts;


internal static class Crud
{
    internal static List<Habit> GetAllHabits()
    {
        var list = new List<Habit>();
        return list;
    }

    internal static void InsertHabit()
    {
        var habit = Helpers.GetHabitInput();
        var date = Helpers.GetDateInput();
        var unit = Helpers.GetUnitInput();
        var quantity = Helpers.GetQuantityInput();

        SqliteConnection sqlConnection = new SqliteConnection(connectionString: Program.connectionString);
        using (sqlConnection)
        {
            sqlConnection.Open();
            var sqlCommand = sqlConnection.CreateCommand();
            string commandText = @$"INSERT INTO habits(Habit, Date, Unit, Quantity)
                                    VALUES('{habit}', '{date}', '{unit}', '{quantity}')";

            sqlCommand.CommandText = commandText;
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }

    internal static void UpdateHabit()
    {

    }
    internal static void DeleteHabit()
    {

    }



}
