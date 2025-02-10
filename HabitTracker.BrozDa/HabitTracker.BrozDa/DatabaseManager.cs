using System.Data.SQLite;
namespace HabitTracker.BrozDa
{
    
    internal class DatabaseManager
    {
        private readonly string _databaseName = "habit-tracker.sqlite";
        private string _connectionString;
        private InputOutputManager _inputOutputManager;
        public string DateTimeFormat { get; init; }
        public DatabaseManager(string dateTimeFormat)
        {
            _connectionString = @$"Data Source={_databaseName};Version=3;";
            DateTimeFormat = dateTimeFormat;
        }
        public bool DoesDatabaseExist()
        {
            return File.Exists(_databaseName);
        }
        public void CreateNewDatabase()
        {
            SQLiteConnection.CreateFile(_databaseName);
            CreateUnitTable();
        }
        public void CreateNewTable(string table, string unit)
        {
            InsertToUnitTable(table, unit);

            string sql = $"CREATE TABLE [{table}] (" +
                             $"ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                             $"Date TEXT," +
                             $"Volume INTEGER" + 
                             $");";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            
        }
        private void CreateUnitTable()
        {
            string sql = $"CREATE TABLE Units(" +
                        $"Habit TEXT," +
                        $"Unit TEXT" +
                        $");";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void InsertToUnitTable(string habit, string unit)
        {
            string sql = $"INSERT INTO Units (Habit, Unit) " +
                             $"VALUES (@habit ,@unit);";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@habit", habit);
                    command.Parameters.AddWithValue("@unit", unit);
                    command.ExecuteNonQuery();
                }
            }
        }
        private string GetFromUnitTable(string table)
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

        // conflicting thought - one one hand dont want handle printing in DBmanager class, but could not find other solution than saving whole DB to list and then sending it to IO manager - not very scalable
        public void PrintRecordsFromATable(string table)
        {
            _inputOutputManager = new InputOutputManager(DateTimeFormat);
            _inputOutputManager.PrintTableColumns(GetTableColumnNames(table), table);

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
                            _inputOutputManager.PrintRecord(new DatabaseRecord(reader.GetInt32(0), DateTime.Parse(reader.GetString(1)), reader.GetInt32(2)), unit);
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
                             $"WHERE ID=@ID;";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("ID", ID);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            record.ID = reader.GetInt32(0);
                            record.Date = DateTime.Parse(reader.GetString(1));
                            record.Volume = reader.GetInt32(2);
                        }
                    }
                }
            }

            return record;
        }
        public void InsertRecord(string table, DatabaseRecord record)
        {
            Console.WriteLine($"Adding record ....");
            string sql = $"INSERT INTO {table} (Date, Volume) " +
                             $"VALUES (@recordDate, @recordVolume);";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@recordDate", record.Date);
                    command.Parameters.AddWithValue("@recordVolume", record.Volume);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Record added to the database");

        }
        public void UpdateRecord(string table, DatabaseRecord updatedRecord ) 
        {
            Console.WriteLine("Updating record...");
            string sql = $"UPDATE {table} " +
                             $"SET Date=@recordDate, Volume=@recordVolume " +
                             $"WHERE ID=@recordID;";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@recordDate", updatedRecord.Date);
                    command.Parameters.AddWithValue("@recordVolume", updatedRecord.Volume);
                    command.Parameters.AddWithValue("@recordID", updatedRecord.ID);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Record Updated");
        }
        public void DeleteRecord(string table, int recordID) 
        {
            Console.WriteLine("Deleting record...");
            string sql = $"DELETE FROM {table} WHERE ID=@recordID;";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@recordID", recordID);
                    command.ExecuteNonQuery();
                }
            }

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
