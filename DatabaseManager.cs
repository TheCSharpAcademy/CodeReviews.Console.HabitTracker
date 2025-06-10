using System;
using System.Data.SQLite;

namespace TaskManager
{
    class DatabaseManager
    {
        static string connectionString = "Data Source=taskmanager.sqlite;Version=3;";

        public static void Initialize()
        {
            CreateTable();
        }

        public static void CreateTable()
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitName TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Date TEXT NOT NULL
                );
            ";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public static bool InsertHabits(string HabitName, int Quantity, string Date)
        {
            try
            {
                using var conn = new SQLiteConnection(connectionString);
                conn.Open();

                string sql = "INSERT INTO Habits (HabitName, Quantity, Date) VALUES (@HabitName, @Quantity, @Date)";
                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HabitName", HabitName);
                cmd.Parameters.AddWithValue("@Quantity", Quantity);
                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while inserting habit: {ex.Message}");
                return false;
            }

        }

        public static List<string> ListHabits()
        {
            List<string> habits = new List<string>();

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string sql = "SELECT Id, HabitName, Quantity, Date FROM Habits";
            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string habitName = reader.GetString(1);
                int quantity = reader.GetInt32(2);
                string date = reader.GetString(3);

                habits.Add($"Id: {id}, Habit: {habitName}, Quantity: {quantity}, Date: {date}");
            }

            return habits;
        }

        public static bool DeleteHabit(int id)
        {
            try
            {
                using var conn = new SQLiteConnection(connectionString);
                conn.Open();

                string sql = "DELETE FROM Habits WHERE Habits.Id == @HabitId;";
                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HabitId", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting habit: {ex.Message}");
                return false;
            }

        }

        public static bool UpdateHabit(int id, string habitName = null, int? quantity = null, string date = null)
        {
            List<string> parameters = new List<string>();
            Dictionary<string, object> camps = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(habitName))
            {
                parameters.Add("HabitName = @HabitName");
                camps.Add("@HabitName", habitName);
            }

            if (quantity != null)
            {
                parameters.Add("Quantity = @Quantity");
                camps.Add("@Quantity", quantity);
            }

            if (!String.IsNullOrEmpty(date))
            {
                parameters.Add("Date = @Date");
                camps.Add("@Date", date);
            }

            if (camps.Count == 0)
            {
                Console.WriteLine("Nothing to update.");
                return false;
            }

            string sql = $"UPDATE Habits SET {string.Join(", ", parameters)} WHERE Id = @Id";
            camps.Add("@Id", id);

            try
            {
                using var conn = new SQLiteConnection(connectionString);
                conn.Open();
                using var cmd = new SQLiteCommand(sql, conn);
                foreach (var p in camps)
                {
                    cmd.Parameters.AddWithValue(p.Key, p.Value);
                }
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating habit: {ex.Message}");
                return false;
            }
        }

        public static List<int?> getHabitsIDs()
        {
            List<int?> habitsId = new List<int?>();

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string sql = "SELECT Id FROM Habits";
            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);

                habitsId.Add(id);
            }

            return habitsId;
        }
    }
}