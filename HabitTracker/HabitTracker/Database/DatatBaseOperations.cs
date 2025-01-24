using HabitTracker.Models;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Database;

internal static class DatatBaseOperations
{
    internal static void CreateDatabase()
    {
        if (File.Exists("HabitTracker.db"))
        {
            return;
        }

        using(var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"CREATE TABLE WaterDrinking
                (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,  
                    Date        TEXT    NOT NULL,
                    Quantity    INTEGER NOT NULL
                )";

            command.ExecuteNonQuery();
        }
    }

    internal static void AddData(WaterDrinkingHabit habit)
    {
        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @$"INSERT INTO WaterDrinking (Date, Quantity)
                Values('{habit.Date.ToString("dd-MM-yyyy")}', {habit.Quantity})";

            command.ExecuteNonQuery();
        }
    }

    internal static List<WaterDrinkingHabit> GetAll()
    {
        var AllData = new List<WaterDrinkingHabit>();

        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM WaterDrinking";

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var habit = new WaterDrinkingHabit();

                habit.Id = reader.GetInt32(0);
                habit.Date = DateTime.Parse(reader.GetString(1));
                habit.Quantity = reader.GetInt32(2);

                AllData.Add(habit);
            }
        }

        return AllData;
    }

    internal static void Update(int id, string date, int quantity)
    {
        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @$"UPDATE WaterDrinking
                SET Date = '{date}',
                Quantity = {quantity}
                WHERE Id = {id}";

            command.ExecuteNonQuery();
        }
    }

    internal static void Delete(int id)
    {
        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM WaterDrinking WHERE Id = {id}";

            command.ExecuteNonQuery();
        }
    }

    internal static bool Exists(int id)
    {
        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM WaterDrinking WHERE Id = {id}";

            var reader = command.ExecuteReader();

            return reader.HasRows;
        }
    }
}
