namespace DataBaseLibrary;
using Microsoft.Data.Sqlite;
using System.Globalization;


public class DataBaseCommands
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    public void Initialization(string habitsTableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            // AUTOINCREMENT - everytime an entry is added, it will increment
            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS "+
                    habitsTableName +
                    "(Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "HabitTableName TEXT," +
                    "HabitUnit TEXT)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void CreateHabitTable(string tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            // AUTOINCREMENT - everytime an entry is added, it will increment
            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS " +
                    tableName +
                    "(Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "Date TEXT," +
                    "Quantity TEXT)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }


    public void Insert(string tableName, string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO " +
                tableName +
                $"(date, quantity) VALUES ('{date}',{quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    public void Insert(string date, int quantity)
    {
        return;
    }

    public void Insert(string habitsTableName,string habitTableName, string habitUnit)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO " +
                habitsTableName +
                "(HabitTableName, HabitUnit)" +
                $" VALUES ('{habitTableName}','{habitUnit}')";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public bool CheckIndex(int index, string tableName) 
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM " +
                tableName +
                $" WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }

    public bool DeleteByIndex(int index, string tableName) 
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE from " +
                tableName +
                $" WHERE Id = '{index}'";

            int rowCount = tableCmd.ExecuteNonQuery();
            connection.Close();
            if (rowCount == 0 ) return false;
            else return true;
        }
    }

    public bool UpdateByIndex(int index, string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                connection.Close();
                return false;
            }

            else 
            {
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} " +
                    $"WHERE Id = {index}";

                tableCmd.ExecuteNonQuery();
                
                connection.Close();

                return true; 
            }
        }
    }   

    public void ViewAll()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    }); ;
                }
            }
            else { Console.WriteLine("Empty"); }

            connection.Close();

            Console.WriteLine("-----------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("\n-----------------------------");
        }
    }

    public SqliteDataReader GetReader(string? db_name)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM " + db_name;

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (!reader.HasRows) reader = null;

            return reader;
        }
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }

    public class Habit
    {
        public int Id { get; set; }
        public string? HabitDB { get; set; }
        public string? Habit_unit { get; set;}

    }

}