using Dapper;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class LocalDatabaseService
    {
        private readonly string _connectionString;
        private SqliteConnection? _connection;

        public LocalDatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            Init();
        }

        private void Init()
        {
            if (_connection != null)
            {
                return;
            }

            try
            {
                _connection = new SqliteConnection(_connectionString);
                CreateTables();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateTables()
        {
            CreateTableHabits();
            CreateTableHabitRecords();
        }

        private void CreateTableHabits()
        {
            string newTableSql =
                $"CREATE TABLE IF NOT EXISTS {Constants.TableHabits} (" +
                $"Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                $"Name TEXT NOT NULL," +
                $"MeasurementMethod TEXT NOT NULL);";
            CreateTable(Constants.TableHabits, newTableSql.ToString());
        }

        private void CreateTableHabitRecords()
        {
            string newTableSql =
                $"CREATE TABLE IF NOT EXISTS {Constants.TableHabitRecords} (" +
                $"Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                $"Date TEXT NOT NULL," +
                $"NumberOfApproachesPerDay INTEGER NOT NULL," +
                $"HabitId INTEGER," +
                $"FOREIGN KEY(HabitId) REFERENCES {Constants.TableHabits}(Id) ON DELETE CASCADE);";
            CreateTable(Constants.TableHabitRecords, newTableSql.ToString());
        }

        private void CreateTable(string tableName, string newTableSql)
        {
            var tableExists = _connection!.Query<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name = '{tableName}';").FirstOrDefault();
            if (!string.IsNullOrEmpty(tableExists) && tableExists == tableName)
            {
                return;
            }
            _connection!.Execute(newTableSql);
        }

        public void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }

        // Habits
        public Habit CreateHabit(Habit obj)
        {
            string createQuery = $"INSERT INTO {Constants.TableHabits} (Name, MeasurementMethod) VALUES ('{obj.Name}', '{obj.MeasurementMethod}') RETURNING *;";
            return _connection!.Query<Habit>(createQuery).First();
        }

        public Habit GetLastHabit()
        {
            string getLastQuery = $"SELECT * FROM {Constants.TableHabits} ORDER BY Id DESC LIMIT 1;";
            return _connection!.Query<Habit>(getLastQuery).First();
        }

        // HabitRecords
        public HabitRecord CreateHabitRecord(HabitRecord obj, int habitId)
        {
            string createQuery = $"INSERT INTO {Constants.TableHabitRecords} (Date, NumberOfApproachesPerDay, HabitId) VALUES ('{obj.Date}', '{obj.NumberOfApproachesPerDay}', '{habitId}') RETURNING *;";
            return _connection!.Query<HabitRecord>(createQuery).First();
        }

        public IEnumerable<HabitRecord> GetAllHabitRecords(int habitId)
        {
            string getAllQuery = $"SELECT * FROM {Constants.TableHabitRecords} WHERE HabitId={habitId};";
            return _connection!.Query<HabitRecord>(getAllQuery);
        }

        public bool IsExistDateRecord(DateTime date)
        {
            string isExistDateQuery = $"SELECT * FROM {Constants.TableHabitRecords} WHERE Date='{date}';";
            return _connection!.Query<HabitRecord>(isExistDateQuery).ToArray().Length > 0;
        }

        public HabitRecord? GetHabitRecordById(int id, int habitId)
        {
            string isExistDateQuery = $"SELECT * FROM {Constants.TableHabitRecords} WHERE Id={id} AND HabitId={habitId};";
            return _connection!.Query<HabitRecord>(isExistDateQuery).FirstOrDefault();
        }

        public HabitRecord UpdateHabitRecord(HabitRecord obj)
        {
            string updateQuery = $"REPLACE INTO {Constants.TableHabitRecords} (Id, Date, NumberOfApproachesPerDay, HabitId) VALUES ({obj.Id}, '{obj.Date}', '{obj.NumberOfApproachesPerDay}', '{obj.HabitId}') RETURNING *;";
            return _connection!.Query<HabitRecord>(updateQuery).First();
        }

        public HabitRecord? DeleteHabitRecord(int id, int habitId)
        {
            string deleteQuery = $"DELETE FROM {Constants.TableHabitRecords} WHERE Id={id} AND HabitId={habitId} RETURNING *;";
            return _connection!.Query<HabitRecord>(deleteQuery).FirstOrDefault();
        }
    }
}
