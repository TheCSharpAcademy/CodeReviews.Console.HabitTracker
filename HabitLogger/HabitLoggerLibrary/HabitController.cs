using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary;
public class HabitController
{
    public static List<HabitModel> GetRecords(string habitTable)
    {
        try
        {
            List<HabitModel> records = new List<HabitModel>();
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();

                cmd.CommandText = $"SELECT Id, Day, Quantity FROM {habitTable}";

                SqliteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    records.Add(new HabitModel()
                    {
                        Id = reader.GetFieldValue<int>(0),
                        Day = reader.GetFieldValue<DateTime>(1),
                        Quantity = reader.GetFieldValue<int>(2),
                    });
                }
                connection.Close();
            }

            return records;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error ocurred: {ex.Message}");
            return new List<HabitModel>();
        }
    }
    public static bool InsertRecord(HabitModel habit, string habitTable)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();

                cmd.CommandText = $@"INSERT INTO {habitTable} (Day, Quantity) 
                                        VALUES ('{habit.Day.ToString("yyyy-MM-dd")}', {habit.Quantity})";

                cmd.ExecuteNonQuery();
                connection.Close();
            }

            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error ocurred: {ex.Message}");
            return false;
        }
    }

    public static bool UpdateRecord(HabitModel habit, string habitTable)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();

                cmd.CommandText = $@"UPDATE {habitTable}  SET 
                                        Day = '{habit.Day.ToString("yyyy-MM-dd")}', 
                                        Quantity = {habit.Quantity}
                                        WHERE Id = {habit.Id}";

                cmd.ExecuteNonQuery();
                connection.Close();
            }

            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error ocurred: {ex.Message}");
            return false;
        }
    }

    public static bool DeleteRecord(HabitModel habit, string habitTable)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();

                cmd.CommandText = $@"DELETE FROM {habitTable}
                                        WHERE Id = {habit.Id}";

                cmd.ExecuteNonQuery();
                connection.Close();
            }

            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error ocurred: {ex.Message}");
            return false;
        }
    }

    public static List<(int, int)> GetQuantityPerYearRecords(string habitTable)
    {
        try
        {
            List<(int, int)> records = new List<(int, int)>();
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();

                cmd.CommandText = @$"SELECT SUBSTR(Day, 0, 5) AS Year, SUM(Quantity) AS TotalQty
                                            FROM {habitTable}
                                            GROUP BY 1;";

                SqliteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    records.Add((reader.GetFieldValue<int>(0), reader.GetFieldValue<int>(1)));
                }
                connection.Close();
            }

            return records;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error ocurred: {ex.Message}");
            return new List<(int, int)>();
        }
    }

    public static List<(int, int)> GetTimesPerYearRecords(string habitTable)
    {
        try
        {
            List<(int, int)> records = new List<(int, int)>();
            using (SqliteConnection connection = new SqliteConnection(Config.ConnectionString))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();

                cmd.CommandText = @$"SELECT SUBSTR(Day, 0, 5) AS Year, COUNT(*) AS Times
                                            FROM {habitTable}
                                            GROUP BY 1;";

                SqliteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    records.Add((reader.GetFieldValue<int>(0), reader.GetFieldValue<int>(1)));
                }
                connection.Close();
            }

            return records;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error ocurred: {ex.Message}");
            return new List<(int, int)>();
        }
    }
}
