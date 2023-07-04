using Microsoft.Data.Sqlite;

namespace HabbitTrackerJMS;

public class DataHelpers
{
    private readonly  string connectionString;
    public string TableName { set; get; }
    public DataHelpers()
    {
        TableName = "WaterDrinking";
        connectionString = @"Data Source = 
C:\Users\João Silva\source\repos\C# academy course\Math Game - maui\HabbitTrackerJMS\HabitTracker.db";
        CreateTable(TableName);
    }

    public void CreateTable(string TableName)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
           connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {TableName} 
                                (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                 Date TEXT,
                                 Quantity INTEGER)";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void InsertInfo(DateTime date, int quantity)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"INSERT INTO {TableName} (Date, Quantity) VALUES (@Date, @Quantity);";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

public void UpdateInfo(int id, DateTime date, int quantity)
{
    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        string query = $"UPDATE {TableName} SET Date = @Date, Quantity = @Quantity WHERE Id = @Id;";
        var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@Quantity", quantity);
        command.Parameters.AddWithValue("@Id", id);
        int countRows = command.ExecuteNonQuery();
        if (countRows == 0)
        {
            Console.WriteLine("The record does not exist!");
        }
        else
        {
            Console.WriteLine("Record updated!");
        }
        connection.Close();
    }
}

    public void DeleteInfo(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"DELETE FROM {TableName} WHERE Id = @Id;";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", id);
            int countRows = command.ExecuteNonQuery();
            if(countRows == 0)
            {
                Console.WriteLine("The record does not exist!");
            }
            else { Console.WriteLine("Record deleted!");}
            connection.Close();
        }
    }

    public int GetTotal()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            int total = 0;
            connection.Open();
            string query = $"SELECT SUM(Quantity) FROM {TableName}";
            var command = connection.CreateCommand();
            command.CommandText = query;
            var result = command.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                total = Convert.ToInt32(result);
            }
            connection.Close();
            return total;
        }
    }

    public bool CheckIDExists(int id)
    {
        bool idExists = false;
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT COUNT(*) FROM {TableName} WHERE Id = @Id;";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@Id", id);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if(count == 0)
            {
                Console.WriteLine("The ID does not exist on record");
                Console.ReadLine();
                idExists = false;
;
            }
            else { idExists = true; }
            connection.Close();
        }
        return idExists;
    }

    public List<HabitInfo> ViewInfo()
    {
        List<HabitInfo> habits = new List<HabitInfo>();
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string query = $"SELECT * FROM {TableName};";
            var command = connection.CreateCommand();
            command.CommandText = query;
            Console.WriteLine(TableName);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string date = reader.GetString(1);
                    int quantity = reader.GetInt32(2);
                    habits.Add(new HabitInfo(id, date, quantity));
                }
            }
        }
        return habits;
    }

    public List<string> GetTableNames()
    {
        List<string> tableNames = new List<string>();
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tableName = reader.GetString(0);
                    tableNames.Add(tableName);
                }
            }
            connection.Close();
        }
        return tableNames;
    }
}

public class HabitInfo
{
    public int Id { get; set; }
    public string Date { get; set; }
    public int Quantity { get; set; }

    public HabitInfo(int id, string date, int quantity)
    {
        Id = id;
        Date = date;
        Quantity = quantity;
    }
}
