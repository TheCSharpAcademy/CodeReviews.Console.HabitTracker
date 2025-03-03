using System.Data.SQLite;

namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Repository for managing <see cref="HabitRecord"/> entities in the database
    /// Implements <see cref="IHabitRecordRepository"/>
    /// </summary>
    internal class HabitRecordRepository : IHabitRecordRepository
    {
        private readonly string _connectionString;
        private const string tableName = "Habit Records";

        /// <summary>
        /// Initializes repository object
        /// </summary>
        /// <param name="connectionString"><see cref="string"/> value representing connection string for database access</param>
        public HabitRecordRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <inheritdoc/>
        public void CreateTable()
        {
            string sql = $"CREATE TABLE [{tableName}] (" +
                             $"ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                             $"Date TEXT," +
                             $"Volume INTEGER," +
                             $"HabitId INTEGER," +
                             $"FOREIGN KEY (HabitId) REFERENCES Habits(ID));";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public void Insert(HabitRecord entity)
        {
            string sql = $"INSERT INTO [{tableName}] (Date, Volume, HabitId) " +
                             $"VALUES (@recordDate, @recordVolume, @habitId);";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@recordDate", entity.Date);
            command.Parameters.AddWithValue("@recordVolume", entity.Volume);
            command.Parameters.AddWithValue("@habitId", entity.habitId);

            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public IEnumerable<HabitRecord> GetAll()
        {
            List<HabitRecord> records = new List<HabitRecord>();
            string sql = $"SELECT * FROM [{tableName}]";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);

            using SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new HabitRecord
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    Volume = reader.GetInt32(2),
                    habitId = reader.GetInt32(3),
                });
            }
            return records;
        }
        /// <inheritdoc/>
        public IEnumerable<int> GetIds()
        {
            List<int> ids = new List<int>();

            string sql = $"SELECT ID FROM [{tableName}]";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);

            using SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ids.Add(reader.GetInt32(0));
            }

            return ids;
        }
        /// <inheritdoc/>
        public IEnumerable<HabitRecord> GetAllByHabitID(int habitID)
        {
            List<HabitRecord> records = new List<HabitRecord>();
            string sql = $"SELECT * FROM [{tableName}] WHERE HabitId=@habitId";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@habitId", habitID);

            using SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new HabitRecord
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    Volume = reader.GetInt32(2),
                    habitId = reader.GetInt32(3),
                });
            }
            return records;
        }
        /// <inheritdoc/>
        public void Update(HabitRecord entity)
        {
            string sql = $"UPDATE [{tableName}] SET Date=@recordDate, Volume=@recordVolume WHERE Id=@id;";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@recordDate", entity.Date);
            command.Parameters.AddWithValue("@recordVolume", entity.Volume);
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public void Delete(HabitRecord entity)
        {
            string sql = $"DELETE From [{tableName}] Where Id=@id;";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public void DeleteAllByHabitId(int habitID)
        {
            string sql = $"DELETE From [{tableName}] Where HabitId=@habitId;";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@habitId", habitID);

            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public void InsertBulk(IEnumerable<HabitRecord> records)
        {
            string sql = $"INSERT INTO [Habit Records] (Date, Volume, HabitId) " +
                             $"VALUES (@recordDate, @recordVolume, @habitId);";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteTransaction transaction = connection.BeginTransaction();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.Add(new SQLiteParameter("@recordDate"));
            command.Parameters.Add(new SQLiteParameter("@recordVolume"));
            command.Parameters.Add(new SQLiteParameter("@habitId"));

            foreach(HabitRecord record in records)
            {
                command.Parameters["@recordDate"].Value = record.Date;
                command.Parameters["@recordVolume"].Value = record.Volume;
                command.Parameters["@habitId"].Value = record.habitId;
                command.ExecuteNonQuery();
            }
            transaction.Commit();
        }

    }
}
