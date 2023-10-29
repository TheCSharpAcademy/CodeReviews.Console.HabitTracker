using Microsoft.Data.Sqlite;
using System.Globalization;

public class WaterHabitHelpers
{
    private const string CONNECTIONSTR = "Data Source=habit.db";

    public static void CreateWaterHabitTableIfNotExist()
    {
        using (var connection = new SqliteConnection(CONNECTIONSTR))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS waterHabit (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    date Date NOT NULL,
                    quantity INT NOT NULL
                );
            ";
            command.ExecuteNonQuery();
        }
    }

    public static void Insert(WaterHabit habit)
    {
        using (var connection = new SqliteConnection(CONNECTIONSTR))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO waterHabit(date, quantity) 
                                     VALUES('{habit.Date}', '{habit.Quantity}')";
            command.ExecuteNonQuery();
        }
    }

    public static List<WaterHabit> SeleteAll()
    {
        List<WaterHabit> waterHabits = new();
        using (var connection = new SqliteConnection(CONNECTIONSTR))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM waterHabit";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = int.Parse(reader.GetString(0));
                    DateTime date = DateTime.Parse(reader.GetString(1));
                    int quantity = int.Parse(reader.GetString(2));
                    waterHabits.Add(new WaterHabit(id, date, quantity));
                }
            }
        }
        return waterHabits;
    }
}