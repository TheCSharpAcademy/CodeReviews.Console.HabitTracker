using Microsoft.Data.Sqlite;
using static habit_tracker.Program;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace habit_tracker;

class SqlCommands
{
    public class TableStructure()
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Unit {get; set; }
    }

    static string connectionString = @"Data Source=Habit-Tracker.db";
    public void SqlInitialize()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER,
                    Unit TEXT DEFAULT ""Cups"" NOT NULL)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public List<string> SqlGetTables()
    {
        List<string> tables = new();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = 
                @"SELECT name FROM sqlite_schema 
                WHERE type = 'table' 
                AND name Not LIKE 'sqlite_%'";

            SqliteDataReader reader = tableCmd.ExecuteReader();
            while (reader.Read())
            {
                tables.Add(reader.GetString(0)); 
            }
            connection.Close();           
        }
        return tables;
    }
    public void SqlInsertAction(string tableName, string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO {tableName}(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void SqlViewAction(string tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {tableName} ";

            List<TableStructure> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new TableStructure
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-mm-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                            Unit = reader.GetString(3)
                        });
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            connection.Close();

            Console.WriteLine("------------------------------------------------");
            foreach (var record in tableData)
            {
                Console.WriteLine($"{record.Id} | {record.Date.ToString("dd-mm-yyyy")} | Quantity: {record.Quantity} {record.Unit}");
            }
            Console.WriteLine("------------------------------------------------\n");
        }
    }

    public bool SqlDeleteAction(string tableName, int recordId)
    {
        bool zeroRow = false;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE from {tableName} WHERE Id = '{recordId}'";
            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                return zeroRow = true;
            }
        }
        return zeroRow;
    }

    public bool SqlUpdateActionCheck(string tableName, int recordId)
    {
        bool zeroRow = false;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                connection.Close();
                return zeroRow = true;
            }
            connection.Close();
        }
        return zeroRow;
    }

    public void SqlUpdateAction(string tableName,int recordId, string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"UPDATE {tableName} SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void sqlCreateTable(string tableName, string measurementUnit)
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = 
                @$"CREATE TABLE IF NOT EXISTS {tableName} (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER,
                Unit TEXT DEFAULT '{measurementUnit}' NOT NULL)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
