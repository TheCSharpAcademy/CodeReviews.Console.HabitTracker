using System.Data.SQLite;
namespace HabitTracker.BrozDa
{
    
    internal class DatabaseManager
    {
        private string _connectionString = @"Data Source=habit-tracker.sqlite;Version=3;";
        private InputOutputManager _inputOutputManager;
        public DatabaseManager()
        {
            _inputOutputManager = new InputOutputManager();
        }
        public void CreateNewTable(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) { 
                connection.Open();
                string sql = $"CREATE TABLE {tableName} (" +
                             $"ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                             $"Date varchar(255), " +
                             $"Glasses varchar(255)" +
                             $");";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public bool CheckIfTableExists(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = $"SELECT name " +
                             $"FROM sqlite_schema " +
                             $"WHERE type ='table' AND name ='{tableName}';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection); 
                
                try
                {
                    SQLiteDataReader output = cmd.ExecuteReader();
                    return output.HasRows;
                }
                catch (Exception ex) {
                    Console.WriteLine("Exception occured in DatabaseManager.DoesTableExist()");
                    Console.WriteLine(ex.ToString());
                }

                connection.Close();
                
            }
            return true;
        }
        public List<string> GetListOfTables()
        {
            List<string> tables = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT name " +
                                   $"FROM sqlite_schema " +
                                   $"WHERE type ='table';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                try
                {
                    SQLiteDataReader output = cmd.ExecuteReader();
                    while (output.Read()) {
                        tables.Add(output.GetString(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured in DatabaseManager.DoesTableExist()");
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }
            return tables;
        }
        public List<string> GetTableColumnNames(string tableName)
        {
            List<string> columns = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = $"SELECT name FROM pragma_table_info('{tableName}');";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader output = command.ExecuteReader();

                while (output.Read())
                {
                    columns.Add(output.GetString(0));
                }
                connection.Close();
            }
            return columns;
        }
        // conflicting thought - one one hand dont want handle printing in DBmanager class, but could not find other solution than saving whole DB to list and then sending it to IO manager - not very scalable
        public void PrintRecordsFromATable(string tableName)
        {
            _inputOutputManager.PrintTableColumns(GetTableColumnNames(tableName), tableName);

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string tableExist = $"SELECT * FROM {tableName};";
                SQLiteCommand cmd = new SQLiteCommand(tableExist, connection);
                SQLiteDataReader output = cmd.ExecuteReader();

                while (output.Read())
                {
                    _inputOutputManager.PrintRecord(new DatabaseRecord(output.GetInt32(0), output.GetString(1), output.GetString(2)));
                }
                connection.Close();
            }
            _inputOutputManager.PrintHorizonalLine();
        }
        public DatabaseRecord GetRecord(int ID)
        {
            DatabaseRecord record = new DatabaseRecord();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM WaterIntake " +
                             $"WHERE ID='{ID}';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                SQLiteDataReader output = cmd.ExecuteReader();
                while (output.Read())
                {
                    record = new DatabaseRecord(output.GetInt32(0), output.GetString(1), output.GetString(2));

                }

                connection.Close();
            }
            return record;
        }
        public DatabaseRecord GetRecord(int ID)
        {
            DatabaseRecord record = new DatabaseRecord();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM WaterIntake " +
                             $"WHERE ID='{ID}';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                SQLiteDataReader output = cmd.ExecuteReader();
                while (output.Read())
                {
                    record = new DatabaseRecord(output.GetInt32(0), output.GetString(1), output.GetString(2));

                }

                connection.Close();
            }
            return record;
        }
        public void InsertRecord(DatabaseRecord record, string table)
        {
<<<<<<< HEAD
=======
            Console.WriteLine($"Adding record: Date: {record.Date}, Volume: {record.Volume}");
>>>>>>> update-record
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            {
                connection.Open();
                string sql = $"INSERT INTO WaterIntake (Date, Glasses) " +
                             $"VALUES ('{record.Date}', '{record.Volume}');";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();
            
                connection.Close();
            }
            Console.WriteLine("Record added to the database");
        }
        public void UpdateRecord(DatabaseRecord UpdatedRecord, string table) {
            Console.WriteLine("Updating record...");
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"UPDATE {table} " +
                             $"SET Date='{UpdatedRecord.Date}', Glasses='{UpdatedRecord.Volume}' " +
                             $"WHERE ID={UpdatedRecord.ID};";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            }
            Console.WriteLine("Record updated");

        }
        public void DeleteRecord(int recordID, string table) 
        {
            Console.WriteLine("Deleting record...");

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = $"DELETE FROM {table} WHERE ID={recordID};";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();
                
                connection.Close();
            }
            Console.WriteLine("Record deleted");

        }
        /*public int GetNumberOfRows(string table)
        {
            int numberOfRows = -1;
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT COUNT(*) FROM {table};";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                SQLiteDataReader output = cmd.ExecuteReader();
                while (output.Read()) {
                    numberOfRows = output.GetInt32(0);
                }
                connection.Close();
            }
            return numberOfRows;
        }*/
        public bool IsIdPresentInDatabase(int id, string table) {
            
            bool isIdPresentInDatabase = false;

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT ID FROM {table} WHERE ID={id};";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                SQLiteDataReader output = cmd.ExecuteReader();
                while (output.Read())
                {
                    isIdPresentInDatabase = output.HasRows;
                }
                connection.Close();
            }
            return isIdPresentInDatabase;
        }
        public void UpdateRecord(DatabaseRecord UpdatedRecord, string table) {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"UPDATE {table} " +
                             $"SET Date='{UpdatedRecord.Date}', Glasses='{UpdatedRecord.Volume}' " +
                             $"WHERE ID={UpdatedRecord.ID};";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            }

        }


    }
}
