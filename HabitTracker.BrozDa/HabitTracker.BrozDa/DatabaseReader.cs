using System.Data.SQLite;
namespace HabitTracker.BrozDa
{
    
    internal class DatabaseReader
    {
        //NOTE: a lot of repeated code with accessing the DB and retrieving information. 
        //Did not find any non-overcomplicated solution for this

        private string _databaseName;
        private string _connectionString;
        public string DateTimeFormat { get; init; }
        public DatabaseReader(string databaseName, string dateTimeFormat)
        {
            _databaseName = databaseName;
            _connectionString = @$"Data Source={databaseName};Version=3;";
            DateTimeFormat = dateTimeFormat;
        }
        public bool DoesDatabaseExist() => File.Exists(_databaseName);
       
        
        public string GetFromUnitTable(string table)
        {
            string sql = $"SELECT Unit FROM Units WHERE Habit='{table}';";
            string unit = string.Empty;
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) {
                            unit = reader.GetString(0);
                        }
                    }
                }
            }
            return unit;
        }
        
        public List<string> GetListOfTablesFromDatabase()
        {
            List<string> tables = new List<string>();

            string sql = $"SELECT name " +
                                   $"FROM sqlite_schema " +
                                   $"WHERE type ='table' AND " +
                                   $"name NOT LIKE 'sqlite_%' AND " +
                                   $"name NOT LIKE 'Units';";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return tables;
        }
        public List<DatabaseRecord> GetRecordsFromTable(string table)
        {
            List<DatabaseRecord> records = new List<DatabaseRecord>();

            string unit = GetFromUnitTable(table);

            string sql = $"SELECT * FROM {table};";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(new DatabaseRecord(reader.GetInt32(0), DateTime.Parse(reader.GetString(1)), reader.GetInt32(2)));
                        }
                    }
                }
            }
            return records;
        }
    }
}
