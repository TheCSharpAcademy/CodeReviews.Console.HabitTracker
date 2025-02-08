using System.Data.SQLite;
namespace HabitTracker.BrozDa
{
    
    internal class DatabaseManager
    {
        private string _connectionString = @"Data Source=habit-tracker.sqlite;Version=3;";
        private InputOutputManager _inputOutputManager;
        public string DateTimeFormat { get; init; }
        public DatabaseManager(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
        private void ExecuteNonQuerry(string sql)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public void CreateNewTable(string table)
        {
            string sql = $"CREATE TABLE {table} (" +
                             $"ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                             $"Date varchar(255), " +
                             $"Glasses varchar(255)" +
                             $");";

            ExecuteNonQuerry(sql);
        }
        public bool CheckIfTableExists(string table)
        {
            bool doesTableExist;

            string sql = $"SELECT name " +
                             $"FROM sqlite_schema " +
                             $"WHERE type ='table' AND name ='{table}';";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    doesTableExist = command.ExecuteReader().HasRows;
                }
            }

            return doesTableExist;
        }
        public List<string> GetListOfTablesFromDatabase()
        {
            List<string> tables = new List<string>();

            string sql = $"SELECT name " +
                                   $"FROM sqlite_schema " +
                                   $"WHERE type ='table';";

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
        public List<string> GetTableColumnNames(string table)
        {
            List<string> columns = new List<string>();

            string sql = $"SELECT name FROM pragma_table_info('{table}');";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return columns;
        }
        // conflicting thought - one one hand dont want handle printing in DBmanager class, but could not find other solution than saving whole DB to list and then sending it to IO manager - not very scalable
        public void PrintRecordsFromATable(string table)
        {
            _inputOutputManager = new InputOutputManager(DateTimeFormat);
            _inputOutputManager.PrintTableColumns(GetTableColumnNames(table), table);
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
                            _inputOutputManager.PrintRecord(new DatabaseRecord(reader.GetInt32(0), DateTime.Parse(reader.GetString(1)), reader.GetString(2)));
                        }
                    }
                }
            }
            //print bottom line on the table 
            _inputOutputManager.PrintHorizonalLine();
        }
        public DatabaseRecord GetRecordUsingID(string table, int ID)
        {
            DatabaseRecord record = new DatabaseRecord();
            string sql = $"SELECT * FROM {table} " +
                             $"WHERE ID='{ID}';";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            record.ID = reader.GetInt32(0);
                            record.Date = DateTime.Parse(reader.GetString(1));
                            record.Volume = reader.GetString(2);
                        }
                    }
                }
            }

            return record;
        }
        public void InsertRecord(string table, DatabaseRecord record)
        {
            Console.WriteLine($"Adding record: Date: {record.Date.ToString(DateTimeFormat)}, Volume: {record.Volume}");
            string sql = $"INSERT INTO WaterIntake (Date, Glasses) " +
                             $"VALUES ('{record.Date}', '{record.Volume}');";

            ExecuteNonQuerry(sql);

            Console.WriteLine("Record added to the database");

        }
        public void UpdateRecord(string table, DatabaseRecord updatedRecord ) 
        {
            Console.WriteLine("Updating record...");
            string sql = $"UPDATE {table} " +
                             $"SET Date='{updatedRecord.Date}', Glasses='{updatedRecord.Volume}' " +
                             $"WHERE ID={updatedRecord.ID};";

            ExecuteNonQuerry(sql);

            Console.WriteLine("Record Updated");
        }
        public void DeleteRecord(string table, int recordID) 
        {
            Console.WriteLine("Deleting record...");
            string sql = $"DELETE FROM {table} WHERE ID={recordID};";

            ExecuteNonQuerry(sql);

            Console.WriteLine("Record deleted");
        }
        public bool IsIdPresentInDatabase(string table, int id) {
            
            bool isIdPresentInDatabase = false;
            string sql = $"SELECT ID FROM {table} WHERE ID={id};";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            isIdPresentInDatabase = reader.HasRows;
                        }
                    }
                }
            }
            return isIdPresentInDatabase;
        }
    }
}
