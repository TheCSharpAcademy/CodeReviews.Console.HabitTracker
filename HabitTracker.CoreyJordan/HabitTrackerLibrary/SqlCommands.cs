using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTrackerLibrary;
public static class SqlCommands
{
    public static List<DrinkingWater> GetAllRecords()
    {
        List<DrinkingWater> drinks = new();

        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"SELECT * FROM drinking_water";
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                drinks.Add(new DrinkingWater
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yy", new CultureInfo("en-US")),
                    Quantity = reader.GetInt32(2)
                });
            }
        }
        return drinks;
    }

    public static void InitializeDB(string connString)
    {
        using (var conn = new SqliteConnection(connString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water
        (Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        Quantity INTEGER)";

            cmd.ExecuteNonQuery();
        }
    }

    public static void InsertRecord(string date, int quantity)
    {
        using (var conn = new SqliteConnection(DataConnection.ConnString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
            cmd.ExecuteNonQuery();
        }
    }

    public static void UpdateRecord(int recordId)
    {
        throw new NotImplementedException();
    }
}
