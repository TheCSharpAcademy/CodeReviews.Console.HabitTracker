using System.Data.SQLite;
namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Represents object for reading and retreiving data from habit database
    /// </summary>
    internal class DatabaseReader
    {
        //NOTE: a lot of repeated code with accessing the DB and retrieving information. 
        //Did not find any non-overcomplicated solution for this


        private string _databaseName;
        private string _connectionString;
        public string DateTimeFormat { get; init; }

        /// <summary>
        /// Initializes new object of <see cref="DatabaseReader"> class
        /// </summary>
        /// <param name="databaseName">FileName of the database</param>
        /// <param name="dateTimeFormat">Date and time format which represents Date column of records in the database</param>
        public DatabaseReader(string databaseName, string dateTimeFormat)
        {
            _databaseName = databaseName;
            _connectionString = @$"Data Source={databaseName};Version=3;";
            DateTimeFormat = dateTimeFormat;
        }
        /// <summary>
        /// Checks whether the database is already created
        /// </summary>
        /// <returns><see cref="bool"/> true if it exists, false otherwise</returns>
        public bool DoesDatabaseExist() => File.Exists(_databaseName);
       
        /// <summary>
        /// Get unit for current table
        /// Unit table is made of two columns Habit and unit, where habit represents name of the table of particular habit
        /// </summary>
        /// <param name="table"><see cref="string"/>representing the habit</param>
        /// <returns><see cref="string"/> representing the unit</returns>
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

        /// <summary>
        /// Gets <see cref="List{string}"/> of current tables representing tracked habits
        /// </summary>
        /// <returns><see cref="List{string}"/> of current tables representing tracked habits</returns>
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
        /// <summary>
        /// Gets <see cref="List{DatabaseRecord}"/> of all records in table
        /// </summary>
        /// <param name="table"><see cref="string"/> representing name of the table</param>
        /// <returns><see cref="List{DatabaseRecord}"/> of all records in table</returns>
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
