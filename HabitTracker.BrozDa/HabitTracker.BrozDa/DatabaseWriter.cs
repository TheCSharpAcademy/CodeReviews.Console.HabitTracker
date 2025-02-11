using System.Data.SQLite;

namespace HabitTracker.BrozDa
{
    internal class DatabaseWriter
    {
        private string _databaseName;
        private string _connectionString;
        public string DateTimeFormat { get; init; }

        public DatabaseWriter(string databaseName, string dateTimeFormat)
        {
            _databaseName = databaseName;
            _connectionString = @$"Data Source={databaseName};Version=3;";
            DateTimeFormat = dateTimeFormat;
        }
        public void CreateNewDatabase()
        {
            SQLiteConnection.CreateFile(_databaseName);
            CreateUnitTable();
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
        private void CreateUnitTable()
        {
            string sql = $"CREATE TABLE Units(" +
                        $"Habit TEXT," +
                        $"Unit TEXT" +
                        $");";

            ExecuteNonQuerry(sql);
        }
        private void InsertToUnitTable(string habit, string unit)
        {
            string sql = $"INSERT INTO Units (Habit, Unit) " +
                             $"VALUES (@habit ,@unit);";

            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@habit", habit),
                new SQLiteParameter("@unit", unit),

            };

            ExecuteNonQuerryWithParameters(sql, parameters);

        }
        public void DeleteTable(string table)
        {
            string sql = $"DROP TABLE '{table}';";

            ExecuteNonQuerry(sql);

        }
        public void InsertRecord(string table, DatabaseRecord record)
        {
            //Console.WriteLine($"Adding record ....");
            string sql = $"INSERT INTO {table} (Date, Volume) " +
                             $"VALUES (@recordDate, @recordVolume);";


            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@recordDate", record.Date),
                new SQLiteParameter("@recordVolume", record.Volume),

            };
            ExecuteNonQuerryWithParameters(sql , parameters);

        }
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
