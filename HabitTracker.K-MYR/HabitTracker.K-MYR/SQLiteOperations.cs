using HabitTracker.K_MYR.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.K_MYR

{
    class SQLiteOperations
    {
        private static readonly string connectionString = @"Data Source=habit-Tracker.db";

        static internal void CreateTableIfNotExists()
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS Habits
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Habit TEXT,
                        Measurement TEXT,
                        Date TEXT,
                        Quantity INTEGER
                    )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        internal static int Delete(int id)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE FROM Habits WHERE Id = '{id}'";

            return tableCmd.ExecuteNonQuery();
        }

        internal static void Insert(Habit habit, string date, int quantity)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO Habits (Habit, Measurement, Date, Quantity) VALUES ('{habit.Name}', '{habit.Measurement}','{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        internal static void Update(int id, string date, int quantity)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE Habits SET date = '{date}', quantity = {quantity} WHERE Id = {id}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        internal static List<Habit> GetAllHabits()
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT DISTINCT Habit, Measurement FROM Habits";

            List<Habit> habits = new();
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Name = reader.GetString(0),
                        Measurement = reader.GetString(1)
                    });
                }
            }
            connection.Close();
            return habits;
        }

        internal static List<HabitRecord> SelectAll()
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM Habits ORDER BY Date";

            List<HabitRecord> tableData = new();
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new HabitRecord
                    {
                        Id = reader.GetInt32(0),
                        Habit = reader.GetString(1),
                        Measurement = reader.GetString(2),
                        Date = DateTime.ParseExact(reader.GetString(3), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(4)
                    });
                }
            }
            connection.Close();
            return tableData;
        }

        internal static int[] SelectStatisticsByHabit(Habit habit)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"SELECT SUM(Quantity) FROM Habits WHERE Habit = '{habit.Name}';
                SELECT COUNT(*) FROM Habits WHERE Habit = '{habit.Name}';
                SELECT AVG(Quantity) FROM Habits WHERE Habit = '{habit.Name}';
                SELECT MAX(Quantity) FROM Habits WHERE Habit = '{habit.Name}'";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            reader.Read();
            int sum = reader.GetInt32(0);
            reader.NextResult();

            reader.Read();
            int numberOfTimes = reader.GetInt32(0);
            reader.NextResult();

            reader.Read();
            int average = reader.GetInt32(0);
            reader.NextResult();

            reader.Read();
            int max = reader.GetInt32(0);
            reader.NextResult();

            int[] results = { sum, numberOfTimes, average, max };

            return results;





        }

        internal static int RecordExists(int id)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM Habits WHERE Id = {id})";
            return Convert.ToInt32(checkCmd.ExecuteScalar());
        }
    }
}
