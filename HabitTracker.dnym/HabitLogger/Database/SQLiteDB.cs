using HabitLogger.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger.Database
{
    internal class SQLiteDB : IDatabase
    {
        private const string _createTableSql = @"CREATE TABLE IF NOT EXISTS ""records"" (
	""id""	INTEGER NOT NULL UNIQUE,
	""datetime""	TEXT NOT NULL,
	""quantity""	INTEGER NOT NULL,
	PRIMARY KEY(""id"" AUTOINCREMENT)
);";
        private const string _insertRecordSql = "INSERT INTO \"records\" (\"datetime\", \"quantity\") VALUES (@datetime, @quantity);";
        private const string _selectRecordSql = "SELECT \"id\", \"datetime\", \"quantity\" FROM \"records\" WHERE \"id\" = @id;";
        private const string _selectSubsetSql = "SELECT \"id\", \"datetime\", \"quantity\" FROM \"records\" ORDER BY \"datetime\" DESC LIMIT @limit OFFSET @skip;";
        private const string _countRecordsSql = "SELECT COUNT(*) FROM \"records\";";
        private const string _updateRecordSql = "UPDATE \"records\" SET \"datetime\" = @datetime, \"quantity\" = @quantity WHERE \"id\" = @id;";
        private const string _deleteRecordSql = "DELETE FROM \"records\" WHERE \"id\" = @id;";

        private readonly string _connectionString;

        public SQLiteDB(string connectionString)
        {
            _connectionString = connectionString;
            using var connection = new SqliteConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to create table: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = _createTableSql;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to create table: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            connection.Close();
        }

        public void InsertRecord(HabitRecord record)
        {
            using var connection = new SqliteConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to insert record: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = _insertRecordSql;
            cmd.Parameters.AddWithValue("@datetime", record.Date.ToUniversalTime().ToString("O"));
            cmd.Parameters.AddWithValue("@quantity", record.Quantity);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to insert record: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            connection.Close();
        }

        public HabitRecord? GetRecord(int id)
        {
            using var connection = new SqliteConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to get record with id={id}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = _selectRecordSql;
            cmd.Parameters.AddWithValue("@id", id);

            HabitRecord? output = null;
            try
            {
                using var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    output = new HabitRecord()
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        Quantity = reader.GetInt32(2)
                    };
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to get record with id={id}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Record with id={id} has bad data: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            return output;
        }

        public List<HabitRecord> GetRecords(int limit = int.MaxValue, int skip = 0)
        {
            using var connection = new SqliteConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to get records: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = _selectSubsetSql;
            cmd.Parameters.AddWithValue("@limit", limit);
            cmd.Parameters.AddWithValue("@skip", skip);

            var output = new List<HabitRecord>();
            try
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    output.Add(new HabitRecord()
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        Quantity = reader.GetInt32(2)
                    });
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to get records: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Record has bad data: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            return output;
        }

        public int GetRecordsCount()
        {
            using var connection = new SqliteConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to count records: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = _countRecordsSql;
            var output = 0;

            try
            {
                if (cmd.ExecuteScalar() is long result)
                {
                    output = (int)result;
                }
                else
                {
                    Console.WriteLine("Failed to count records!\nAborting!");
                    Environment.Exit(1);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to count records: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            return output;
        }

        public void UpdateRecord(HabitRecord record)
        {
            using var connection = new SqliteConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to update record with id={record.Id}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = _updateRecordSql;
            cmd.Parameters.AddWithValue("@id", record.Id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to update record with id={record.Id}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }

        public void DeleteRecord(int id)
        {
            using var connection = new SqliteConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to delete record with id={id}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = _deleteRecordSql;
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Failed to delete record with id={id}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
    }
}
