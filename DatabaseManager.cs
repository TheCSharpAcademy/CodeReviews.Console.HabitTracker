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
    }
}