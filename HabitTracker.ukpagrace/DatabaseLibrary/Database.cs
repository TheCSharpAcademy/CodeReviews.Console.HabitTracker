using Microsoft.Data.Sqlite;
using System.Globalization;
namespace DatabaseLibrary
{
    public class Database
    {
        readonly string connectionString = @"Data Source=HabitTracker.db";
        public List<Table> tableData = new();
        public List<string> table = new();
        public void Create(string habit, string unitofmeasurement)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = @$"CREATE TABLE IF NOT EXISTS {habit}_{unitofmeasurement} (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";
            TableCommand.ExecuteNonQuery();
            connection.Close();
        }

        public void GetTables()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = @"SELECT name FROM sqlite_schema where type = 'table' AND name NOT LIKE 'sqlite_%'";


            SqliteDataReader reader = TableCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    table.Add(reader.GetString(0));
                }
            }
            else
            {
                Console.WriteLine("No Habits Created");
            }
            connection.Close();
        }

        public void Insert(string table, string date, int quantity)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = $"INSERT INTO {table} (Date, Quantity) VALUES('{date}',{quantity})";
            TableCommand.ExecuteNonQuery();
            connection.Close();
        }

        public void Retrieve(string table)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = $"SELECT * FROM {table}";
            SqliteDataReader reader = TableCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new Table
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", CultureInfo.InvariantCulture),
                            Quantity = reader.GetInt32(2)
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine("No Rows Found");
            }
            connection.Close();
        }

        public void Update(string table, string date, int quantity, int id)
        {
            var connection = new SqliteConnection(connectionString);

            connection.Open();

            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = $"UPDATE {table} SET Date = '{date}',Quantity={quantity} where Id = {id}";

            TableCommand.ExecuteNonQuery();
            connection.Close();
        }

        public void Delete(string table, int id)
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = $"DELETE FROM {table} where Id={id}";

            TableCommand.ExecuteNonQuery();
            connection.Close();

        }

        public void Analysis(string table)
        {
            List<ReportData> report = new();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = @$"SELECT 
                substr(Date, 7, 4) || '-' || substr(Date, 4, 2) as month,
                COUNT(id) as entryCount,
                SUM(quantity) as achieved
                FROM 
                    {table}
                GROUP BY month
                ORDER BY month desc;
            ";

            SqliteDataReader reader = TableCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    report.Add(
                        new ReportData
                        {
                            Month = reader.GetString(0),
                            EntryCount = reader.GetInt32(1),
                            Achieved = reader.GetInt32(2)
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine("No Habits Created");
            }
            connection.Close();


            Console.WriteLine("------------------------------\n\n");
            Console.WriteLine($"Month\tEntryCount\tAchieved");
            foreach (var data in report)
            {
                Console.WriteLine($"{data.Month}\t{data.EntryCount}\t\t{data.Achieved}");
            }
            Console.WriteLine("------------------------------\n");
        }

        public bool TableHasRows(string table)
        {
            bool hasRows;
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var TableCommand = connection.CreateCommand();
            TableCommand.CommandText = @$"SELECT 
                Id 
                FROM 
                    {table}
                LIMIT 1;
            ";

            SqliteDataReader reader = TableCommand.ExecuteReader();
            hasRows = reader.HasRows;
            connection.Close();
            return hasRows;
        }
    }

    public class Table()
    {
        public int Id { set; get; }
        public DateTime Date { set; get; }
        public int Quantity { set; get; }
    }

    class ReportData()
    {
        public string Month { set; get; }
        public int EntryCount { set; get; }
        public int Achieved { set; get; }
    }
}
