using HabbitTrackerLibrary.sadklouds.Models;
using Microsoft.Data.Sqlite;

namespace HabbitTrackerLibrary.sadklouds;

public class SqliteDataAccess
{
    private readonly string _connectionString;

    public SqliteDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }
    public void ConnectDB()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var table = connection.CreateCommand();
            table.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity REAL,
                Unit TEXT)";
            table.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<HabitModel> LoadHabit()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            List<HabitModel> output = new List<HabitModel>();

            var rows = connection.CreateCommand();
            rows.CommandText = @$"SELECT * FROM drinking_water;";

            SqliteDataReader reader = rows.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DateTime date = DateTime.Parse(reader.GetString(1));
                    double quantity = reader.GetDouble(2);
                    string unit = reader.GetString(3);
                    output.Add(new HabitModel { Id = id, Date = date, Quantity = quantity, Unit = unit });
                }
            }

            connection.Close();
            return output;
        }

    }

    public string UpdateRecord(int id, string date, double quantity, string unit)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string output = "";
            if (CheckIdExists(id) == 1)
            {
                var update = connection.CreateCommand();
                update.CommandText = $@"UPDATE drinking_water SET Date = '{date}', Quantity = {quantity}, Unit = '{unit}' WHERE Id = {id}";
                update.ExecuteNonQuery();
                output = "\nrecord was updated successfully\n";
            }
            else
            {
                output = $"\nrecord with id:{id} could not be found and updated!\n";
            }
            connection.Close();
            return output;
        }
    }

    private int CheckIdExists(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var existsCheck = connection.CreateCommand();
            existsCheck.CommandText = @$"SELECT EXISTS (SELECT Id FROM drinking_water WHERE Id={id});";

            int checkResult = Convert.ToInt32(existsCheck.ExecuteScalar());
            connection.Close();
            return checkResult;
        }
    }

    public string DeleteRecord(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string output = "";
            if (CheckIdExists(id) == 1)
            {
                var delete = connection.CreateCommand();
                delete.CommandText = @$"DELETE FROM drinking_water WHERE Id = {id}";
                delete.ExecuteNonQuery();
                output = "\nrecord was deleted\n";
            }
            else
            {
                output = $"\nrecord with iD:{id} could not be found\n";
            }
            connection.Close();
            return output;
        }
    }
    public string InsertHabit(string date, double quantity, string unit)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string output = "";

            try
            {
                var insert = connection.CreateCommand();
                insert.CommandText = $"INSERT INTO drinking_water (Date, Quanitiy, Unit) VALUES('{date}', {quantity}, '{unit}');";
                insert.ExecuteNonQuery();
                connection.Close();
                output = "\nrecord was inserted\n";
            }
            catch (Exception ex)
            {
                throw;
            }

            connection.Close();
            return output;
        }
    }

}
