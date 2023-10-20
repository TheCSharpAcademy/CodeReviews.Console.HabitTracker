using System.Data;
using System.Data.SQLite;
using Dapper;
using StatLibrary;
using HabitLibrary;

namespace DatabaseHandler
{
    public sealed class Database
    {
        private readonly string _databaseName;
        private readonly string _connectionString;

        public Database(string databaseName)
        {
            _databaseName = databaseName;
            _connectionString = $"Data Source={_databaseName}.sqlite;Version=3;";
        }
        public void Create(string tableName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string createTableQuery = @$"CREATE TABLE {tableName} (name varchar(20), amount int)";

            if (TableExists(tableName))
            {
                throw new Exception("Habit already exists");
            }

            try
            {
                connection.Execute(createTableQuery);
            }
            catch (Exception ex)
            {
                throw new Exception("An unknown error occurred");
            }
        }
        public int Drop(string tableName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string dropTableQuery = $"DROP TABLE IF EXISTS {tableName}";
            int results = connection.Execute(dropTableQuery);
            return results;
        }
        public void Insert(string tableName, Stat stat)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string insertQuery = $"INSERT INTO {tableName} (name, amount) VALUES (@Name, @Value)";
            connection.Execute(insertQuery, new { Name = stat.name, Value = stat.value });
        }

        public void Read(string tableName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string readTableQuery = $"SELECT * FROM {tableName}";
            IEnumerable<dynamic> results = connection.Query(readTableQuery);

            Console.Clear();
            Console.WriteLine("Displaying detailed results...");
            Console.WriteLine("--------------------");
            Console.Write($"{tableName} | ");
            foreach (var row in results)
            {
                Console.WriteLine($"{row.name}: {row.amount} | ");
            }
            Console.WriteLine("--------------------");
        }

        public void UpdateHabitName(string tableName, string newName)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string updateNameQuery = $"ALTER TABLE {tableName} RENAME TO {newName}";

            if (TableExists(newName))
            {
                throw new Exception("Habit already exists");
            }

            try
            {
                connection.Execute(updateNameQuery);
            }
            catch
            {
                throw new Exception("An unknown error occurred.");
            }
        }

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

        public List<Habit> InitializeDatabase()
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            List<Habit> habits = new();

            string query = "SELECT name FROM sqlite_master WHERE type='table'";
            List<string> tableNames = connection.Query<string>(query).AsList();

            foreach (string tableName in tableNames)
            {
                Habit habit = new()
                {
                    Name = tableName,
                };

                string statNameQuery = $"SELECT name FROM {tableName}";
                string statValueQuery = $"SELECT amount FROM {tableName}";

                string statName = connection.QuerySingleOrDefault<string>(statNameQuery);
                int statValue = connection.QuerySingleOrDefault<int>(statValueQuery);

                Stat stat = new(statName, statValue);
                habit.Stat = stat;
                habits.Add(habit);
            }

            return habits;
        }
    }
}