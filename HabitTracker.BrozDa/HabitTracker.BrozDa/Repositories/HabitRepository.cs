using System.Data.SQLite;

namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Repository for managing <see cref="Habit"/> entities in the database
    /// Implements <see cref="IHabitRepository"/>
    /// </summary>
    internal class HabitRepository : IHabitRepository
    {
        private readonly string _connectionString;
        private readonly string tableName = "Habits";
        /// <summary>
        /// Initializes repository object
        /// </summary>
        /// <param name="connectionString"><see cref="string"/> value representing connection string for database access</param>
        public HabitRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <inheritdoc/>
        public void CreateTable()
        {
            string sql = $"CREATE TABLE [{tableName}] (" +
                        $"ID INTEGER PRIMARY KEY AUTOINCREMENT," +
                        $"Habit TEXT," +
                        $"Unit TEXT" +
                        $");";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public void Insert(Habit entity)
        {
            string sql = $"INSERT INTO [{tableName}] (Habit, Unit) " +
                             $"VALUES (@habit ,@unit);";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand( sql, connection);
            command.Parameters.AddWithValue("@habit", entity.Name);
            command.Parameters.AddWithValue("@unit", entity.Unit);

            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public void Update(Habit entity)
        {
            string sql = $"UPDATE [{tableName}] SET Habit=@habit, Unit=@unit WHERE Id=@id;";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@habit", entity.Name);
            command.Parameters.AddWithValue("@unit", entity.Unit);
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public void Delete(Habit entity)
        {
            string sql = $"DELETE From [{tableName}] Where Id=@id;";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", entity.Id);

            command.ExecuteNonQuery();
        }
        /// <inheritdoc/>
        public IEnumerable<Habit> GetAll()
        {
            List<Habit> records = new List<Habit>();
            string sql = $"SELECT * FROM [{tableName}]";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql,connection);

            using SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                records.Add(new Habit { Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Unit = reader.GetString(2)
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
        public void InsertBulk(IEnumerable<Habit> records) 
        {

            string sql = $"INSERT INTO [{tableName}] (Habit, Unit) " +
                             $"VALUES (@habit ,@unit);";

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteTransaction transaction = connection.BeginTransaction();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);

            command.Parameters.Add(new SQLiteParameter("@habit"));
            command.Parameters.Add(new SQLiteParameter("@unit"));

            foreach(var record in records)
            {
                command.Parameters["@habit"].Value = record.Name;
                command.Parameters["@unit"].Value = record.Unit;

                command.ExecuteNonQuery();
            }

            transaction.Commit();

        }
        /// <inheritdoc/>
        public Habit GetHabitById(int habitId)
        {
            string sql = "Select * From [Habits] Where Id=@id;";
            Habit habit = new Habit();

            using SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.Add(new SQLiteParameter("@id", habitId));

            using SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                habit.Id = reader.GetInt32(0);
                habit.Name = reader.GetString(1);
                habit.Unit = reader.GetString(2);
            }
            
            return habit;
        }

        
    }
}
