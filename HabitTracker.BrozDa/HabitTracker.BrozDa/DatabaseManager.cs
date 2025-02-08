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
        public void CreateNewTable(string table)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            { 
                string sql = $"CREATE TABLE {table} (" +
                             $"ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                             $"Date varchar(255), " +
                             $"Glasses varchar(255)" +
                             $");";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public bool CheckIfTableExists(string table)
        {
            bool doesTableExist;

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            {
                string sql = $"SELECT name " +
                             $"FROM sqlite_schema " +
                             $"WHERE type ='table' AND name ='{table}';";

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();
                doesTableExist = command.ExecuteReader().HasRows;
                connection.Close();
            }
            return doesTableExist;
        }
        // NOT CLEANED YET
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
        public List<string> GetTableColumnNames(string table)
        {
            List<string> columns = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            {
                string sql = $"SELECT name FROM pragma_table_info('{table}');";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();
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
        public void PrintRecordsFromATable(string table)
        {
            _inputOutputManager = new InputOutputManager(DateTimeFormat);
            _inputOutputManager.PrintTableColumns(GetTableColumnNames(table), table);

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                string sql = $"SELECT * FROM {table};";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();
                SQLiteDataReader output = command.ExecuteReader();

                while (output.Read())
                {
                    _inputOutputManager.PrintRecord(new DatabaseRecord(output.GetInt32(0), DateTime.Parse(output.GetString(1)), output.GetString(2)));
                }
                connection.Close();
            }
            //bottom line on the table
            _inputOutputManager.PrintHorizonalLine();
        }
        public DatabaseRecord GetRecordUsingID(string table, int ID)
        {
            DatabaseRecord record = new DatabaseRecord();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                string sql = $"SELECT * FROM {table} " +
                             $"WHERE ID='{ID}';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);

                connection.Open();
                SQLiteDataReader output = cmd.ExecuteReader();

                while (output.Read())
                {
                    record.ID = output.GetInt32(0);
                    record.Date = DateTime.Parse(output.GetString(1));
                    record.Volume = output.GetString(2);
                }

                connection.Close();
            }
            return record;
        }
        public void InsertRecord(string table, DatabaseRecord record)
        {
            Console.WriteLine($"Adding record: Date: {record.Date.ToString(DateTimeFormat)}, Volume: {record.Volume}");

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            {

                string sql = $"INSERT INTO WaterIntake (Date, Glasses) " +
                             $"VALUES ('{record.Date}', '{record.Volume}');";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine("Record added to the database");
        }
        public void UpdateRecord(string table, DatabaseRecord updatedRecord ) {
            Console.WriteLine("Updating record...");
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                string sql = $"UPDATE {table} " +
                             $"SET Date='{updatedRecord.Date}', Glasses='{updatedRecord.Volume}' " +
                             $"WHERE ID={updatedRecord.ID};";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            Console.WriteLine("Record updated");
        }
        public void DeleteRecord(string table, int recordID) 
        {
            Console.WriteLine("Deleting record...");

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            {
                string sql = $"DELETE FROM {table} WHERE ID={recordID};";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            Console.WriteLine("Record deleted");
        }
        public bool IsIdPresentInDatabase(string table, int id) {
            
            bool isIdPresentInDatabase = false;

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                string sql = $"SELECT ID FROM {table} WHERE ID={id};";
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                connection.Open();
                SQLiteDataReader output = command.ExecuteReader();

                while (output.Read())
                {
                    isIdPresentInDatabase = output.HasRows;
                }
                connection.Close();
            }
            return isIdPresentInDatabase;
        }
    }
}
