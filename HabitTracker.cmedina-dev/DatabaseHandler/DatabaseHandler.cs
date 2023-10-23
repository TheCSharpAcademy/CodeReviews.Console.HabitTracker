using System.Data;
using System.Data.SQLite;
using Dapper;
using StatLibrary;
using HabitLibrary;
using System;

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

        private void TestRead()
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string testQuery = "SELECT habit_name, stat_name, stat_value, entry_timestamp FROM Habits";
            var result = connection.Query<Record>(testQuery);
            foreach (var item in result)
            {
                Console.WriteLine($"Habit Name: {item.habit_name}, Stat Name: {item.stat_name}, Stat Value: {item.stat_value} Date: {item.entry_timestamp}");
            }
            Console.ReadLine();
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

            TestRead();
        }

        // Delete ALL RecordIds matching the given HabitName column
        public int Drop(string tableName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            return 0;
        }

        // Return ALL entries of a given habit
        public void Read(string habitName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
        }

        // Overload - Return ALL entries for a habit on a specific date
        public void Read(string habitName, string date)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
        }

        // Overload - Return ALL entries between two dates (inclusive)
        public void Read(string habitName, string startDate, string endDate)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
        }

        // Update ALL RecordIds with matching HabitName column
        public void UpdateHabitName(string oldName, string newName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string updateNameQuery = $"UPDATE Habits SET habit_name = @HabitName WHERE habit_name = {oldName}";

            try
            {
                connection.Execute(updateNameQuery);
            }
            catch
            {
                throw new Exception("An error occurred while updating habit names.");
            }
        }

        // Update ALL RecordIds with matching HabitName AND StatName columns
        public void UpdateStatName(string tableName, string oldName, string newName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string updateNameQuery = $"UPDATE {tableName} SET name = @NewName WHERE name = @OldName";

            try
            {
                connection.Execute(updateNameQuery, new { NewName = newName, OldName = oldName });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR thrown: {ex}");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadLine();
            }
        }

        // Update specified RecordId based on HabitName and StatName with new StatValue
        public void UpdateStatValue(string tableName, string statName, int value)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string updateNameQuery = $"UPDATE {tableName} SET amount = @NewValue WHERE name = @StatName";

            try
            {
                connection.Execute(updateNameQuery, new { NewValue = value, StatName = statName });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR thrown: {ex}");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadLine();
                return;
            }
        }

        
        public bool TableExists(string tableName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string validateTableQuery = $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            int count = connection.QuerySingleOrDefault<int>(validateTableQuery);
            return count >= 1;
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
            string statQuery = $"SELECT stat_name FROM Habits WHERE habit_name = {habitName}";
            string result = connection.QueryFirst<string>(statQuery);
            return result;
        }
    }
}