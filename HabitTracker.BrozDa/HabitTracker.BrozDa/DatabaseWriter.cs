using System.Data.SQLite;

namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Represents object for reading and retreiving data from habit database
    /// </summary>
    internal class DatabaseWriter
    {
        private string _databaseName;
        private string _connectionString;
        public string DateTimeFormat { get; init; }

        /// <summary>
        /// Initializes new object of <see cref="DatabaseWriter"/> class
        /// </summary>
        /// <param name="databaseName">FileName of the database</param>
        /// <param name="dateTimeFormat">Date and time format which represents Date column of records in the database</param>
        public DatabaseWriter(string databaseName, string dateTimeFormat)
        {
            _databaseName = databaseName;
            _connectionString = @$"Data Source={databaseName};Version=3;";
            DateTimeFormat = dateTimeFormat;
        }
        /// <summary>
        /// Creates new file representing the habit database
        /// </summary>
        public void CreateNewDatabase()
        {
            SQLiteConnection.CreateFile(_databaseName);
            CreateUnitTable();
        }
        /// <summary>
        /// Executes non querrry SQL command
        /// </summary>
        /// <param name="sql"><see cref="string"/> representing SQL command</param>
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
        /// <summary>
        /// Executes non querrry SQL command with parameters
        /// </summary>
        /// <param name="sql"><see cref="string"/> representing SQL command</param>
        /// <param name="parameters">Array of <see cref="SQLiteParameter"/> commands</param>
        private void ExecuteNonQuerryWithParameters(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Creates a new table representing a habit
        /// </summary>
        /// <param name="table"><see cref="string"/> representing table name and the habit</param>
        /// <param name="unit"><see cref="string"/> representing the unit</param>
        public void CreateNewTable(string table, string unit)
        {
            InsertToUnitTable(table, unit);

            string sql = $"CREATE TABLE [{table}] (" +
                             $"ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                             $"Date TEXT," +
                             $"Volume INTEGER" +
                             $");";

            ExecuteNonQuerry(sql);
        }
        /// <summary>
        /// Creates a new table consisting units for tracked habits
        /// </summary>
        private void CreateUnitTable()
        {
            string sql = $"CREATE TABLE Units(" +
                        $"Habit TEXT," +
                        $"Unit TEXT" +
                        $");";

            ExecuteNonQuerry(sql);
        }
        /// <summary>
        /// Adds new record to unit table
        /// </summary>
        /// <param name="table"><see cref="string"/> representing table name and the habit</param>
        /// <param name="unit"><see cref="string"/> representing the unit</param>
        private void InsertToUnitTable(string table, string unit)
        {
            string sql = $"INSERT INTO Units (Habit, Unit) " +
                             $"VALUES (@habit ,@unit);";

            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@habit", table),
                new SQLiteParameter("@unit", unit),

            };

            ExecuteNonQuerryWithParameters(sql, parameters);
        }
        /// <summary>
        /// Removes specified table from the database
        /// </summary>
        /// <param name="table"><see cref="string"/> representing table name and the habit</param>
        public void DeleteTable(string table)
        {
            string sql = $"DROP TABLE '{table}';";

            ExecuteNonQuerry(sql);

        }
        /// <summary>
        /// Adds new record to habit table
        /// </summary>
        /// <param name="table"><see cref="string"/> representing table name and the habit</param>
        /// <param name="record"><see cref="DatabaseRecord"/> representing new record</param>
        public void InsertRecord(string table, DatabaseRecord record)
        {
            string sql = $"INSERT INTO {table} (Date, Volume) " +
                             $"VALUES (@recordDate, @recordVolume);";

            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@recordDate", record.Date),
                new SQLiteParameter("@recordVolume", record.Volume),

            };

            ExecuteNonQuerryWithParameters(sql , parameters);
        }
        /// <summary>
        /// Updates record in table with new values
        /// </summary>
        /// <param name="table"><see cref="string"/> representing table name and the habit</param>
        /// <param name="updatedRecord"><see cref="DatabaseRecord"/> representing updated record</param>
        public void UpdateRecord(string table, DatabaseRecord updatedRecord)
        {
            string sql = $"UPDATE {table} " +
                             $"SET Date=@recordDate, Volume=@recordVolume " +
                             $"WHERE ID=@recordID;";

            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@recordDate", updatedRecord.Date),
                new SQLiteParameter("@recordVolume", updatedRecord.Volume),
                new SQLiteParameter("@recordID", updatedRecord.ID),
            };
            ExecuteNonQuerryWithParameters (sql , parameters);  

        }
        /// <summary>
        /// Removes record from the database
        /// </summary>
        /// <param name="table"><see cref="string"/> representing table name and the habit</param>
        /// <param name="recordID"><see cref="int"/> representing unique ID of the record, validation have to be done outside</param>
        public void DeleteRecord(string table, int recordID)
        {
            string sql = $"DELETE FROM {table} WHERE ID=@recordID;";

            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@recordID", recordID),
            };

            ExecuteNonQuerryWithParameters(sql,parameters);

        }
    }
}
