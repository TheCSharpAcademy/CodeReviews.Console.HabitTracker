using System.Data;
using System.Data.SQLite;
using Dapper;
using StatLibrary;

namespace DatabaseHandler
{
    public sealed class Database
    {
        private readonly string _databaseName;
        private readonly string _connectionString;

        // Create a new table if one doesn't exist when DB is created
        public Database(string databaseName)
        {
            _databaseName = databaseName;
            _connectionString = $"Data Source={_databaseName}.sqlite;Version=3;";
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string initializeQuery = @"CREATE TABLE IF NOT EXISTS Habits (
                                        record_id INTEGER PRIMARY KEY,
                                        habit_name TEXT NOT NULL,
                                        stat_name TEXT NOT NULL,
                                        stat_value INTEGER NOT NULL,
                                        entry_timestamp TEXT NOT NULL)";
            connection.Execute(initializeQuery);
        }
        // Create a new habit record with the following columns:
        // RecordId | HabitName | StatName | StatCount | EntryTimestamp
        public void Create(string habitName, Stat stat)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string testInsert = @"INSERT INTO Habits (habit_name, stat_name, stat_value, entry_timestamp)
                                    VALUES (@HabitName, @StatName, @StatValue, @Date)";
            DateTime dateTime = DateTime.Now;
            string todayFormatted = dateTime.ToString("yyyy-MM-dd");
            connection.Execute(testInsert, new { HabitName = habitName, StatName = stat.Name, StatValue = stat.Value, Date = todayFormatted });
        }

        // Delete ALL RecordIds matching the given HabitName column
        public int Drop(string habitName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string dropQuery = @"DELETE FROM Habits WHERE habit_name = @HabitName";
            return connection.Execute(dropQuery, new { HabitName = habitName });
        }

        // Return ALL entries of a given habit
        public List<Record> Read(string habitName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string readQuery = @"SELECT habit_name, stat_name, stat_value, entry_timestamp 
                                    FROM Habits 
                                    WHERE habit_name = @HabitName";
            var records = connection.Query<Record>(readQuery, new { HabitName = habitName });
            return records.ToList();
        }

        // Update ALL RecordIds with matching HabitName column
        public void UpdateHabitName(string oldName, string newName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string updateNameQuery = $"UPDATE Habits SET habit_name = @HabitName WHERE habit_name = @OldName";

            try
            {
                connection.Execute(updateNameQuery, new { HabitName = newName, OldName = oldName });
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while updating habit names: {ex}");
            }
        }

        // Update ALL RecordIds with matching HabitName AND StatName columns
        public void UpdateStatName(string habitName, string newName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string updateNameQuery = $"UPDATE Habits SET stat_name = @StatName WHERE habit_name = @HabitName";

            try
            {
                connection.Execute(updateNameQuery, new { StatName = newName, HabitName = habitName });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating habit names: {ex}");
            }
        }

        public HashSet<string> InitializeDatabase()
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            HashSet<string> habits = new();

            string query = "SELECT habit_name FROM Habits";
            List<string> habitRecords = connection.Query<string>(query).AsList();

            foreach (string record in habitRecords)
            {
                habits.Add(record);
            }

            return habits;
        }

        public string GetStatName(string habitName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string statQuery = $"SELECT stat_name FROM Habits WHERE habit_name = @HabitName";
            try
            {
                string result = connection.QueryFirst<string>(statQuery, new { HabitName = habitName });
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred: {ex}");
            }
        }

        public bool GetHabit(string habitName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string query = "SELECT habit_name FROM Habits";
            List<string> habitRecords = connection.Query<string>(query).AsList();
            return habitRecords.Count > 0;
        }
    }
}